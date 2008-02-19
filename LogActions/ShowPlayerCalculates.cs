using System;
using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player calculation", Priority=1)]
	public class ShowPlayerCalculation : AbstractEnabledAction
	{
		#region ILogAction Members

/*		public enum eCharacterClass : byte
		{
			Unknown = 0,
			Paladin = 1,
			Armsman = 2,
			Scout = 3,
			Minstrel = 4,
			Theurgist = 5,
			Cleric = 6,
			Wizard = 7,
			Sorcerer = 8,
			Infiltrator = 9,
			Friar = 10,
			Mercenary = 11,
			Necromancer = 12,
			Cabalist = 13,
			Fighter = 14,
			Elementalist = 15,
			Acolyte = 16,
			AlbionRogue = 17,
			Mage = 18,
			Reaver = 19,
			Disciple = 20,
			Thane = 21,
			Warrior = 22,
			Shadowblade = 23,
			Skald = 24,
			Hunter = 25,
			Healer = 26,
			Spiritmaster = 27,
			Shaman = 28,
			Runemaster = 29,
			Bonedancer = 30,
			Berserker = 31,
			Savage = 32,
			Heretic = 33,
			Valkyrie = 34,
			Viking = 35,
			Mystic = 36,
			Seer = 37,
			MidgardRogue = 38,
			Bainshee = 39,
			Eldritch = 40,
			Enchanter = 41,
			Mentalist = 42,
			Blademaster = 43,
			Hero = 44,
			Champion = 45,
			Warden = 46,
			Druid = 47,
			Bard = 48,
			Nightshade = 49,
			Ranger = 50,
			Magician = 51,
			Guardian = 52,
			Naturalist = 53,
			Stalker = 54,
			Animist = 55,
			Valewalker = 56,
			Forester = 57,
			Vampiir = 58,
			Warlock = 59,
			AlbdMauler = 60,
			MidMauler = 61,
			HibdMauler = 62,
		}*/

		public static readonly object[][] ClassInfo =
		{
			// [realm, className, BaseHP]
			new object[3] {0, "Unknown", 0},
			new object[3] {1, "Paladin", 760},
			new object[3] {1, "Armsman", 880},
			new object[3] {1, "Scout", 720},
			new object[3] {1, "Minstrel", 720},
			new object[3] {1, "Theurgist", 560},
			new object[3] {1, "Cleric", 720},
			new object[3] {1, "Wizard", 560},
			new object[3] {1, "Sorcerer", 560},
			new object[3] {1, "Infiltrator", 720},
			new object[3] {1, "Friar", 720},
			new object[3] {1, "Mercenary", 880},
			new object[3] {1, "Necromancer", 560},
			new object[3] {1, "Cabalist", 560},
			new object[3] {1, "Fighter", 880},
			new object[3] {1, "Elementalist", 560},
			new object[3] {1, "Acolyte", 720},
			new object[3] {1, "Rogue", 720}, //AlbionRogue
			new object[3] {1, "Mage", 560},
			new object[3] {1, "Reaver", 760},
			new object[3] {1, "Disciple", 560},
			new object[3] {2, "Thane", 720},
			new object[3] {2, "Warrior", 880},
			new object[3] {2, "Shadowblade", 760},
			new object[3] {2, "Skald", 760},
			new object[3] {2, "Hunter", 720},
			new object[3] {2, "Healer", 720},
			new object[3] {2, "Spiritmaster", 560},
			new object[3] {2, "Shaman", 720},
			new object[3] {2, "Runemaster", 560},
			new object[3] {2, "Bonedancer", 560},
			new object[3] {2, "Berserker", 880},
			new object[3] {2, "Savage", 880},
			new object[3] {1, "Heretic", 720},
			new object[3] {2, "Valkyrie", 760},// DOL value 720 is wrong !!!
			new object[3] {2, "Viking", 880},
			new object[3] {2, "Mystic", 560},
			new object[3] {2, "Seer", 720},
			new object[3] {2, "Rogue", 720}, //MidgardRogue
			new object[3] {3, "Bainshee", 560},
			new object[3] {3, "Eldritch", 560},
			new object[3] {3, "Enchanter", 560},
			new object[3] {3, "Mentalist", 560},
			new object[3] {3, "Blademaster", 880},
			new object[3] {3, "Hero", 880},
			new object[3] {3, "Champion", 760},
			new object[3] {3, "Warden", 720},
			new object[3] {3, "Druid", 720},
			new object[3] {3, "Bard", 720},
			new object[3] {3, "Nightshade", 720},
			new object[3] {3, "Ranger", 720},
			new object[3] {3, "Magician", 560},
			new object[3] {3, "Guardian", 880},
			new object[3] {3, "Naturalist", 720},
			new object[3] {3, "Stalker", 720},
			new object[3] {3, "Animist", 560},
			new object[3] {3, "Valewalker", 720},
			new object[3] {3, "Forester", 560},
			new object[3] {3, "Vampiir", 880},
			new object[3] {2, "Warlock", 560},
			new object[3] {1, "Mauler", 600},
			new object[3] {2, "Mauler", 600},
			new object[3] {3, "Mauler", 600},
		};
		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public override bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			int selectedIndex = selectedPacket.PacketIndex;
			int MaxHealth = -1;
			int level = -1;
			int champ_level = -1;
			string className = "";
			int classId = 0;
			bool flagFound = false;
			bool flagScarsOfBattleFound = false;
			bool flagSkillsChecked = false;
			StringBuilder str = new StringBuilder();
			StoC_0xFB_CharStatsUpdate_175 charStats = null;
			int RASkillHPBonus = 0;
			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0xFB_CharStatsUpdate_175)
				{
					if (charStats == null && (pak as StoC_0xFB_CharStatsUpdate_175).Flag != 0xFF)
					{
						charStats = pak as StoC_0xFB_CharStatsUpdate_175;
						if (MaxHealth == -1)
							MaxHealth = charStats.MaxHealth;
					}
				}
				else if (pak is CtoS_0x9D_RegionListRequest_174)
				{
					if (classId == 0 && (pak as CtoS_0x9D_RegionListRequest_174).Flag > 0)
					{
						classId = (pak as CtoS_0x9D_RegionListRequest_174).ClassId;
					}
				}
				else if (pak is StoC_0xAD_StatusUpdate_190)
				{
					if (MaxHealth == -1)
						MaxHealth = (pak as StoC_0xAD_StatusUpdate_190).MaxHealth;
				}
				else if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate)pak;
					if (stat.SubCode == 3)
					{
						if (level == -1)
						{
							StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate)stat.SubData;
							level = subData.playerLevel;
							if (className != subData.className)
							{
								for (int j = 0; j < ClassInfo.Length; j++)
								{
// TODO check for realm, while it not too nessery, becose same class name on different realms have same same HPBase
									if ((string)ClassInfo[j][1] == subData.className)
									{
										classId = j;
										break;
									}
								}
							}
							className = subData.className;
							if (MaxHealth == -1)
								MaxHealth = (((subData.maxHealthHigh & 0xFF) << 8) | (subData.maxHealthLow & 0xFF));
//							charName = subData.playerName;
							if (subData is StoC_0x16_VariousUpdate_179.PlayerUpdate_179)
							{
								champ_level = (subData as StoC_0x16_VariousUpdate_179.PlayerUpdate_179).championLevel;
							}
						}
					}
					else if (stat.SubCode == 1)
					{
						if (!flagSkillsChecked)
						{
							StoC_0x16_VariousUpdate.SkillsUpdate subData = (StoC_0x16_VariousUpdate.SkillsUpdate)stat.SubData;
							for (int j = (pak as StoC_0x16_VariousUpdate).SubCount - 1; j >=0; j--)
							{
								bool flagPrintSkill = false;
								StoC_0x16_VariousUpdate.Skill skill = subData.data[j];
								if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities && skill.name.StartsWith("Toughness "))
								{
									switch (skill.name.Substring(10))
									{
										case "I":
											RASkillHPBonus = 25;
											break;
										case "II":
											RASkillHPBonus = 75;
											break;
										case "III":
											RASkillHPBonus = 150;
											break;
										case "IV":
											RASkillHPBonus = 250;
											break;
										case "V":
											RASkillHPBonus = 400;
											break;
										default:
											RASkillHPBonus = 0; // something wrong...
											break;
									}
									flagPrintSkill = true;
								}
								else if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities && skill.name.StartsWith("Augmented Constitution "))
								{
									flagPrintSkill = true;
								}
								else if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities && skill.name.Equals("Scars of Battle"))
								{
									flagPrintSkill = true;
									flagScarsOfBattleFound = true;
								}
								if (flagPrintSkill)
									str.AppendFormat("level:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-2} icon:0x{5:X4} name:\"{6}\"\n",
										skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
							}
							if ((pak as StoC_0x16_VariousUpdate).StartIndex == 0)// not found any RA
								flagSkillsChecked = true;
						}
					}
				}
				if (charStats != null && level != -1 && classId != 0 && flagSkillsChecked)
				{
					flagFound = true;
					break;
				}
			}
			if (flagFound)
			{
				str.Append(charStats.GetPacketDataString(true));
				str.Append('\n');
				str.Append('\n');
				str.AppendFormat("class:{0}({1}) level:{2}", classId, className, level);
				if (champ_level > -1)
					str.AppendFormat(" champLevel:{0}", champ_level);
				str.Append('\n');
				str.AppendFormat("classBaseHP:{0}\n", (int)ClassInfo[classId][2]);
				str.AppendFormat("CON:{0}", charStats.con);
				int Constitution = charStats.con;
				if (classId == 58) // Vampiir
				{
					str.AppendFormat(" VampBonus CON:{0}(calced:{1})", charStats.Flag * 3, (level - 5) * 3);
					Constitution += charStats.Flag * 3;
				}
				int RealmAbilitiesBonusConstitution = charStats.R_con;
				Constitution += RealmAbilitiesBonusConstitution;
				if (RealmAbilitiesBonusConstitution != 0)
					str.AppendFormat(" RA_CON:{0}", RealmAbilitiesBonusConstitution);
				int BuffBonusConstitution = charStats.B_con;
				Constitution += BuffBonusConstitution;
				if (BuffBonusConstitution!= 0)
					str.AppendFormat(" BuffBonus:{0}", BuffBonusConstitution);
				int ItemBonusConstitution = Math.Min(charStats.I_con, charStats.C_con);
				Constitution += ItemBonusConstitution;
				if (ItemBonusConstitution != 0)
					str.AppendFormat(" ItemBonus:{0}", ItemBonusConstitution);
				if (RASkillHPBonus != 0)
					str.AppendFormat(" RealmAbilitiesHP:{0}", RASkillHPBonus);
				str.Append('\n');
				int ItemBonusHits = 0;
				int MaxHealthCalculated = CalculateMaxHealth(level, Constitution, (int)ClassInfo[classId][2], champ_level);
				if (flagScarsOfBattleFound)
				{
					MaxHealthCalculated = (int)(MaxHealthCalculated * (1.0 + (level - 40) * 0.01));
					// Need i see Scar HP add ?
				}
				MaxHealthCalculated += ItemBonusHits + RASkillHPBonus;
				str.AppendFormat("HP:{0} CalcedHP:{1}(calcedCON:{2})\n", MaxHealth, MaxHealthCalculated, Constitution);
				if (MaxHealthCalculated != MaxHealth)
					str.AppendFormat("unknown Hits:{0} (possible ItemBonus:+Hits, +MaxHits, wrong check position (not finished update))\n", MaxHealth - MaxHealthCalculated);
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player calc info (right click to close)";
			infoWindow.Width = 820;
			infoWindow.Height = 320;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		public int CalculateMaxHealth(int level, int constitution, int BaseHP, int ChampionLevel)
		{
			constitution -= 50;
			if (constitution < 0)
				constitution *= 2;
			double hp1 = (BaseHP * level * (1.0 + (ChampionLevel > 0 ? (ChampionLevel - 1) * 0.02 : 0)));
			double hp2 = (hp1 * constitution * 0.0001);
			return (int)Math.Max(1, 20 + hp1 * 0.02 + hp2);
		}
		#endregion
	}
}
