using System;
using System.IO;
using System.Collections;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("KeepComponentXmlWriter", "KeepComponent.xml")]
	public class KeepComponentXmlWriter : ILogWriter
	{
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			using (StreamWriter s = new StreamWriter(stream))
			{
				int region = 0;
				Hashtable keeps = new Hashtable();
				s.WriteLine("<KeepComponent>");
				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i, log.Count - 1);

						Packet pak = log[i];
						if (pak is StoC_0x20_PlayerPositionAndObjectID_171)
						{
							StoC_0x20_PlayerPositionAndObjectID_171 reg20 = (StoC_0x20_PlayerPositionAndObjectID_171)pak;
							region = reg20.Region;
						}
						else if (pak is StoC_0xB7_RegionChange)
						{
							StoC_0xB7_RegionChange regB7 = (StoC_0xB7_RegionChange)pak;
							region = regB7.RegionId;
						}
						else if (pak is StoC_0x6C_KeepComponentOverview)
						{
							StoC_0x6C_KeepComponentOverview partCreate = (StoC_0x6C_KeepComponentOverview)pak;
//							if (region != 163) continue;
//							if (partCreate.KeepId < 256) continue;
							string key = "KEEPID-" + partCreate.KeepId + "-WALLID-" + partCreate.ComponentId;
							if (keeps.ContainsKey(key)) continue;
							keeps[key] = partCreate;
							s.WriteLine("  <KeepComponent>");
							s.WriteLine(string.Format("    <KeepComponent_ID>{0}</KeepComponent_ID>", key));
							s.WriteLine(string.Format("    <X>{0}</X>", (sbyte)partCreate.X));
							s.WriteLine(string.Format("    <Y>{0}</Y>", (sbyte)partCreate.Y));
							s.WriteLine(string.Format("    <Heading>{0}</Heading>", partCreate.Heading));
//							s.WriteLine(string.Format("    <Height>{0}</Height>", partCreate.Height)); // in DOL height calced depended on keep level
							s.WriteLine(string.Format("    <Health>{0}</Health>", /*partCreate.Health*/100));
							s.WriteLine(string.Format("    <Skin>{0}</Skin>", partCreate.Skin));
							s.WriteLine(string.Format("    <KeepID>{0}</KeepID>", partCreate.KeepId));
							s.WriteLine(string.Format("    <ID>{0}</ID>", partCreate.ComponentId));
							s.WriteLine("  </KeepComponent>");
						}
					}
				}
				s.WriteLine("</KeepComponent>");
			}
		}
	}
}
