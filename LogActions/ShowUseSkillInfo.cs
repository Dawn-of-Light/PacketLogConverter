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
							for (int j = variousPak.SubCount - 1; j >=0; j--)
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
								foreach (StoC_0x16_VariousUpdate.Skill skill in skillList)
								{
									index--;
									if (index == spellIndex)
									{
										str.Append("\n");
										str.AppendFormat("level:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-2} icon:0x{5:X4} name:\"{6}\"\n",
											skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
										spellIcon = skill.icon;
										spellName = skill.name;
										additionStringCount += 2;
										searchInSpellEffects = skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities || skill.page == StoC_0x16_VariousUpdate.eSkillPage.Spells || skill.page == StoC_0x16_VariousUpdate.eSkillPage.Songs;
										break;
									}
								}
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
				bool effectFound = false;
				for (int i = selectedIndex; i < log.Count ; i++)
				{
					Packet pak = log[i];
					if (pak is StoC_0x7F_UpdateIcons)
					{
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
									effectFound = true;
									break;
								}
							}
							if (effectFound)
								break;
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
	}
}
