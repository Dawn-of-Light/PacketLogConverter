using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player inventory")]
	public class ShowPlayerInventoryAction : AbstractEnabledAction
	{
		#region ILogAction Members

		private enum eWindowType: byte
		{
	        Unknown = 0,
	        Horse = 1,
    	    Weapon = 2,
    	    Quiver = 3,
    	    Doll = 4,
    	    Backpack = 5,
		}
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
			SortedList m_inventoryItems = new SortedList();
			int VisibleSlots = 0xFF;
			for (int i = 0; i <= selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x02_InventoryUpdate)
				{
					StoC_0x02_InventoryUpdate invPack = (StoC_0x02_InventoryUpdate)pak;
					if (invPack.PreAction == 1 || invPack.PreAction == 0 || invPack.PreAction == 11 || invPack.PreAction == 10)
					{
						VisibleSlots = invPack.VisibleSlots;
					}
						for (int j = 0; j < invPack.SlotsCount; j++)
						{
							StoC_0x02_InventoryUpdate.Item item = (StoC_0x02_InventoryUpdate.Item)invPack.Items[j];
							if (item.name == null || item.name == "")
							{
								if (m_inventoryItems.ContainsKey(item.slot))
									m_inventoryItems.Remove(item.slot);
							}
							else
							{
								if (m_inventoryItems.Contains(item.slot))
									m_inventoryItems[item.slot] = item;
								else
									m_inventoryItems.Add(item.slot, item);
							}
						}
				}
			}

			StringBuilder str = new StringBuilder();
			str.AppendFormat("VisibleSlots:0x{0:X2}\n", VisibleSlots);
			eWindowType prevWindowType = eWindowType.Unknown;
			eWindowType windowType = eWindowType.Unknown;
			foreach (StoC_0x02_InventoryUpdate.Item item in m_inventoryItems.Values)
			{
				if (item.slot < 80)
				{
					string selected = " ";
					if (item.slot >= 7 && item.slot <= 9)
						windowType = eWindowType.Horse;
					else if (item.slot >= 10 && item.slot <= 13)
					{
						windowType = eWindowType.Weapon;
						if (((item.slot - 10) == (VisibleSlots & 0x0F)) || ((item.slot - 10) == ((VisibleSlots >> 4) & 0x0F)))
							selected = "*";
					}
					else if (item.slot >= 14 && item.slot <= 17)
						windowType = eWindowType.Quiver;
					else if (item.slot >= 21 && item.slot <= 37)
						windowType = eWindowType.Doll;
					else if (item.slot >= 40 && item.slot <= 79)
						windowType = eWindowType.Backpack;
					if (windowType != prevWindowType)
						str.AppendFormat("\n{0}", windowType);
					str.AppendFormat("\n{16}slot:{0,-3} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"",
						item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name, selected);
					if (item.name != null && item.name != "")
						str.AppendFormat(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
					prevWindowType = windowType;
				}
			}

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player inventory info (right click to close)";
			infoWindow.Width = 800;
			infoWindow.Height = 400;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion
	}
}
