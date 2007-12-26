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
	[LogReader("stats example", "*.log", Priority=-1000)]
	public class DaocLoggerV3TextLogReaderStats : ILogReader
	{
		private int packetsCount;
		private ReportForm report;


		public DaocLoggerV3TextLogReaderStats()
		{
			report = new ReportForm();
			report.buttonReset.Click += new EventHandler(ResetClick);
			MainForm.Instance.FilesLoaded += new MainForm.LogReaderDelegate(ShowReport);
		}

		public ICollection<Packet> ReadLog(Stream stream, ProgressCallback callback)
		{
			int counter = 0;
			string line = null;
			Packet pak = null;
			PacketLog log = new PacketLog();
			int dataBytesCount = -1;
			ArrayList ignoredPacketHeaders = new ArrayList();
			string header = "";
			
			using(StreamReader s = new StreamReader(stream, Encoding.ASCII))
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
									/// Completed packet!
									
									
									
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
								dataBytesCount = DaocLoggerV3TextLogReader.ParseHeader(line, out code, out dir, out prot, out time);
								
								if (dataBytesCount < 0)
								{
									pak = null;
									ignoredPacketHeaders.Add(header);
								}
								else
								{
									// header parsed successfully!
									if (code == 0xa9)
									{
										packetsCount++; // get statistics
										pak = null; // ignore the data
									}
									continue;
									
/*									pak = PacketManager.CreatePacket(log.Version, code, dir, dataBytesCount);
									pak.Code = code;
									pak.Direction = dir;
									pak.Protocol = prot;
									pak.Time = time;*/
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

						DaocLoggerV3TextLogReader.ParseDataLine(line, pak);
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
						// last completed packet!
						
						
						
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

			return new List<Packet>(0);
		}
		
		private void ShowReport(ILogReader reader)
		{
			if (reader != this)
				return;
			UpdateReport();
			report.ShowDialog();
		}

		private void UpdateReport()
		{
			report.Data.Text = string.Format("count of 0xA9 packets: {0:N0}\n", packetsCount);
		}
		
		private void ResetClick(object sender, EventArgs e)
		{
			packetsCount = 0;
			UpdateReport();
		}
	}
}
