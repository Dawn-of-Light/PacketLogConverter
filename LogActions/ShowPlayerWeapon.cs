using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player weapon", Priority=950)]
	public class ShowPlayerWeaponAction : ILogAction
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
			int VisibleSlots = 0xFF;
			int weaponSkill = -1;
			double DPS = -1;
			StoC_0x02_InventoryUpdate.Item[] weapons = new StoC_0x02_InventoryUpdate.Item[4];
			StoC_0xFB_CharStatsUpdate_175 charStats = null;

			for (int i = 0; i < selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate)pak;
					if (stat.SubCode == 5)
					{
						StoC_0x16_VariousUpdate.PlayerStateUpdate subData = (StoC_0x16_VariousUpdate.PlayerStateUpdate)stat.SubData;
						DPS = subData.weaponDamageHigh + 0.01 * subData.weaponDamageLow;
						weaponSkill = (subData.weaponSkillHigh << 8) + subData.weaponSkillLow;
					}
				}
				else if (pak is StoC_0xFB_CharStatsUpdate_175)
				{
					if ((pak as StoC_0xFB_CharStatsUpdate_175).Flag != 0xFF)
						charStats = pak as StoC_0xFB_CharStatsUpdate_175;
				}
				else if (pak is StoC_0x02_InventoryUpdate)
				{
					StoC_0x02_InventoryUpdate invPack = (StoC_0x02_InventoryUpdate)pak;
					if (invPack.PreAction == 1 || invPack.PreAction == 0)
					{
						VisibleSlots = invPack.VisibleSlots;
						for (int j = 0; j < invPack.SlotsCount; j++)
						{
							StoC_0x02_InventoryUpdate.Item item = (StoC_0x02_InventoryUpdate.Item)invPack.Items[j];
							switch (item.slot)
							{
								case 10:
									weapons[0] = item;
//									Logger.Say(string.Format("{16:X2}\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"",
//									item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name, invPack.VisibleSlots));
									break;
								case 11:
									weapons[1] = item;
									break;
								case 12:
									weapons[2] = item;
									break;
								case 13:
									weapons[3] = item;
									break;
								default:
									break;
							}
						}
					}
				}
			}

			StringBuilder str = new StringBuilder();
			str.AppendFormat("Weapon damage = {0:0.00}\n", DPS);
			str.AppendFormat("Weapon skill  = {0}\n", weaponSkill);
			if (charStats != null)
			{
				str.Append(charStats.GetPacketDataString(true));
				str.AppendFormat("\n");
			}
			str.AppendFormat("\n");
			str.AppendFormat("Visible slots = 0x{0:X2}", VisibleSlots);
			str.AppendFormat("\n");
			//((VisibleSlots & 0x0F) == 0)
			for (int i = 0; i < 4; i++)
			{
				if ((i == (VisibleSlots & 0x0F)) || (i == ((VisibleSlots >> 4) & 0x0F)))
				{
					StoC_0x02_InventoryUpdate.Item item = (StoC_0x02_InventoryUpdate.Item)weapons[i];
//					str.AppendFormat("slot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"\n",
//					item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name);
					if (item.objectType == 42)//shield
						str.AppendFormat("slot:{0,-2} level:{1,-2} size:{2} con:{3,-3} qual:{4,-3} bonus:{5,-2} model:0x{6:X4} \"{7}\"",
						item.slot, item.level, item.value1, item.condition, item.quality, item.bonus, item.model, item.name);
					else
						str.AppendFormat("slot:{0,-2} level:{1,-2} dps:{2:0.00} spd:{3:0.00} damageType:{4} con:{5,-3} qual:{6,-3} bonus:{7,-2} model:0x{8:X4} \"{9}\"",
						item.slot, item.level, 0.1 * item.value1, 0.1 * item.value2, item.damageType, item.condition, item.quality, item.bonus, item.model, item.name);
					str.AppendFormat(" ({0})\n", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
				}
			}

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player weapon info (right click to close)";
			infoWindow.Width = 800;
			if (charStats == null)
				infoWindow.Height = 200;
			else
				infoWindow.Height = 300;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion
	}
}
