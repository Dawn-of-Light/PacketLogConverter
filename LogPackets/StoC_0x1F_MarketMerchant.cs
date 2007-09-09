using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1F, -1, ePacketDirection.ServerToClient, "Market merchant")]
	public class StoC_0x1F_MarketMerchant: Packet
	{
		protected sbyte itemCount;
		protected byte page;
		protected byte maxPages;
		protected byte unk1;
		protected MerchantItem[] items;

		#region public access properties

		public sbyte ItemCount { get { return itemCount; } }
		public byte Page { get { return page; } }
		public byte MaxPages { get { return maxPages; } }
		public byte Unk1 { get { return unk1; } }
		public MerchantItem[] Items { get { return items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("\n\tcount:{0,-2} page:{1} pages:{2} unk1:0x{3:X2}", itemCount, page, maxPages, unk1);
			if (itemCount > 0)
			{
				for (int i = 0; i < itemCount; i++)
				{
					MerchantItem item = items[i];
					str.AppendFormat("\n\tindex:{0,-2} level:{1,-2} value1:{2,-3} value2:{3,-3} hand:0x{4:X2} damageAndObjectType:0x{5:X2} canUse:{6} weigh:{7,-4} condition:{8,3} durability:{9,3} quality:{10,3} bonus:{11,2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X4} lot:0x{15:X4} price:{16,-8} name:\"{17}\"",
						item.index,
						item.level,
						item.value1,
						item.value2,
						item.hand,
						item.damageAndObjectType,
						item.canUse,
						item.weight/10f,
						item.condition,
						item.durability,
						item.quality,
						item.bonus,
						item.model,
						item.color,
						item.effect,
						item.lot,
						item.price,
						item.name);
					if (flagsDescription)
						str.AppendFormat(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)(item.damageAndObjectType & 0x3F));
				}
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			itemCount = (sbyte)ReadByte();
			page = ReadByte();
			maxPages = ReadByte();
			unk1 = ReadByte();
			if (itemCount > 0)
			{
				items = new MerchantItem[itemCount];

				for (int i = 0; i < itemCount; i++)
				{
					MerchantItem item = new MerchantItem();

					item.index = ReadByte();
					item.level = ReadByte();
					item.value1 = ReadByte(); // DPS_AF
					item.value2 = ReadByte(); // SPD_ABS
					item.hand = ReadByte();
					item.damageAndObjectType = ReadByte(); // Hand + ObjectType ?
					item.canUse = ReadByte();
					item.weight = ReadShort();
					item.condition = ReadByte();
					item.durability = ReadByte();
					item.quality = ReadByte();
					item.bonus = ReadByte();
					item.model = ReadShort();
					item.color = ReadShort();
					item.effect = ReadShort();
					item.lot = ReadShort();
					item.price = ReadInt();
					item.name = ReadPascalString(); // max length = 0x2F ?

					items[i] = item;
				}
			}
		}

		public struct MerchantItem
		{
			public byte index;
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte damageAndObjectType;
			public byte canUse;
			public ushort weight;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public ushort color;
			public ushort effect;
			public ushort lot;
			public uint price;
			public string name;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x1F_MarketMerchant(int capacity) : base(capacity)
		{
		}
	}
}