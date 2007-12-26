using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogReaders
{
	/// <summary>
	/// Parse v3.0+ binary logs
	/// </summary>
	[LogReader("DAOCLogger v3.0 binary logs", "*.log", Priority=9900)]
	public class DaocLoggerV3BinaryLogReader : ILogReader
	{
		public ICollection<Packet> ReadLog(Stream stream, ProgressCallback callback)
		{
			List<Packet> packets = new List<Packet>((int)(stream.Length / 128));

			int counter = 0;
			Packet pak = null;
			PacketLog log = new PacketLog();
			int dataBytesCount = -1;
			int ignoredCount = 0;
			StringBuilder header = new StringBuilder('<', 64);

			using(BinaryReader s = new BinaryReader(stream, Encoding.ASCII))
			{
				try
				{
					while (s.BaseStream.Position < s.BaseStream.Length)
					{
						if (callback != null && (counter++ & 0xFFF) == 0) // update progress every 4096th packet
							callback((int)(s.BaseStream.Position>>10), (int)(s.BaseStream.Length>>10));

						if (s.ReadChar() != '<')
							continue;

						if (pak != null)
						{
							if (pak.Length == dataBytesCount)
							{
								packets.Add(pak);
								try { pak.InitLog(log); }
								catch {}
							}
							else
							{
								ignoredCount++;
							}
						}

						// get the header string
						header.Length = 1;
						header.Append(s.ReadChars(48));
						for(;;)
						{
							char curChar = s.ReadChar();
							if (curChar == '<')
							{
								s.BaseStream.Seek(-1, SeekOrigin.Current);
								break;
							}
							header.Append(curChar);
							if (curChar == '>')
							{
								s.BaseStream.Seek(2, SeekOrigin.Current);
								break;
							}
							if (header.Length > 128)
							{
								break;
							}
						}

						try
						{
							int code;
							ePacketDirection dir;
							ePacketProtocol prot;
							TimeSpan time;
							dataBytesCount = DaocLoggerV3TextLogReader.ParseHeader(header.ToString(), out code, out dir, out prot, out time);

							pak = PacketManager.CreatePacket(log.Version, code, dir, dataBytesCount);
							pak.Code = code;
							pak.Direction = dir;
							pak.Protocol = prot;
							pak.Time = time;
						}
						catch
						{
							pak = null;
							ignoredCount++;
//							MessageBox.Show("parse header failed:\nposition="+s.BaseStream.Position+"\n'"+header.ToString()+"'\n");
						}

						if (pak == null)
							continue;

						byte[] pakData = s.ReadBytes(dataBytesCount);
						pak.Write(pakData, 0, pakData.Length);
					}
				}
				catch (Exception e)
				{
					Log.Error("reading file", e);
				}

				if (pak != null)
				{
					if (pak.Length == dataBytesCount)
					{
						packets.Add(pak);
					}
					else
					{
						ignoredCount++;
					}
				}
			}

			if (ignoredCount > 0)
				Log.Info("ignored packets: " + ignoredCount);

			return packets;
		}
	}
}
