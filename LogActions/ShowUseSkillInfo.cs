using System;
using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows use skill info
	/// </summary>
	[LogAction("Show use skill info", Priority=980)]
	public class ShowUseSkillInfoAction : ILogAction
	{
		#region ILogAction Members

		/// <summary>
		/// Determines whether the action is enabled.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns>
		/// 	<c>true</c> if the action is enabled; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			Packet originalPak = context.LogManager.GetPacket(selectedPacket);
			if (!(originalPak is CtoS_0xBB_UseSkill || originalPak is CtoS_0x7D_UseSpellList  || originalPak is CtoS_0xD8_DetailDisplayRequest)) // activate condition
				return false;
			return true;
		}

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			int selectedIndex = selectedPacket.PacketIndex;

			Packet originalPak = log[selectedIndex];
			if (!(originalPak is CtoS_0xBB_UseSkill || originalPak is CtoS_0x7D_UseSpellList  || originalPak is CtoS_0xD8_DetailDisplayRequest)) // activate condition
				return false;
			int spellIndex = -1;
			int spellLineIndex = -1;
			if (originalPak is CtoS_0xBB_UseSkill)
				spellIndex = (originalPak as CtoS_0xBB_UseSkill).Index;
			else if (originalPak is CtoS_0x7D_UseSpellList)
			{
				spellLineIndex = (originalPak as CtoS_0x7D_UseSpellList).SpellLineIndex;
				spellIndex = (originalPak as CtoS_0x7D_UseSpellList).SpellLevel;
			}
			else if (originalPak is CtoS_0xD8_DetailDisplayRequest)
			{
				switch((originalPak as CtoS_0xD8_DetailDisplayRequest).ObjectType)
				{
					case 2:
						spellLineIndex = (originalPak as CtoS_0xD8_DetailDisplayRequest).ObjectId / 100;
						spellIndex = (originalPak as CtoS_0xD8_DetailDisplayRequest).ObjectId % 100;
						break;
					default:
						return false;
				}
			}
			else
				return false;
			StringBuilder str = new StringBuilder();
			IList skillList = new ArrayList();
			int additionStringCount = 0;
			ushort spellIcon = 0xFFFF;
			string spellName = "UNKNOWN";
			bool searchInSpellEffects = false;
			str.Append(originalPak.ToHumanReadableString(TimeSpan.Zero, true));
			str.Append('\n');
			for (int i = selectedIndex; i >= 0 ; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate variousPak = (pak as StoC_0x16_VariousUpdate);
					if (originalPak is CtoS_0xBB_UseSkill)
					{
						if (variousPak.SubCode == 1)
						{
							StoC_0x16_VariousUpdate.SkillsUpdate data = (variousPak.SubData as StoC_0x16_VariousUpdate.SkillsUpdate);
							for (int j = variousPak.SubCount - 1; j >= 0; j--)
							{
								StoC_0x16_VariousUpdate.Skill skill = data.data[j];
								if ((originalPak as CtoS_0xBB_UseSkill).Type == 0 && (int)skill.page == 0)
									skillList.Add(skill);
								else if ((originalPak as CtoS_0xBB_UseSkill).Type == 1 && (int)skill.page > 0)
									skillList.Add(skill);
							}
							if (variousPak.StartIndex == 0)
							{
								int index = skillList.Count;
								int lineIndex = -1;
								string skillInfo = "";
								foreach (StoC_0x16_VariousUpdate.Skill skill in skillList)
								{
									index--;
									if (index == spellIndex)
									{
										if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.Spells && skill.stlOpen != 0xFE)
											lineIndex = skill.stlOpen;
										if (log.Version >= 180)
										{
											if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.Styles)
											{
												str.AppendFormat("\nSpec:\"{0}\"", GetSpecNameFromInternalIndex(skill.bonus));
											}
										}
										skillInfo = string.Format("\nlevel:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-2} icon:0x{5:X4} name:\"{6}\"\n",
											skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
										spellIcon = skill.icon;
										spellName = skill.name;
										additionStringCount += 2;
										searchInSpellEffects = skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities || skill.page == StoC_0x16_VariousUpdate.eSkillPage.Spells || skill.page == StoC_0x16_VariousUpdate.eSkillPage.Songs;
										break;
									}
								}
								if (lineIndex >= 0)
								{
									index = 0;
									foreach (StoC_0x16_VariousUpdate.Skill skill in data.data)
									{
										if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.Specialization)
										{
											if (index++ == lineIndex)
											{
												str.AppendFormat("\nSpec:\"{0}\"", skill.name);
//												str.AppendFormat("\nlevel:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-2} icon:0x{5:X4} name:\"{6}\"\n",
//													skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
												break;
											}
										}
										else
											break;
									}
								}
								str.Append(skillInfo);
								break;
							}
						}
					}
					else if (spellLineIndex >= 0)
					{
						if (variousPak.SubCode == 2)
						{
							StoC_0x16_VariousUpdate.SpellsListUpdate data = (variousPak.SubData as StoC_0x16_VariousUpdate.SpellsListUpdate);
							if (variousPak.StartIndex == spellLineIndex)
							{
								string spellLineName = "";
								for (int j = 0; j < variousPak.SubCount; j++)
								{
									StoC_0x16_VariousUpdate.Spell spell = data.list[j];
									if (spell.level == spellIndex)
									{
										str.AppendFormat("\nspellLineIndex:{0}(\"{4}\") spellLevel:{1,-2} icon:0x{2:X4} name:\"{3}\"\n",
											spellLineIndex, spell.level, spell.icon, spell.name, spellLineName);
										spellIcon = spell.icon;
										spellName = spell.name;
										searchInSpellEffects = true;
										additionStringCount += 2;
										break;
									}
									else if (spell.level == 0)
										spellLineName = spell.name;
								}
							}
							if (variousPak.SubType == 2 && variousPak.StartIndex == 0) // not this spell found in spellList
								break;
						}
					}
				}
			}
			if (searchInSpellEffects)
			{
				bool spellEffectFound = false;
				bool concEffectFound = false;
				for (int i = selectedIndex; i < log.Count ; i++)
				{
					Packet pak = log[i];
					if (pak is StoC_0x7F_UpdateIcons)
					{
						if (spellEffectFound) continue;
						StoC_0x7F_UpdateIcons effectsPak = (pak as StoC_0x7F_UpdateIcons);
						if (effectsPak != null)
						{
							for (int j = 0; j < effectsPak.EffectsCount; j++)
							{
								if (effectsPak.Effects[j].name == spellName)
								{
									StoC_0x7F_UpdateIcons.Effect effect = effectsPak.Effects[j];
									str.Append('\n');
									str.Append(pak.ToHumanReadableString(TimeSpan.Zero, true));
									str.Append('\n');
									spellIcon = effect.icon;
									additionStringCount += (2 + effectsPak.EffectsCount);
									spellEffectFound = true;
									break;
								}
							}
							if (spellEffectFound && concEffectFound)
								break;
						}
					}
					else if (pak is StoC_0x75_SetConcentrationList)
					{
						if (concEffectFound) continue;
						if (spellIcon != 0xFFFF)
						{
							StoC_0x75_SetConcentrationList concPak = (pak as StoC_0x75_SetConcentrationList);
							if (concPak != null)
							{
								for (int j = 0; j < concPak.EffectsCount; j++)
								{
									if (concPak.Effects[j].icon == spellIcon)
									{
//										if (concPak.Effects[j].effectName.Substring(10) != spellName.Substring(10)) continue;
										StoC_0x75_SetConcentrationList.ConcentrationEffect effect = concPak.Effects[j];
										str.AppendFormat("\nCONC index:{0,-2} conc:{1,-2} icon:0x{2:X4} ownerName:\"{3}\" effectName:\"{4}\"", effect.index, effect.concentration, effect.icon, effect.ownerName, effect.effectName);
										str.Append('\n');
										additionStringCount += (2 + concPak.EffectsCount);
										concEffectFound = true;
										break;
									}
								}
								if (/*spellEffectFound && */concEffectFound) // conc packet always after effect packet, so we can break on conc packet
									break;
							}
						}
					}
				}
			}
			if (spellName != "UNKNOWN")
				additionStringCount += FormInfoString(log, selectedIndex, str, spellIcon, spellName);

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Use skill/Cast spell info (right click to close)";
			infoWindow.Width = 800;
			infoWindow.Height = 100;
			infoWindow.Height += 14 * additionStringCount;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		private int FormInfoString(PacketLog log, int selectedIndex, StringBuilder str, ushort icon, string spellName)
		{
        	int additionStringCount = 0;
			for (int i = selectedIndex; i < log.Count ; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x72_SpellCastAnimation)
				{
					StoC_0x72_SpellCastAnimation animatePak = pak as StoC_0x72_SpellCastAnimation;
					if (animatePak != null && animatePak.SpellId == icon)
					{
						str.Append('\n');
						str.Append(pak.ToHumanReadableString(TimeSpan.Zero, true));
						str.Append('\n');
						additionStringCount += 3;
						break;
					}
				}
			}
			for (int i = selectedIndex; i < log.Count ; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x1B_SpellEffectAnimation)
				{
					StoC_0x1B_SpellEffectAnimation effectPak = pak as StoC_0x1B_SpellEffectAnimation;
					if (effectPak != null && effectPak.SpellId == icon)
					{
						str.Append('\n');
						str.Append(pak.ToHumanReadableString(TimeSpan.Zero, true));
						str.Append('\n');
						additionStringCount += 3;
						break;
					}
				}
			}
			for (int i = 0; i < log.Count ; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0xC4_CustomTextWindow && (pak as StoC_0xC4_CustomTextWindow).Caption == spellName)
				{
					str.Append('\n');
					str.Append(pak.ToHumanReadableString(TimeSpan.Zero, true));
					additionStringCount += ((pak as StoC_0xC4_CustomTextWindow).Lines.Length + 4);
					break;
				}
			}
			return additionStringCount;
		}
		#endregion

		public enum eSpecNameByInternalIndex: byte
		{
			Slash = 0x01,
			Thrust = 0x02,
			Parry = 0x08,
			Sword = 0x0E,
			Hammer = 0x10,
			Axe = 0x11,
			Left_Axe = 0x12,
			Stealth = 0x13,
			Spear = 0x1A,
			Mending = 0x1D,
			Augmentation = 0x1E,
			Crush = 0x21,
			Pacification = 0x22,
//			Cave_Magic = 0x25,
			Darkness = 0x26,
			Suppression = 0x27,
			Runecarving = 0x2A,
			Shields = 0x2B,
			Flexible = 0x2E,
			Staff = 0x2F,
			Summoning = 0x30,
			Stormcalling = 0x32,
			Beastcraft = 0x3E,
			Polearms = 0x40,
			Two_Handed = 0x41,
			Fire_Magic = 0x42,
			Wind_Magic = 0x43,
			Cold_Magic = 0x44,
			Earth_Magic = 0x45,
			Light = 0x46,
			Matter_Magic = 0x47,
			Body_Magic = 0x48,
			Spirit_Magic = 0x49,
			Mind_Magic = 0x4A,
			Void = 0x4B,
			Mana = 0x4C,
			Dual_Wield = 0x4D,
			CompositeBow = 0x4E,
			Battlesongs = 0x52,
			Enhancement = 0x53,
			Enchantments = 0x54,
			Rejuvenation = 0x58,
			Smite = 0x59,
			Longbow = 0x5A,
			Crossbow = 0x5B,
			Chants = 0x61,
			Instruments = 0x62,
			Blades = 0x65,
			Blunt = 0x66,
			Piercing = 0x67,
			Large_Weapons = 0x68,
			Mentalism = 0x69,
			Regrowth = 0x6A,
			Nurture = 0x6B,
			Nature = 0x6C,
			Music = 0x6D,
			Celtic_Dual = 0x6E,
			Celtic_Spear = 0x70,
			RecurveBow = 0x71,
			Valor = 0x72,
			Pathfinding = 0x74,
			Envenom = 0x75,
			Critical_Strike = 0x76,
			Deathsight = 0x78,
			Painworking = 0x79,
			Death_Servant = 0x7A,
			Soulrending = 0x7B,
			HandToHand = 0x7C,
			Scythe = 0x7D,
//			Bone_Army = 0x7E,
			Arboreal_Path = 0x7F,
			Creeping_Path = 0x81,
			Verdant_Path = 0x82,
			OdinsWill = 0x85,
			SpectralForce = 0x86, // Spectral Guard ?
			PhantasmalWail = 0x87,
			EtherealShriek = 0x88,
			ShadowMastery = 0x89,
			VampiiricEmbrace = 0x8A,
			Dementia = 0x8B,
			Witchcraft = 0x8C,
			Cursing = 0x8D,
			Hexing = 0x8E,
			Fist_Wraps = 0x93,
			Mauler_Staff = 0x94,
			SpectralGuard = 0x95,
			Archery  = 0x9B,
		}

		public static string GetSpecNameFromInternalIndex(int internalIndex)
		{
			return ((eSpecNameByInternalIndex)internalIndex).ToString();
		}
	}
}
