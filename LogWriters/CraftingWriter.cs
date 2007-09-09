using System.IO;
using System.Text;
using System.Collections;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("Crafting writer", "*.txt")]
	public class CraftingWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			StoC_0x16_VariousUpdate.CraftingSkillsUpdate lastCraftingSkill = null;
			StoC_0xF3_TimerWindow lastCraftBeginTimerPacket = null;
			StoC_0xF3_TimerWindow lastCloseTimerPacket = null;
			ushort lastReceipId = 0;
			SortedList recpToSkill = new SortedList();
			//Alb receipts
			recpToSkill.Add(1, 4);		// Alchemy
			recpToSkill.Add(701, 2);	// Armorcraft
			recpToSkill.Add(1401, 8);	// Clothworking
			recpToSkill.Add(1801, 15);	// Basic Crafting
			recpToSkill.Add(2051, 12);	// Fletching
			recpToSkill.Add(2751, 7);	// Leatherworking
			recpToSkill.Add(3001, 11);	// Tailoring
			recpToSkill.Add(3451, 6);	// Metalworking
			recpToSkill.Add(4165, 3);	// Siegecraft
			recpToSkill.Add(4201, 13);	// Spellcraft
			recpToSkill.Add(4901, 11);	// Tailoring
			recpToSkill.Add(5601, 1);	// Weaponcrafting
			recpToSkill.Add(6501, 14);	// Woodworking
			//Mid receipts
			recpToSkill.Add(7001, 4);	// Alchemy
			recpToSkill.Add(7701, 2);	// Armorcraft
			recpToSkill.Add(8401, 8);	// Clothworking
			recpToSkill.Add(8801, 15);	// Basic Crafting
			recpToSkill.Add(9051, 12);	// Fletching
			recpToSkill.Add(9751, 7);	// Leatherworking
			recpToSkill.Add(10001, 11);	// Tailoring
			recpToSkill.Add(10451, 6);	// Metalworking
			recpToSkill.Add(11165, 3);	// Siegecraft
			recpToSkill.Add(11201, 13);	// Spellcraft
			recpToSkill.Add(11901, 11);	// Tailoring
			recpToSkill.Add(12601, 1);	// Weaponcrafting
			recpToSkill.Add(13301, 14);	// Woodworking
			//Hib receipts
			recpToSkill.Add(14001, 4);	// Alchemy
			recpToSkill.Add(14701, 2);	// Armorcraft
			recpToSkill.Add(15401, 8);	// Clothworking
			recpToSkill.Add(15801, 15);	// Basic Crafting
			recpToSkill.Add(16051, 12);	// Fletching
			recpToSkill.Add(16751, 7);	// Leatherworking
			recpToSkill.Add(17001, 11);	// Tailoring
			recpToSkill.Add(17451, 6);	// Metalworking
			recpToSkill.Add(18165, 3);	// Siegecraft
			recpToSkill.Add(18201, 13);	// Spellcraft
			recpToSkill.Add(18901, 11);	// Tailoring
			recpToSkill.Add(19601, 1);	// Weaponcrafting
			recpToSkill.Add(20301, 14);	// Woodworking
			using (StreamWriter s = new StreamWriter(stream))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					Packet pak = log[i];
					if (pak is StoC_0x16_VariousUpdate)
					{
						StoC_0x16_VariousUpdate stat = pak as StoC_0x16_VariousUpdate;
						if (stat.SubCode == 8)
						{
							lastCraftingSkill = (StoC_0x16_VariousUpdate.CraftingSkillsUpdate)stat.SubData;
						}
					}
					else if (pak is CtoS_0xED_CraftItem)
					{
						CtoS_0xED_CraftItem craft = pak as CtoS_0xED_CraftItem;
						lastReceipId = craft.ReceptId;
					}
					else if (pak is StoC_0xF3_TimerWindow)
					{
						StoC_0xF3_TimerWindow timer = pak as StoC_0xF3_TimerWindow;
						if (timer.Flag == 1 && timer.Message.StartsWith("Making: "))
							lastCraftBeginTimerPacket = timer;
						else if (timer.Flag == 0)
							lastCloseTimerPacket = timer;
					}
					else if (pak is StoC_0xAF_Message)
					{
						StoC_0xAF_Message message = pak as StoC_0xAF_Message;
						if (message.Text.StartsWith("You successfully make "))
						{
							byte craftSkill = (byte)FindSkill(recpToSkill, lastReceipId);
//							StringBuilder str = new StringBuilder();
//							lastCraftingSkill.MakeString(str, true);
//							s.WriteLine(str);
							bool found = false;
							foreach(StoC_0x16_VariousUpdate.CraftingSkill cs in lastCraftingSkill.skills)
							{
								if (cs.icon == craftSkill)
								{
									s.WriteLine(string.Format("{0}, crafting skill:{1}({3}:{4}) receptId:{2} (time:{5}, pakDiffTime:{6})",
										message.Text, craftSkill, lastReceipId, cs.name, cs.points, lastCraftBeginTimerPacket.Timer, lastCloseTimerPacket.Time - lastCraftBeginTimerPacket.Time));
									found = true;
									break;
								}
							}
							if (!found)
								s.WriteLine(string.Format("{0}, crafting skill:{1} receptId:{2}", message.Text, craftSkill, lastReceipId));
						}
						else if (message.Text.StartsWith("You gain skill in"))
						{
							s.WriteLine(message.Text);
						}
					}
				}
			}
		}
		public static int FindSkill(SortedList list, int recept_id)
		{
			int old_value = -1;
			foreach (DictionaryEntry de in list)
			{
				if ((int)de.Key > recept_id)
					return old_value;
				old_value = (int)de.Value;
			}
			return old_value;
		}
	}
}
