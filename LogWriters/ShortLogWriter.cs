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
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			TimeSpan baseTime = new TimeSpan(0);
			using (StreamWriter s = new StreamWriter(stream))
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

						s.WriteLine(packet.ToHumanReadableString(baseTime, true));
					}
				}
			}
		}
	}
}
