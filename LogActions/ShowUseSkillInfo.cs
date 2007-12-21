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
	public class ShowUseSkillInfoAction: ILogAction
	{
		TimeSpan zeroTimeSpan = new TimeSpan(0);
		#region ILogAction Members
		/// <summary>
		/// Activate log action
		/// </summary>
		/// <param name="log">The current log</param>
		/// <param name="selectedIndex">The selected packet index</param>
		/// <returns>True if log data tab should be updated</returns>
		public virtual bool Activate(PacketLog log, int selectedIndex)
		{
			Packet originalPak = log[selectedIndex];
			if (!(originalPak is CtoS_0xBB_UseSkill) && !(originalPak is CtoS_0x7D_UseSpellList)) // activate condition
				return false;
			StringBuilder str = new StringBuilder();
			IList skillList = new ArrayList();
			int spellIndex = -1;
			if (originalPak is CtoS_0xBB_UseSkill)
				spellIndex = (originalPak as CtoS_0xBB_UseSkill).Index;
			else if (originalPak is CtoS_0x7D_UseSpellList)
				spellIndex = (originalPak as CtoS_0x7D_UseSpellList).SpellLevel;
			int additionStringCount = 0;
			ushort spellIcon = 0xFFFF;
			string spellName = "UNKNOWN";
			bool searchInSpellEffects = false;
			str.Append(originalPak.ToHumanReadableString(zeroTimeSpan, true));
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
							for (int j = data.count - 1; j >=0; j--)
							{
								StoC_0x16_VariousUpdate.Skill skill = data.data[j];
								if ((originalPak as CtoS_0xBB_UseSkill).Type == 0 && (int)skill.page == 0)
									skillList.Add(skill);
								else if ((originalPak as CtoS_0xBB_UseSkill).Type == 1 && (int)skill.page > 0)
									skillList.Add(skill);
							}
							if (data.startIndex == 0)
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
										searchInSpellEffects = skill.page == StoC_0x16_VariousUpdate.eSkillPage.RealmAbilities || skill.page == StoC_0x16_VariousUpdate.eSkillPage.Spells || skill.page == StoC_0x16_VariousUpdate.eSkillPage.Songs;
										break;
									}
								}
								break;
							}
						}
					}
					else if (originalPak is CtoS_0x7D_UseSpellList)
					{
						if (variousPak.SubCode == 2)
						{
							StoC_0x16_VariousUpdate.SpellsListUpdate data = (variousPak.SubData as StoC_0x16_VariousUpdate.SpellsListUpdate);
							if (data.lineIndex == (originalPak as CtoS_0x7D_UseSpellList).SpellLineIndex)
							{
								for (int j = data.count - 1; j >=0; j--)
								{
									StoC_0x16_VariousUpdate.Spell spell = data.list[j];
									if (spell.level == spellIndex)
									{
										str.AppendFormat("\nspell level:{0,-2} icon:0x{1:X4} name:\"{2}\"\n",
											spell.level, spell.icon, spell.name);
										spellIcon = spell.icon;
										spellName = spell.name;
										searchInSpellEffects = true;
										additionStringCount += 3;
										break;
									}
								}
							}
							if (data.subtype == 2 && data.lineIndex == 0) // not this spell found in spellList
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
									str.Append(pak.ToHumanReadableString(zeroTimeSpan, true));
									str.Append('\n');
									spellIcon = effect.icon;
									additionStringCount += (3 + effectsPak.EffectsCount);
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
						str.Append(pak.ToHumanReadableString(zeroTimeSpan, true));
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
					str.Append(pak.ToHumanReadableString(zeroTimeSpan, true));
					additionStringCount += ((pak as StoC_0xC4_CustomTextWindow).Lines.Length + 4);
					break;
				}
			}
			return additionStringCount;
        }
		#endregion
	}
}
