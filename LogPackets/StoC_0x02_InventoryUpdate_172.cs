using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x02, 172, ePacketDirection.ServerToClient, "Inventory update v172")]
	public class StoC_0x02_InventoryUpdate_172 : StoC_0x02_InventoryUpdate
	{
		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(16 + m_slotsCount*32);
			str.AppendFormat("slots:{0} bits:0x{1:X2} visibleSlots:0x{2:X2} preAction:0x{3:X2}", SlotsCount, Bits, VisibleSlots, PreAction);
			if (flagsDescription)
				str.AppendFormat("({0})", (ePreActionType)PreAction);

			for (int i = 0; i < m_slotsCount; i++)
			{
				Item item = Items[i];

				str.AppendFormat("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} \"{17}\"",
				                 item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			m_slotsCount = ReadByte();
			m_bits = ReadByte();
			m_visibleSlots = ReadByte();
			m_preAction = ReadByte();

			m_items = new Item[m_slotsCount];

			for (int i = 0; i < m_slotsCount; i++)
			{
				Item item = new Item();

				item.slot = ReadByte();
				item.level = ReadByte();

				item.value1 = ReadByte();
				item.value2 = ReadByte();

				item.hand = ReadByte();
				byte temp = ReadByte(); //WriteByte((byte) ((item.Type_Damage*64) + item.Object_Type));
				item.damageType = (byte)(temp >> 6);
				item.objectType = (byte)(temp & 0x3F);
				item.weight = ReadShort();
				item.condition = ReadByte();
				item.durability = ReadByte();
				item.quality = ReadByte();
				item.bonus = ReadByte();
				item.model = ReadShort();
				item.extension = ReadByte();
				item.color = ReadShort();
				item.flag = ReadByte();
				item.effect = ReadByte();
				item.name = ReadPascalString();

				m_items[i] = item;
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x02_InventoryUpdate_172(int capacity) : base(capacity)
		{
		}
	}
}