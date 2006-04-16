using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes short human readable logs
	/// </summary>
	[LogWriter("DAOCLogger v3.0 binary log", "*.log", Priority=9000)]
	public class DaocLoggerV3BinaryLogWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			StringBuilder header = new StringBuilder("<", 64);
			using (BinaryWriter s = new BinaryWriter(stream, Encoding.ASCII))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					Packet packet = log[i];
					if (FilterManager.IsPacketIgnored(packet))
						continue;

					header.Length = 1;

					switch (packet.Direction)
					{
						case ePacketDirection.ClientToServer: header.Append("SEND "); break;
						case ePacketDirection.ServerToClient: header.Append("RECV "); break;
						default: header.Append("UNKN "); break;
					}

					switch (packet.Protocol)
					{
						case ePacketProtocol.TCP: header.Append("TCP "); break;
						case ePacketProtocol.UDP: header.Append("UDP "); break;
						default: header.Append("UNK "); break;
					}

					header.Append("Time: ")
						.Append(packet.Time.Hours.ToString("D2")).Append('h')
						.Append(packet.Time.Minutes.ToString("D2")).Append('m')
						.Append(packet.Time.Seconds.ToString("D2")).Append('s')
						.Append(packet.Time.Milliseconds.ToString("D3")).Append("ms");

					header.Append(" Code:0x").Append(packet.Code.ToString("X4"));

					header.Append(" Len: ").Append(packet.Length.ToString());

					header.Append('>');

					s.Write(header.ToString().ToCharArray());
					s.Write((byte)0x0D);
					s.Write((byte)0x0A);
					s.Write(packet.ToArray());
				}
			}
		}
	}
}
