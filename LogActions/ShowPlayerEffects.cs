using System.Text;
using System.Collections;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player effects", Priority=950)]
	public class ShowPlayerEffectsAction : AbstractEnabledAction
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
			PacketLog log = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			int selectedIndex = selectedPacket.PacketIndex;

			StoC_0x7F_UpdateIcons.Effect[] effects = new StoC_0x7F_UpdateIcons.Effect[40];
			for (int i = 0; i < selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x7F_UpdateIcons)
				{
					StoC_0x7F_UpdateIcons.Effect[] listeffects = (pak as StoC_0x7F_UpdateIcons).Effects;
					for (int j = 0; j < listeffects.Length; j++)
						effects[listeffects[j].iconIndex] = listeffects[j];
				}
			}

			StringBuilder str = new StringBuilder();
			for (int i = 0; i < effects.Length; i++)
			{
				StoC_0x7F_UpdateIcons.Effect effect = effects[i];
				if (effect.name != null && effect.name != "")
					str.AppendFormat("iconIndex:{0,-2} {6} immunity:0x{1:X2} icon:0x{2:X4} remainingTime:{3,-4} internalId:{4,-5} name:\"{5}\"\n",
						effect.iconIndex, effect.immunity, effect.icon, (short)effect.remainingTime, effect.internalId, effect.name, effect.unk1 == 0xFF ? "SKL" : "SPL");
			}
			StoC_0x16_VariousUpdate.Skill[] Skills = null;
			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate skills = (pak as StoC_0x16_VariousUpdate);
					if (skills.SubCode == 1)
					{
						if (Skills == null)
							Skills = new StoC_0x16_VariousUpdate.Skill[skills.StartIndex + skills.SubCount];
						int j = skills.StartIndex;
						foreach (StoC_0x16_VariousUpdate.Skill skill in skills.InSkillsUpdate.data)
						{
							Skills[j++] = skill;
						}
						if (skills.StartIndex == 0)
							break;
					}
				}
			}
			int index = -1;
			if (effects.Length > 0)
				str.Append("\n");
			if (Skills.Length > 0)
				str.Append("Skills:");
			foreach(StoC_0x16_VariousUpdate.Skill skill in Skills)
			{
				str.Append("\n");
				if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.Styles && skill.stlOpen >= 0x6400) // 0x6400 = 100 << 8
				{
					str.AppendFormat("*({0,-2})", (skill.stlOpen >> 8) - 100);
				}
				str.Append("\t");
			 	if((int)skill.page > 0)
					index++;
				str.AppendFormat("[{0,-2}] ", index);
				str.AppendFormat("level:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-3} icon:0x{5:X4} name:\"{6}\"",
						skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player effects info (right click to close)";
			infoWindow.Width = 820;
			infoWindow.Height = 320;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion
	}
}
