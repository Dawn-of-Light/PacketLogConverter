#define SHOW_PACKETS
using System.Collections;
using System.IO;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("Skill writer", "*.txt")]
	public class SkillWriter : ILogWriter
	{
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			int playerOid = -1;
			byte playerLevel = 0;
			string playerClassName = "";
			Skills playerSkills = new Skills();
			
			using (StreamWriter s = new StreamWriter(stream))
			{
				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i, log.Count - 1);

						Packet pak = log[i];
						if (pak is StoC_0x20_PlayerPositionAndObjectID)
						{
							StoC_0x20_PlayerPositionAndObjectID plr = (StoC_0x20_PlayerPositionAndObjectID) pak;
							playerOid = plr.PlayerOid;
#if SHOW_PACKETS
							s.WriteLine("playerOid:0x{0:X4}", playerOid);
#endif
						}
						else if (pak is StoC_0x16_VariousUpdate)
						{
							// Name, level, class
							StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate) pak;
							if (stat.SubCode == 3)
							{
								StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate) stat.SubData;
								playerLevel = subData.playerLevel;
								playerClassName = subData.className;
#if SHOW_PACKETS
								s.WriteLine("{0, -16} 0x16:3 class:{1} level:{2}", pak.Time.ToString(), playerClassName, playerLevel);
#endif
							}
						}
						else if (pak is StoC_0xAF_Message)
						{
							StoC_0xAF_Message msg = (StoC_0xAF_Message) pak;
#if SHOW_PACKETS
//							s.WriteLine("{0, -16} 0xAF 0x{1:X2} {2}", pak.Time.ToString(), msg.Type, msg.Text);
#endif
						}
					}
				}
			}
		}

		public class Skills
		{
			public Spell[] spells;
		}

		public class Spell
		{
			public string specialization;
			public byte level;
			public ushort icon;
			public string name;
		}
	}
}
