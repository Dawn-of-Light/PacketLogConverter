using System;
using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes short human readable logs
	/// </summary>
	[LogWriter("DAOCLogger v3.0 text log", "*.log", Priority=9100)]
	public class DaocLoggerV3TextLogWriter : ILogWriter
	{
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			StringBuilder header = new StringBuilder("<", 64);
			StringBuilder rawData = new StringBuilder(64);
			ArrayList lineBytes = new ArrayList();

			using (BinaryWriter s = new BinaryWriter(stream, Encoding.ASCII))
			{
				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i, log.Count - 1);

						Packet packet = log[i];
						if (FilterManager.IsPacketIgnored(packet))
							continue;

						header.Length = 1;

						AppendHeader(header, packet);

						s.Write(header.ToString().ToCharArray());
						s.Write((byte) 0x0D);
						s.Write((byte) 0x0A);

						packet.Position = 0;
						while (packet.Position < packet.Length)
						{
							int byteCount = rawData.Length = 0;
							lineBytes.Clear();
							for (; byteCount < 16 && packet.Position < packet.Length; byteCount++)
							{
								int b = packet.ReadByte();
								rawData.Append(b.ToString("X2")).Append(' ');
								lineBytes.Add((byte) b);
							}

							s.Write(string.Format("{0,-50}", rawData).ToCharArray());
							for (int j = 0; j < lineBytes.Count; j++)
							{
								byte lineByte = (byte) lineBytes[j];
								if (lineByte < 32)
									lineByte = (byte) '.';
								s.Write(lineByte);
							}
							s.Write((byte) 0x0D);
							s.Write((byte) 0x0A);
						}

						s.Write((byte) 0x0D);
						s.Write((byte) 0x0A);
					}
				}
			}
		}

		public static void AppendHeader(StringBuilder header, Packet packet)
		{
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
		}
	}
}
