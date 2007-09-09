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
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			using (StreamWriter s = new StreamWriter(stream))
			{
				Hashtable keeps = new Hashtable();
				int region = 0;
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					StoC_0x20_PlayerPositionAndObjectID_171 reg20 = log[i] as StoC_0x20_PlayerPositionAndObjectID_171;
					if (reg20 != null)
					{
						region = reg20.Region;
						continue;
					}
					StoC_0xB7_RegionChange regB7 = log[i] as StoC_0xB7_RegionChange;
					if (regB7 != null)
					{
						region = regB7.RegionId;
						continue;
					}
					StoC_0x69_KeepOverview keepCreate = log[i] as StoC_0x69_KeepOverview;
					if (keepCreate == null) continue;
					if (keeps.ContainsKey(keepCreate.KeepId)) continue;
					keeps[keepCreate.KeepId]=keepCreate;
					string keepName;
					if(region==163)
					{
						if(keepCreate.KeepId<256)
						{
							keepName="KEEP_";
							continue;
						}
						else
							keepName="TOWER_";
					}
					else
					{
						continue;
						if(keepCreate.KeepId<1000)
							keepName="KEEP_";
						else
							keepName="TOWER_";
					}
					keepName+=keepCreate.KeepId.ToString();
					s.WriteLine("  <Keep>");
					s.WriteLine(string.Format("    <Keep_ID>{0}</Keep_ID>",keepCreate.KeepId));
					s.WriteLine(string.Format("    <KeepID>{0}</KeepID>",keepCreate.KeepId));
					s.WriteLine(string.Format("    <Name>{0}</Name>",keepName));
					s.WriteLine(string.Format("    <Region>{0}</Region>",region));
					s.WriteLine(string.Format("    <X>{0}</X>",keepCreate.KeepX));
					s.WriteLine(string.Format("    <Y>{0}</Y>",keepCreate.KeepY));
					s.WriteLine("    <Z>0</Z>");
					s.WriteLine(string.Format("    <Heading>{0}</Heading>",keepCreate.Heading));
					s.WriteLine(string.Format("    <Realm>{0}</Realm>",keepCreate.Realm));
					s.WriteLine(string.Format("    <Level>{0}</Level>",keepCreate.Level));
					s.WriteLine("    <ClaimedGuildName />");
					s.WriteLine("    <LordID />");
					s.WriteLine("  </Keep>");
				}
			}
		}
	}
}
