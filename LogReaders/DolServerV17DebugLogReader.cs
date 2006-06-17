using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PacketLogConverter.LogReaders
{
	/// <summary>
	/// Parse DOL server v1.7 logs
	/// </summary>
	[LogReader("DOL Server v1.7 debug logs", "*.log", Priority=8000)]
	public class DolServerV17DebugLogReader : ILogReader
	{
		public ICollection ReadLog(Stream stream, ProgressCallback callback)
		{
			ArrayList packets = new ArrayList();

			int index = 0;
			int currentVersion = -1;
			int counter = 0;
			string line = null;
			Packet pak = null;
			PacketLog log = new PacketLog();
			ArrayList badLines = new ArrayList();
			byte[] data;

			using(StreamReader s = new StreamReader(stream, Encoding.ASCII))
			{
				while ((line = s.ReadLine()) != null)
				{
					try
					{
						if (callback != null && (counter++ & 0x1FFF) == 0) // update progress every 8k lines
							callback((int)(stream.Position>>10),
								(int)(stream.Length>>10));

						if (line.Length < 1) continue;
						if (line.IndexOf("PacketHandler.PacketProcessor - ===>") >= 0)
						{
							int code = line.IndexOf("> Packet 0x");
							if (code >= 0 && Util.ParseHexFast(line, code+11, 2, out code))
							{
								data = ReadPacketData(s);
								pak = new Packet(data.Length);
								pak.Direction = ePacketDirection.ClientToServer;
								pak.Protocol = ePacketProtocol.TCP;
								pak.Code = code;
								pak.Time = ParseTime(line);
								pak.Write(data, 0, data.Length);
							}
						}
						else if (line.IndexOf("PacketHandler.PacketProcessor - <===") >= 0)
						{
							int code = line.IndexOf("> Packet 0x");
							if (code >= 0 && Util.ParseHexFast(line, code+11, 2, out code))
							{
								data = ReadPacketData(s);
								if (data.Length < 3)
									continue;
								pak = new Packet(data.Length - 3);
								pak.Protocol = ePacketProtocol.TCP; // can't detect protocol
								pak.Direction = ePacketDirection.ServerToClient;
								pak.Code = code;
								pak.Time = ParseTime(line);
								pak.Write(data, 3, data.Length - 3);
							}
							else
							{
								badLines.Add("not GSPacketIn?  : " + line);
								continue;
							}
						}
						else
						{
							continue;
						}

						pak = PacketManager.ChangePacketClass(pak, currentVersion);
//						pak.AllowClassChange = false;
						packets.Add(pak);
					}
					catch (Exception e)
					{
						MessageBox.Show(e.ToString());
						badLines.Add(e.GetType().FullName + ": " + line);
					}
				}
			}

			if (badLines.Count > 0)
			{
				StringBuilder str = new StringBuilder("error parsing following lines (" + badLines.Count + "):\n\n");
				int i = 0;
				foreach (string s in badLines)
				{
					str.Append(s).Append('\n');
					if (++i > 15)
					{
						str.Append("...\n").Append(badLines.Count - i).Append(" lines more.\n");
						break;
					}
				}
				Log.Info(str.ToString());
			}

			return packets;
		}

		private byte[] ReadPacketData(StreamReader s)
		{
			MemoryStream buf = new MemoryStream();
			string line;

			while ((line = s.ReadLine()) != null)
			{
				int dataOffset = 6;
				int byteInLine = 0;
				do
				{
					int i;
					if (!Util.ParseHexFast(line, dataOffset, 2, out i))
					{
						buf.Capacity = (int) buf.Length;
						return buf.GetBuffer();
					}
					buf.WriteByte((byte) i);
					byteInLine++;
					dataOffset += 3;
				} while (byteInLine < 16);
			}

			buf.Capacity = (int) buf.Length;
			return buf.GetBuffer();
		}

		private TimeSpan ParseTime(string line)
		{
			int hours, minutes, seconds, milliseconds;
			if (line[2] == ':' && line[5] == ':' && line[8] == ',')
			{
				if (Util.ParseDecFast(line, 0, 2, out hours)
					&& Util.ParseDecFast(line, 3, 2, out minutes)
					&& Util.ParseDecFast(line, 6, 2, out seconds)
					&& Util.ParseDecFast(line, 9, 3, out milliseconds))
				{
					return new TimeSpan(0, hours, minutes, seconds, milliseconds);
				}
			}
			else if (line[4] == '-' && line[7] == '-' && line[10] == ' ' && line[13] == ':' && line[16] == ':' && line[19] == ',')
			{
				if (Util.ParseDecFast(line, 11, 2, out hours)
					&& Util.ParseDecFast(line, 14, 2, out minutes)
					&& Util.ParseDecFast(line, 17, 2, out seconds)
					&& Util.ParseDecFast(line, 20, 3, out milliseconds))
				{
					return new TimeSpan(0, hours, minutes, seconds, milliseconds);
				}
			}
			return TimeSpan.Zero;
		}
	}
}
