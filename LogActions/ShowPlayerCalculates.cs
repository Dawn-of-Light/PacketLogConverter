using System;
using System.Text;
using System.Collections;
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

		public enum eManaStat : byte
		{
			None = 0,
			Intelligence = 1,
			Piety = 2,
			Empathy = 3,
			Charisma = 4,
		}

		public static readonly object[][] ClassInfo =
		{
			// [realm, className, BaseHP]
			new object[5] {0, "Unknown", 0, eManaStat.None, false},
			new object[5] {1, "Paladin", 760, eManaStat.Piety, false},
			new object[5] {1, "Armsman", 880, eManaStat.None, false},
			new object[5] {1, "Scout", 720, eManaStat.Intelligence, false}, // ???
			new object[5] {1, "Minstrel", 720, eManaStat.Charisma, false},
			new object[5] {1, "Theurgist", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Cleric", 720, eManaStat.Piety, false},
			new object[5] {1, "Wizard", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Sorcerer", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Infiltrator", 720, eManaStat.None, false},
			new object[5] {1, "Friar", 720, eManaStat.Piety, false},
			new object[5] {1, "Mercenary", 880, eManaStat.None, false},
			new object[5] {1, "Necromancer", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Cabalist", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Fighter", 880, eManaStat.None, false},
			new object[5] {1, "Elementalist", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Acolyte", 720, eManaStat.Piety, false},
			new object[5] {1, "Rogue", 720, eManaStat.None, false}, //AlbionRogue
			new object[5] {1, "Mage", 560, eManaStat.Intelligence, true},
			new object[5] {1, "Reaver", 760, eManaStat.Piety, false},
			new object[5] {1, "Disciple", 560, eManaStat.Intelligence, true},
			new object[5] {2, "Thane", 720, eManaStat.Piety, false},
			new object[5] {2, "Warrior", 880, eManaStat.None, false},
			new object[5] {2, "Shadowblade", 760, eManaStat.None, false},
			new object[5] {2, "Skald", 760, eManaStat.Charisma, false},
			new object[5] {2, "Hunter", 720, eManaStat.Piety, false},
			new object[5] {2, "Healer", 720, eManaStat.Piety, false},
			new object[5] {2, "Spiritmaster", 560, eManaStat.Piety, true},
			new object[5] {2, "Shaman", 720, eManaStat.Piety, true},
			new object[5] {2, "Runemaster", 560, eManaStat.Piety, true},
			new object[5] {2, "Bonedancer", 560, eManaStat.Piety, true},
			new object[5] {2, "Berserker", 880, eManaStat.None, false},
			new object[5] {2, "Savage", 880, eManaStat.None, false},
			new object[5] {1, "Heretic", 720, eManaStat.Piety, true},// test
			new object[5] {2, "Valkyrie", 760, eManaStat.Piety, false},// DOL value 720 is wrong !!!
			new object[5] {2, "Viking", 880, eManaStat.None, false},
			new object[5] {2, "Mystic", 560, eManaStat.Intelligence, false}, // ?
			new object[5] {2, "Seer", 720, eManaStat.Piety, false}, // ?
			new object[5] {2, "Rogue", 720, eManaStat.None, false}, //MidgardRogue
			new object[5] {3, "Bainshee", 560, eManaStat.Intelligence, true},
			new object[5] {3, "Eldritch", 560, eManaStat.Intelligence, true},
			new object[5] {3, "Enchanter", 560, eManaStat.Intelligence, true},
			new object[5] {3, "Mentalist", 560, eManaStat.Intelligence, true},
			new object[5] {3, "Blademaster", 880, eManaStat.None, false},
			new object[5] {3, "Hero", 880, eManaStat.None, false},
			new object[5] {3, "Champion", 760, eManaStat.Intelligence, false},
			new object[5] {3, "Warden", 720, eManaStat.Empathy, false},
			new object[5] {3, "Druid", 720, eManaStat.Empathy, true},
			new object[5] {3, "Bard", 720, eManaStat.Charisma, false},
			new object[5] {3, "Nightshade", 720, eManaStat.Intelligence, false},
			new object[5] {3, "Ranger", 720, eManaStat.Intelligence, false},
			new object[5] {3, "Magician", 560, eManaStat.Intelligence, true},
			new object[5] {3, "Guardian", 880, eManaStat.None, false},
			new object[5] {3, "Naturalist", 720, eManaStat.Empathy, true},
			new object[5] {3, "Stalker", 720, eManaStat.None, false},
			new object[5] {3, "Animist", 560, eManaStat.Intelligence, true},
			new object[5] {3, "Valewalker", 720, eManaStat.Intelligence, false},
			new object[5] {3, "Forester", 560, eManaStat.Intelligence, false},
			new object[5] {3, "Vampiir", 880, eManaStat.None, false},
			new object[5] {2, "Warlock", 560, eManaStat.Piety, true},
			new object[5] {1, "Mauler", 720, eManaStat.None, false},// DOL value 600 is wrong !!!
			new object[5] {2, "Mauler", 720, eManaStat.None, false},// DOL value 600 is wrong !!!
			new object[5] {3, "Mauler", 720, eManaStat.None, false},// DOL value 600 is wrong !!!
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
			int MaxPower = -1;
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
			int RASkillMPBonus = 0;
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
					if (MaxPower == -1)
						MaxPower = (pak as StoC_0xAD_StatusUpdate_190).MaxPower;
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
								else if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities && skill.name.StartsWith("Ethereal Bond "))
								{
									switch (skill.name.Substring(14))
									{
										case "I":
											RASkillMPBonus = 20; // +20 for scout (20)
											break;
										case "II":
											RASkillMPBonus = 50; // +30 for scout (50)
											break;
										case "III":
											RASkillMPBonus = 80; // +30 for scout (80)
											break;
										case "IV":
											RASkillMPBonus = 130; // +50 for scout (130)
											break;
										case "V":
											RASkillMPBonus = 200; // +70 for scout (200)
											break;
										default:
											RASkillMPBonus = 0; // something wrong...
											break;
									}
									flagPrintSkill = true;
								}
								else if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities && skill.name.StartsWith("Augmented Constitution "))
								{
									flagPrintSkill = true;
								}
								else if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities && skill.name.StartsWith("Augmented Acuity "))
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
				bool flagPureCaster = (bool)ClassInfo[classId][4];
				str.Append(charStats.GetPacketDataString(true));
				str.Append('\n');
				str.Append('\n');
				str.AppendFormat("class:{0}({1}) level:{2} PureCaster:{3}", classId, className, level, flagPureCaster);
				if (champ_level > -1)
					str.AppendFormat(" champLevel:{0}", champ_level);
				str.Append('\n');
				str.AppendFormat("ManaStat:{0}", (eManaStat)ClassInfo[classId][3]);
				int ManaStat = 0;
				int ManaStatBuffBonus = 0;
				int ManaStatItemBonus = 0;
				int ManaStatRealmAbilitiesBonus = 0;
				switch ((eManaStat)ClassInfo[classId][3])
				{
					case eManaStat.Intelligence:
						ManaStat = charStats.@int;
						ManaStatBuffBonus = charStats.B_int;
						ManaStatItemBonus = Math.Min(charStats.I_int, charStats.C_int);
						ManaStatRealmAbilitiesBonus = charStats.R_int;
						break;
					case eManaStat.Piety:
						ManaStat = charStats.pie;
						ManaStatBuffBonus = charStats.B_pie;
						ManaStatItemBonus = Math.Min(charStats.I_pie, charStats.C_pie);
						ManaStatRealmAbilitiesBonus = charStats.R_pie;
						break;
					case eManaStat.Empathy:
						ManaStat = charStats.emp;
						ManaStatBuffBonus = charStats.B_emp;
						ManaStatItemBonus = Math.Min(charStats.I_emp, charStats.C_emp);
						ManaStatRealmAbilitiesBonus = charStats.R_emp;
						break;
					case eManaStat.Charisma:
						ManaStat = charStats.chr;
						ManaStatBuffBonus = charStats.B_chr;
						ManaStatItemBonus = Math.Min(charStats.I_chr, charStats.C_chr);
						ManaStatRealmAbilitiesBonus = charStats.R_chr;
						break;
					default:
						break;
				}
				if (ManaStat != 0)
					str.AppendFormat(":{0}", ManaStat);
//				ManaStat += (int)(ManaStatRealmAbilitiesBonus * 1.2);
				if (ManaStatRealmAbilitiesBonus != 0)
					str.AppendFormat(" RA_ManaStat:{0}", ManaStatRealmAbilitiesBonus);
//				ManaStat += ManaStatBuffBonus;
				if (ManaStatBuffBonus != 0)
					str.AppendFormat(" BuffBonus:{0}", ManaStatBuffBonus);
				ManaStat += (int)(ManaStatItemBonus * 1.2);
				if (ManaStatItemBonus != 0)
					str.AppendFormat(" ItemBonus:{0}", ManaStatItemBonus);
				if (RASkillMPBonus != 0)
					str.AppendFormat(" RealmAbilitiesMana:{0}", RASkillMPBonus);
				str.Append('\n');
				int Constitution = charStats.con;
				str.AppendFormat("classBaseHP:{0} CON:{1}", (int)ClassInfo[classId][2], charStats.con);
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
				int ItemBonusHits = 0;
				int MaxHealthCalculated = CalculateMaxHealth(level, Constitution, (int)ClassInfo[classId][2], champ_level);
				if (flagScarsOfBattleFound)
				{
					int HeavyTankBonusHP = (int)(MaxHealthCalculated * (1.0 + (level - 40) * 0.01));
					str.AppendFormat(" HeavyTankBonusHP:{0}", HeavyTankBonusHP - MaxHealthCalculated);
					MaxHealthCalculated = HeavyTankBonusHP;
				}
				str.Append('\n');
				MaxHealthCalculated += ItemBonusHits + RASkillHPBonus;
				int MaxManaCalculated = CalculateMaxMana(level, ManaStat, champ_level, flagPureCaster, str);
				str.AppendFormat("HP:{0} CalcedHP:{1}(calcedCON:{2})\n", MaxHealth, MaxHealthCalculated, Constitution);
				str.AppendFormat("MaxMana:{0} CalcedMaxMana:{1}(ManaStat:{2})\n", MaxPower, MaxManaCalculated, ManaStat);
				int insertPos = str.Length;
				int ItemHitsBonus = 0;
				int ItemHitsBonusCap = 0;
				int ItemPowerBonus = 0;
				int ItemPowerPoolBonus = 0;
				int ItemPowerPoolCapBonus = 0;
				CheckItemsHitsBonus(log, selectedIndex, str, ref ItemHitsBonus, ref ItemHitsBonusCap, ref ItemPowerBonus, ref ItemPowerPoolBonus, ref ItemPowerPoolCapBonus);
				str.Insert(insertPos, string.Format("\nCalcedMP:{0} ItemBonusPower:{1} ItemBonusPowerPool:{2} (+cap:{3})\n", (int)((MaxManaCalculated + ItemPowerBonus) * (1 + 0.01 * ItemPowerPoolBonus)) + RASkillMPBonus + ManaStatRealmAbilitiesBonus * 1.2, ItemPowerBonus, ItemPowerPoolBonus, ItemPowerPoolCapBonus));
				if (MaxHealthCalculated != MaxHealth)
				{
					int CapItemBonusHits = ItemHitsBonusCap + level * 4;
					if (MaxHealth - MaxHealthCalculated - Math.Min(ItemHitsBonus, CapItemBonusHits) == 0)
						str.Insert(insertPos, string.Format("\nCalcedHP:{2} ItemBonusHits:{0} (+cap:{1})", Math.Min(ItemHitsBonus, CapItemBonusHits), ItemHitsBonusCap, MaxHealthCalculated + Math.Min(ItemHitsBonus, CapItemBonusHits)));
					else
						str.Insert(insertPos, string.Format("\nCalcedHP:{3} unknown Hits:{0}, ItemBonusHits:{1} (+cap:{2})", MaxHealth - MaxHealthCalculated - Math.Min(ItemHitsBonus, CapItemBonusHits), Math.Min(ItemHitsBonus, CapItemBonusHits), ItemHitsBonusCap, MaxHealthCalculated + Math.Min(ItemHitsBonus, CapItemBonusHits)));
				}
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player calc info (right click to close)";
			infoWindow.Width = 820;
			infoWindow.Height = 320;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		private int CalculateMaxHealth(int level, int constitution, int BaseHP, int ChampionLevel)
		{
			constitution -= 50;
			if (constitution < 0)
				constitution *= 2;
			double hp1 = (BaseHP * level * (1.0 + (ChampionLevel > 0 ? (ChampionLevel - 1) * 0.02 : 0)));
			double hp2 = (hp1 * constitution * 0.0001);
			return (int)Math.Max(1, 20 + hp1 * 0.02 + hp2);
		}

		private int CalculateMaxMana(int level, int manaStat, int ChampionLevel, bool flagPureCaster, StringBuilder str)
		{
			if (manaStat == 0)
			{
				if (ChampionLevel <= 1)
					return 0;
				return 100 + ChampionLevel - 1;
			}
			int manaStatPower = 0;
			if (flagPureCaster)
			{
				manaStatPower = level * (manaStat - 30) / 50;
				str.AppendFormat("{0}+", manaStatPower);
			}
			double MaxMana = manaStatPower + level * 5;
			str.Append(level * 5);
			double secondBonus = (level - 20) / 5;
			MaxMana -= secondBonus;
			str.AppendFormat("{0}{1}={2}\n", secondBonus < 0 ? "+" : "", -secondBonus, MaxMana);
			return (int)((MaxMana) * (1.0 + (ChampionLevel > 0 ? (ChampionLevel - 1) * 0.02 : 0)));
		}

		private void CheckItemsHitsBonus(PacketLog log, int selectedIndex, StringBuilder str, ref int bonusHits, ref int bonusHitCap, ref int bonusPower, ref int bonusPowerPool, ref int bonusPowerPoolCap)
		{
			SortedList m_inventoryItems = new SortedList();
			int VisibleSlots = 0xFF;
			for (int i = 0; i <= selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x20_PlayerPositionAndObjectID_171)
				{
					VisibleSlots = 0xFF;
					m_inventoryItems.Clear();
				}
				else if (pak is StoC_0x02_InventoryUpdate)
				{
					StoC_0x02_InventoryUpdate invPack = (StoC_0x02_InventoryUpdate)pak;
					if ((invPack.PreAction >= 0 && invPack.PreAction <= 1) || (invPack.PreAction >= 10 && invPack.PreAction <= 11))
					{
						VisibleSlots = invPack.VisibleSlots;
					}
					if ((invPack.PreAction >= 0 && invPack.PreAction <= 2) || (invPack.PreAction >= 10 && invPack.PreAction <= 12))
					{
						for (int j = 0; j < invPack.SlotsCount; j++)
						{
							StoC_0x02_InventoryUpdate.Item item = (StoC_0x02_InventoryUpdate.Item)invPack.Items[j];
							if (item.slot >= 10 && item.slot < 40) // Only equiped slot
							{
								if (item.name == null || item.name == "")
								{
									if (m_inventoryItems.ContainsKey(item.slot))
										m_inventoryItems.Remove(item.slot);
								}
								else
								{
									m_inventoryItems[item.slot] = new BonusItem(item);
								}
							}
						}
					}
				}
			}

			for (int i = 0; i < log.Count; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0xC4_CustomTextWindow)
				{
					foreach (BonusItem item in m_inventoryItems.Values)
					{
						if (item.slot < 40)
						{
							if (item.slot >= 10 && item.slot <= 13)
							{
								if (!(((item.slot - 10) == (VisibleSlots & 0x0F)) || ((item.slot - 10) == ((VisibleSlots >> 4) & 0x0F))))
									continue;
							}
							if (item.delvePack == null && item.name == (pak as StoC_0xC4_CustomTextWindow).Caption)
							{
								item.delvePack = pak as StoC_0xC4_CustomTextWindow;
							}
						}
					}
				}
			}
			foreach (BonusItem item in m_inventoryItems.Values)
			{
				if (item.slot < 40)
				{
					if (item.slot >= 10 && item.slot <= 13)
					{
						if (!(((item.slot - 10) == (VisibleSlots & 0x0F)) || ((item.slot - 10) == ((VisibleSlots >> 4) & 0x0F))))
							continue;
					}
					str.AppendFormat("\nslot:{0,-3} objectType:0x{1:X2} \"{2}\" ({3})",
						item.slot, item.objectType, item.name, (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
					if (item.delvePack == null)
						str.Append(" - delve not found");
					else
					{
						int flagDelveShowed = 0;
						bool flagArtifact = false;
						for (int i = 0; i < item.delvePack.Lines.Length; i++)
						{
							StoC_0xC4_CustomTextWindow.LineEntry line = item.delvePack.Lines[i];
							string text = line.text;
							if (flagArtifact)
							{
								int artLevelDescBegin = line.text.IndexOf("[L");
								if (artLevelDescBegin >= 0)
								{
									text = line.text.Substring(0, artLevelDescBegin) + line.text.Substring(line.text.IndexOf("]: ") + 3);
								}
							}
							if (line.text.StartsWith("Artifact (Current level:"))
							{
								flagArtifact = true;
							}
							else if (text.StartsWith("- Hits: "))
							{
								if (flagDelveShowed++ > 0)
									str.Append(',');
								str.Append(' ');
								str.Append(line.text);
								bonusHits += int.Parse(text.Substring(8, text.IndexOf(" pts") - 8));
							}
							else if (text.StartsWith("Bonus to hit points bonus cap:"))
							{
								if (flagDelveShowed++ > 0)
									str.Append(',');
								str.Append(' ');
								str.Append(line.text);
								bonusHitCap += int.Parse(text.Substring(31));
							}
							else if (text.StartsWith("- Power: "))
							{
								if (flagDelveShowed++ > 0)
									str.Append(',');
								str.Append(' ');
								str.Append(line.text);
								if (text.IndexOf(" % of power pool") >= 0)
									bonusPowerPool += int.Parse(text.Substring(9, text.IndexOf(" % of power pool") - 9));
								else if (text.IndexOf(" pts") >= 0)
									bonusPower += int.Parse(text.Substring(9, text.IndexOf(" pts") - 9));
							}
						}
					}
				}
			}
		}

		private class BonusItem: StoC_0x02_InventoryUpdate.Item
		{
			public StoC_0xC4_CustomTextWindow delvePack = null;
			public BonusItem() {}
			public BonusItem(StoC_0x02_InventoryUpdate.Item item)
				: this()
			{
				this.slot = item.slot;
				this.name = item.name;
				this.objectType = item.objectType;
			}
			public BonusItem(BonusItem item)
				: this()
			{
				this.name = item.name;
				this.slot = item.slot;
				this.objectType = item.objectType;
			}
		}
		#endregion
	}
}
