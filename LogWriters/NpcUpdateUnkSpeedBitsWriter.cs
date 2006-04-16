using System;
using System.Collections;
using System.IO;
using PacketLogConverter.LogActions;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("NpcUpdateUnkSpeedBitsWriter", "*.txt")]
	public class NpcUpdateUnkSpeedBitsWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			TimeSpan baseTime = new TimeSpan(0);
			ArrayList oids = new ArrayList();
			Hashtable bitsByOid = new Hashtable();
			using (StreamWriter s = new StreamWriter(stream))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					StoC_0xA1_NpcUpdate npcUpdate = log[i] as StoC_0xA1_NpcUpdate;
					if (npcUpdate == null) continue;
					if ((npcUpdate.Temp & 0xF000) == 0) continue;
					if ((npcUpdate.Temp & 0x0FFF) != 0) continue;

					s.WriteLine(npcUpdate.ToHumanReadableString(baseTime));
					if (!oids.Contains(npcUpdate.NpcOid))
						oids.Add(npcUpdate.NpcOid);
					ArrayList bitsList = (ArrayList)bitsByOid[npcUpdate.NpcOid];
					if (bitsList == null)
					{
						bitsList = new ArrayList();
						bitsByOid[npcUpdate.NpcOid] = bitsList;
					}
					int bits = npcUpdate.Temp >> 12;
					if (!bitsList.Contains(bits))
						bitsList.Add(bits);
				}

				int regionId;
				int zoneId;
				SortedList oidInfo = ShowKnownOidAction.MakeOidList(log.Count-1, log, out regionId, out zoneId);
				s.WriteLine("\n\noids for region {0}, zone {1}\n", regionId, zoneId);
				foreach (DictionaryEntry entry in oidInfo)
				{
					ushort oid = (ushort)entry.Key;
					if (!oids.Contains(oid)) continue;
					ShowKnownOidAction.ObjectInfo objectInfo = (ShowKnownOidAction.ObjectInfo)entry.Value;
					s.Write("0x{0:X4}: ", oid);
					s.Write(objectInfo.ToString());
					foreach (int bits in (ArrayList)bitsByOid[oid])
					{
						s.Write("\t0b{0}", Convert.ToString(bits, 2));
					}
					s.WriteLine();
				}
			}
		}
	}
}
