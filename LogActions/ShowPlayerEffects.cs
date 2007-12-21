using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player effects", Priority=950)]
	public class ShowPlayerEffectsAction : ILogAction
	{
		#region ILogAction Members
		/// <summary>
		/// Activate log action
		/// </summary>
		/// <param name="log">The current log</param>
		/// <param name="selectedIndex">The selected packet index</param>
		/// <returns>True if log data tab should be updated</returns>
		public virtual bool Activate(PacketLog log, int selectedIndex)
		{
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
					str.AppendFormat("\niconIndex:{0,-2} {6} immunity:0x{1:X2} icon:0x{2:X4} remainingTime:{3,-4} internalId:{4,-5} name:\"{5}\"",
						effect.iconIndex, effect.immunity, effect.icon, (short)effect.remainingTime, effect.internalId, effect.name, effect.unk1 == 0xFF ? "SKL" : "SPL");
			}

			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate skills = (pak as StoC_0x16_VariousUpdate);
					if (skills.SubCode == 1)
					{
						str.Append("\n");
						str.Append("\n");
						str.Append(skills.GetPacketDataString(true));
						StoC_0x16_VariousUpdate.SkillsUpdate data = (skills.SubData as StoC_0x16_VariousUpdate.SkillsUpdate);
						if (data.startIndex == 0)
							break;
					}
				}
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
