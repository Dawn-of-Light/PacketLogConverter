using System;
using System.IO;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes short human readable logs
	/// </summary>
	[LogWriter("Human readable log", "*.txt", Priority=10000)]
	public class ShortLogWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			TimeSpan baseTime = new TimeSpan(0);
			using (StreamWriter s = new StreamWriter(stream))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					Packet packet = log[i];
					if (FilterManager.IsPacketIgnored(packet))
						continue;

					s.WriteLine(packet.ToHumanReadableString(baseTime, true));
				}
			}
		}
	}
}
