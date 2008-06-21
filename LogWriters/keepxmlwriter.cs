using System;
using System.IO;
using PacketLogConverter.LogPackets;
using System.Collections;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("KeepXmlWriter", "keep.xml")]
	public class KeepXmlWriter : ILogWriter
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
				Hashtable keeps = new Hashtable();
				int region = 0;
				s.WriteLine("<Keep>");
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
						else if (pak is StoC_0x69_KeepOverview)
						{
							StoC_0x69_KeepOverview keepCreate = (StoC_0x69_KeepOverview)pak;
							if (keeps.ContainsKey(keepCreate.KeepId)) continue;
							keeps[keepCreate.KeepId] = keepCreate;
							string keepName;
							if (region == 163)
							{
								if (keepCreate.KeepId < 256)
								{
									keepName = "KEEP_";
//									continue;
								}
								else
									keepName = "TOWER_";
							}
							else
							{
//								continue;
								if (keepCreate.KeepId < 1000)
									keepName = "KEEP_";
								else
									keepName = "TOWER_";
							}
							keepName += keepCreate.KeepId.ToString();
							s.WriteLine("  <Keep>");
							s.WriteLine(string.Format("    <Keep_ID>{0}</Keep_ID>", keepCreate.KeepId));
							s.WriteLine(string.Format("    <KeepID>{0}</KeepID>", keepCreate.KeepId));
							s.WriteLine(string.Format("    <Name>{0}</Name>", keepName));
							s.WriteLine(string.Format("    <Region>{0}</Region>", region));
							s.WriteLine(string.Format("    <X>{0}</X>", keepCreate.KeepX));
							s.WriteLine(string.Format("    <Y>{0}</Y>", keepCreate.KeepY));
							s.WriteLine("    <Z>0</Z>");
							s.WriteLine(string.Format("    <Heading>{0}</Heading>", keepCreate.Heading));
							s.WriteLine(string.Format("    <Realm>{0}</Realm>", keepCreate.Realm));
							s.WriteLine(string.Format("    <Level>{0}</Level>", keepCreate.Level));
							s.WriteLine("    <ClaimedGuildName />");
							s.WriteLine("    <AlbionDifficultyLevel>0</AlbionDifficultyLevel>");
							s.WriteLine("    <MidgardDifficultyLevel>0</MidgardDifficultyLevel>");
							s.WriteLine("    <HiberniaDifficultyLevel>0</HiberniaDifficultyLevel>");
							s.WriteLine("    <OriginalRealm>0</OriginalRealm>");
							s.WriteLine("    <KeepType>0</KeepType>");
							s.WriteLine("    <BaseLevel>0</BaseLevel>");
							s.WriteLine("  </Keep>");
						}
					}
				}
				s.WriteLine("</Keep>");
			}
		}
	}
}
