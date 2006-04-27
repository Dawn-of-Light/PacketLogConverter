using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x02, -1, ePacketDirection.ServerToClient, "Inventory update")]
	public class StoC_0x02_InventoryUpdate : Packet
	{
		protected byte m_slotsCount;
		protected byte m_bits;
		protected byte m_visibleSlots;
		protected byte m_preAction;
		protected Item[] m_items;

		#region public access properties

		public int SlotsCount { get { return m_slotsCount; } }
		public int Bits { get { return m_bits; } }
		public int VisibleSlots { get { return m_visibleSlots; } }
		public int PreAction { get { return m_preAction; } }
		public Item[] Items { get { return m_items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(16 + m_slotsCount*32);
			str.AppendFormat("slots:{0} bits:0x{1:X2} visibleSlots:0x{2:X2} preAction:0x{3:X2}", SlotsCount, Bits, VisibleSlots, PreAction);

			for (int i = 0; i < m_slotsCount; i++)
			{
				Item item = (Item)Items[i];
				str.AppendFormat("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"",
				                 item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name);
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
				item.color = ReadShort();
				item.flag = ReadByte();
				item.effect = ReadByte();
				item.name = ReadPascalString();

				m_items[i] = item;
			}
		}

		public struct Item
		{
			public byte slot;
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte temp;
			public byte damageType;
			public byte objectType;
			public ushort weight;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public ushort color;
			public byte effect;
			public byte flag;
			public string name;
			public byte extension; // new in 1.72
			public ushort effectIcon; // new 1.82
			public string effectName; // new 1.82
			public ushort effectIcon2; // new 1.82
			public string effectName2; // new 1.82
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x02_InventoryUpdate(int capacity) : base(capacity)
		{
		}
	}
}