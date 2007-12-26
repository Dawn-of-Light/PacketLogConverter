using System;
using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows all known Oids before selected packet
	/// </summary>
	[LogAction("Show combat statistics")]
	public class ShowPlayerCombatAction : AbstractEnabledAction
	{
		#region ILogAction Members

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public override bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.Logs[selectedPacket.LogIndex];
			Hashtable plrInfo = MakeCombatList(selectedPacket.PacketIndex, log);
			StringBuilder str = new StringBuilder();

			foreach (DictionaryEntry entry in plrInfo)
			{
				PlayerInfo plr = (PlayerInfo)entry.Value;
				if (plr != null)
				{
//					str.AppendFormat("key = {0}\n", entry.Key);
					int playerAttacks = plr.hit + plr.miss + plr.fumble + plr.failAttacks;
					int playerAttacked = plr.block + plr.evade + plr.parry + plr.attacked;
					if ((playerAttacks + playerAttacked) > 0)
					{
						str.AppendFormat("Level:{0} Name:{1} Class:{2} WS:{3} WeaponDamage:{4}\n", plr.level, plr.name, plr.className, plr.weaponSkill, plr.weaponDamage);
						str.AppendFormat("player->target\tHits:{0} ({8:0.00}%)\t Miss:{1} ({9:0.00}%)\t Fumble:{2} ({10:0.00}%)\t FailAttacks:{3} ({11:0.00}%)\ntarget->player\tParry:{4} ({12:0.00}%)\t Block:{5} ({13:0.00}%)\t Evade:{6} ({14:0.00}%)\t attacked:{7} ({15:0.00}%)\n\n",
							plr.hit, plr.miss, plr.fumble, plr.failAttacks, plr.parry, plr.block, plr.evade, plr.attacked,
							(plr.hit + plr.miss) > 0 ? 100.0 * plr.hit / (plr.hit + plr.miss) : 0,
							(plr.hit + plr.miss ) > 0 ? 100.0 * plr.miss / (plr.hit + plr.miss): 0,
							playerAttacks > 0 ? 100.0 * plr.fumble / playerAttacks : 0,
							playerAttacks > 0 ? 100.0 * plr.failAttacks / playerAttacks : 0,
							(playerAttacked - plr.evade) > 0 ? 100.0 * plr.parry / (playerAttacked - plr.evade) : 0,
							(playerAttacked - plr.parry - plr.evade) > 0 ? 100.0 * plr.block / (playerAttacked - plr.parry - plr.evade): 0,
							playerAttacked > 0 ? 100.0 * plr.evade / playerAttacked: 0,
							playerAttacked > 0 ? 100.0 * plr.attacked / playerAttacked: 0);
					}
				}
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Show weapon use statistics (right click to close)";
			infoWindow.Width = 550;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		public static Hashtable MakeCombatList(int selectedIndex, PacketLog log)
		{
			Hashtable plrInfo = new Hashtable();
			PlayerInfo playerInfo = null;// = new PlayerInfo();
			int playerOid = -1;
			string nameKey = "";
			string statKey = "";
			string plrName = "";
			string plrClass = "";
			int plrLevel = 0;
			for (int i = 0; i < selectedIndex; ++i)
			{
				Packet pak = log[i];
				if (pak is StoC_0x20_PlayerPositionAndObjectID)
				{
					StoC_0x20_PlayerPositionAndObjectID plr = (StoC_0x20_PlayerPositionAndObjectID)pak;
					playerOid = plr.PlayerOid;
				}
				else if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate)pak;
					if (stat.SubCode == 3)
					{
						StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate)stat.SubData;
						nameKey = "N:" + subData.playerName + "L:" + subData.playerLevel;
						statKey = "";
						plrName = subData.playerName;
						plrLevel = subData.playerLevel;
						plrClass = subData.className;
					}
					else if (stat.SubCode == 5)
					{
						StoC_0x16_VariousUpdate.PlayerStateUpdate subData = (StoC_0x16_VariousUpdate.PlayerStateUpdate)stat.SubData;
						string key = string.Format("WD:{0}.{1}WS:{2}", subData.weaponDamageHigh, subData.weaponDamageLow, (subData.weaponSkillHigh << 8) + subData.weaponSkillLow);
						if (nameKey != "")
						{
							if(plrInfo.ContainsKey(nameKey + key))
							{
								playerInfo = (PlayerInfo)plrInfo[nameKey + key];
							}
							else
							{
								playerInfo = new PlayerInfo();
								playerInfo.name = plrName;
								playerInfo.level = plrLevel;
								playerInfo.className = plrClass;
								playerInfo.weaponDamage = string.Format("{0,2}.{1,-3}", subData.weaponDamageHigh, subData.weaponDamageLow);
								playerInfo.weaponSkill = (subData.weaponSkillHigh << 8) + subData.weaponSkillLow;
								plrInfo.Add(nameKey + key, playerInfo);
							}
							plrInfo[nameKey + key] = playerInfo;
						}
						statKey = key;
					}
				}
				else if (pak is StoC_0xBC_CombatAnimation && (playerInfo != null))
				{
					StoC_0xBC_CombatAnimation combat = (StoC_0xBC_CombatAnimation)pak;
					if (combat.Stance == 0 && combat.AttackerOid == playerOid && combat.DefenderOid != 0)
					{
						switch (combat.Result & 0x7F)
						{
							case 0:
								playerInfo.miss++;
								break;
							case 1:
							case 2:
							case 3:
								playerInfo.failAttacks++;
								break;
							case 4:
								playerInfo.fumble++;
								break;
							case 0xA:
							case 0xB:
								playerInfo.hit++;
								break;
							default:
							break;
						}
					}
					else if (combat.AttackerOid != 0 && combat.DefenderOid == playerOid)
					{
						switch (combat.Result & 0x7F)
						{
							case 1:
								playerInfo.parry++;
								break;
							case 2:
								playerInfo.block++;
								break;
							case 3:
								playerInfo.evade++;
								break;
							case 0x0:
							case 0xA:
							case 0xB:
								playerInfo.attacked++;
								break;
							default:
							break;
						}
					}
				}
/*				else if (pak is StoC_0xAF_Message)
				{
					StoC_0xAF_Message msg = (StoC_0xAF_Message)pak;
					switch (msg.Type)
					{
						case 0x10: // Your cast combat
						case 0x11: // Your Melee combat
						case 0x1B: // resist
						case 0x1D: // X hits you
						case 0x1E: // X miss you
							break;
						default:
							break;
					}
				}*/
			}
			if (nameKey != "" && statKey != "")
				plrInfo[nameKey + statKey] = playerInfo;
			return plrInfo;
		}

		#endregion

		public class PlayerInfo
		{
			public string className;
			public int level;
			public string name;
			public string weaponDamage;
			public int weaponSkill;
			public int hit;
			public int miss;
			public int fumble;
			public int failAttacks;
			public int parry;
			public int block;
			public int evade;
			public int attacked;
		}
	}
}
