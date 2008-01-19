using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogReaders
{
	/// <summary>
	/// Parse v3.0+ text logs
	/// </summary>
	[LogReader("DAOCLogger v3.0 text logs", "*.log", Priority=10000)]
	public class DaocLoggerV3TextLogReader : ILogReader
	{
		public ICollection<Packet> ReadLog(Stream stream, ProgressCallback callback)
		{
			List<Packet> packets = new List<Packet>((int)(stream.Length / 128));

			int counter = 0;
			string line = null;
			Packet pak = null;
			PacketLog log = new PacketLog();
			int dataBytesCount = -1;
			ArrayList ignoredPacketHeaders = new ArrayList();
			string header = "";
			
			using(StreamReader s = new StreamReader(stream, Encoding.ASCII))
//			using(BufferedStringReader strings = new BufferedStringReader(stream, 16)) // precache 65k strings
			{
				try
				{
					while ((line = s.ReadLine()) != null)
					{
						if (callback != null && (counter++ & 0x1FFF) == 0) // update progress every 8k lines
							callback((int)(stream.Position>>10),
							         (int)(stream.Length>>10));

						if (line.Length < 1) continue;

						if (line[0] == '<')
						{
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
									ignoredPacketHeaders.Add(header);
								}
							}

							header = line;

							try
							{
								int code;
								ePacketDirection dir;
								ePacketProtocol prot;
								TimeSpan time;
								dataBytesCount = ParseHeader(line, out code, out dir, out prot, out time);
								
								if (dataBytesCount < 0)
								{
									pak = null;
									ignoredPacketHeaders.Add(header);
								}
								else
								{
									pak = PacketManager.CreatePacket(log.Version, (byte)code, dir, dataBytesCount);
									pak.Code = (byte)code;
									pak.Direction = dir;
									pak.Protocol = prot;
									pak.Time = time;
								}
							}
							catch
							{
								pak = null;
								ignoredPacketHeaders.Add(header);
							}

							continue;
						}

						if (pak == null)
							continue;

						ParseDataLine(line, pak);
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
						ignoredPacketHeaders.Add(header);
					}
				}
			}

			if (ignoredPacketHeaders.Count > 0)
			{
				StringBuilder ignored = new StringBuilder("Ignored packets: "+ignoredPacketHeaders.Count+"\n\n");
				for (int i = 0; i < ignoredPacketHeaders.Count; i++)
				{
					string s = (string)ignoredPacketHeaders[i];
					ignored.Append('\n').Append(s);
					if (i > 15)
					{
						ignored.Append("\n...");
						break;
					}
				}
				Log.Info(ignored.ToString());
			}

			return packets;
		}

		public static void ParseDataLine(string line, Packet pak)
		{
			try
			{
				for(int i = 0; i<16*3; i+=3)
				{
					if (line.Length < i+2)
						break;

					if (line[i] == ' ')
						return;

					int val;
					if (!Util.ParseHexFast(line, i, 2, out val))
						return;
					pak.WriteByte((byte)val);
				}
			}
			catch
			{}
		}

		public static int ParseHeader(string line, out int code, out ePacketDirection dir, out ePacketProtocol prot, out TimeSpan time)
		{
			code = Convert.ToInt32(line.Substring(38, 4), 16);

			if (string.Compare(line, 1, "SEND", 0, 4, false) == 0)
				dir = ePacketDirection.ClientToServer;
			else if (string.Compare(line, 1, "RECV", 0, 4, false) == 0)
				dir = ePacketDirection.ServerToClient;
			else
				throw new Exception("unknown packet direction: "+line.Substring(1, 4));

			if (string.Compare(line, 6, "TCP", 0, 3, false) == 0)
				prot = ePacketProtocol.TCP;
			else if (string.Compare(line, 6, "UDP", 0, 3, false) == 0)
				prot = ePacketProtocol.UDP;
			else
				throw new Exception("unknown packet protocol: "+line.Substring(6, 3));

			int hours, minutes, seconds, milliseconds;
			if (Util.ParseDecFast(line, 16, 2, out hours)
			    && Util.ParseDecFast(line, 19, 2, out minutes)
			    && Util.ParseDecFast(line, 22, 2, out seconds)
			    && Util.ParseDecFast(line, 25, 3, out milliseconds))
			{
				time = new TimeSpan(0, hours, minutes, seconds, milliseconds);
			}
			else
			{
				time = TimeSpan.Zero;
				return -1;
			}
			
			int pakSize;
			if (!Util.ParseDecFast(line, 48, line.Length - 49, out pakSize))
				return -1;

			return pakSize;
		}
	}
}
