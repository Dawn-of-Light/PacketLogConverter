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
	[LogAction("Show slot item info", Priority=980)]
	public class ShowMoveItemInfoAction: ILogAction
	{
		#region ILogAction Members

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.Logs[selectedPacket.LogIndex];
			int selectedIndex = selectedPacket.PacketIndex;

			Packet originalPak = log[selectedIndex];
			if (!(originalPak is CtoS_0xDD_PlayerMoveItemRequest || originalPak is CtoS_0x79_SellItem || originalPak is CtoS_0xE0_AppraiseItem || originalPak is CtoS_0x71_UseItem || originalPak is CtoS_0x80_DestroyInventoryItem || originalPak is CtoS_0x41_UseNewQuestItem || originalPak is CtoS_0x0C_HouseItemPlacementRequest || originalPak is CtoS_0xEB_ModifyTradeWindow || originalPak is StoC_0xEA_TradeWindow || originalPak is CtoS_0xD8_DetailDisplayRequest)) // activate condition
				return false;
			IList slots = new ArrayList();
			if (originalPak is CtoS_0xDD_PlayerMoveItemRequest)
			{
				if ((originalPak as CtoS_0xDD_PlayerMoveItemRequest).FromSlot < 1000)
					slots.Add((ushort)(originalPak as CtoS_0xDD_PlayerMoveItemRequest).FromSlot);
				if ((originalPak as CtoS_0xDD_PlayerMoveItemRequest).ToSlot <= 250 && (originalPak as CtoS_0xDD_PlayerMoveItemRequest).ToSlot > 1)
					slots.Add((ushort)(originalPak as CtoS_0xDD_PlayerMoveItemRequest).ToSlot);
			}
			else if (originalPak is CtoS_0x0C_HouseItemPlacementRequest)
			{
				slots.Add((ushort)(originalPak as CtoS_0x0C_HouseItemPlacementRequest).Slot);
			}
			else if (originalPak is CtoS_0x41_UseNewQuestItem)
			{
				slots.Add((ushort)(originalPak as CtoS_0x41_UseNewQuestItem).GoalIndex);
			}
			else if (originalPak is CtoS_0x71_UseItem)
			{
				slots.Add((ushort)(originalPak as CtoS_0x71_UseItem).Slot);
			}
			else if (originalPak is CtoS_0x79_SellItem)
			{
				slots.Add((ushort)(originalPak as CtoS_0x79_SellItem).Slot);
			}
			else if (originalPak is CtoS_0x80_DestroyInventoryItem)
			{
				slots.Add((ushort)(originalPak as CtoS_0x80_DestroyInventoryItem).Slot);
			}
			else if (originalPak is CtoS_0xE0_AppraiseItem)
			{
				slots.Add((ushort)(originalPak as CtoS_0xE0_AppraiseItem).Slot);
			}
			else if (originalPak is StoC_0xEA_TradeWindow)
			{
				foreach(byte slot in ((byte[])(originalPak as StoC_0xEA_TradeWindow).Slots))
				{
					if (slot != 0)
						slots.Add((ushort)slot);
				}
			}
			else if (originalPak is CtoS_0xEB_ModifyTradeWindow)
			{
				foreach(byte slot in ((byte[])(originalPak as CtoS_0xEB_ModifyTradeWindow).Slots))
				{
					if (slot != 0)
						slots.Add((ushort)slot);
				}
			}
			else if (originalPak is CtoS_0xD8_DetailDisplayRequest)
			{
				if ((originalPak as CtoS_0xD8_DetailDisplayRequest).ObjectType == 1)
				{
					slots.Add((ushort)(originalPak as CtoS_0xD8_DetailDisplayRequest).ObjectId);
				}
			}
			if (slots.Count == 0)
				return false;
			TimeSpan zeroTimeSpan = new TimeSpan(0);
			StringBuilder str = new StringBuilder();
//			int additionStringCount = 0;
			str.Append(originalPak.ToHumanReadableString(zeroTimeSpan, true));
			str.Append('\n');
			for (int i = selectedIndex; i >= 0 ; i--)
			{
				Packet pak = log[i];
				if ((originalPak is CtoS_0xDD_PlayerMoveItemRequest || originalPak is CtoS_0x79_SellItem || originalPak is CtoS_0xE0_AppraiseItem || originalPak is CtoS_0x71_UseItem || originalPak is CtoS_0x80_DestroyInventoryItem || originalPak is CtoS_0x0C_HouseItemPlacementRequest || originalPak is CtoS_0xEB_ModifyTradeWindow || originalPak is StoC_0xEA_TradeWindow || originalPak is CtoS_0xD8_DetailDisplayRequest))
				{
					if (pak is StoC_0x02_InventoryUpdate)
					{
						StoC_0x02_InventoryUpdate itemsPak = (pak as StoC_0x02_InventoryUpdate);
						for (int j = 0; j < itemsPak.SlotsCount; j++)
						{
							StoC_0x02_InventoryUpdate.Item item = itemsPak.Items[j];
							int slotIndex = 0;
							do
							{
								if (slotIndex < slots.Count)
								{
									ushort slot = (ushort)slots[slotIndex];
									if (item.slot == slot)
									{
										str.AppendFormat("\nslot:{0,-3} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"",
											item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name);
										if (item.name != null && item.name != "")
											str.AppendFormat(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
										str.Append("\n");
										slots.RemoveAt(slotIndex);
										break;
									}
								}
								slotIndex++;
							}
							while (slotIndex < slots.Count);
							if (slots.Count == 0)
								break;
						}
						if (slots.Count == 0)
							break;
					}
				}
				else if (originalPak is CtoS_0x41_UseNewQuestItem)
				{
					if (pak is StoC_0x83_QuestUpdate_186)
					{
						StoC_0x83_QuestUpdate_186.NewQuestUpdate_186 newQuest = (pak as StoC_0x83_QuestUpdate_186).SubData as StoC_0x83_QuestUpdate_186.NewQuestUpdate_186;
						if (newQuest != null && newQuest.index == (originalPak as CtoS_0x41_UseNewQuestItem).QuestIndex && newQuest.goalsCount > (ushort)slots[0])
						{
							StoC_0x02_InventoryUpdate.Item item = newQuest.goalItems[(ushort)slots[0]];
							str.AppendFormat("\nslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} \"{17}\"",
								item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name);
							if (item.name != null && item.name != "")
								str.AppendFormat(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
							if ((item.flag & 0x08) == 0x08)
								str.AppendFormat("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
								item.effectIcon, item.effectName);
							if ((item.flag & 0x10) == 0x10)
								str.AppendFormat("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
								item.effectIcon2, item.effectName2);
							break;
						}
					}
				}
			}

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Use skill/Cast spell info (right click to close)";
			infoWindow.Width = 800;
			infoWindow.Height = 200;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion
	}
}
