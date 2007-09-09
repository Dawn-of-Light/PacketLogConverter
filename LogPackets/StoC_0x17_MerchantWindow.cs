using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x17, -1, ePacketDirection.ServerToClient, "Merchant window")]
	public class StoC_0x17_MerchantWindow : Packet
	{
		protected byte itemCount;
		protected byte windowType;
		protected byte page;
		protected byte unk1;
		protected MerchantItem[] items;

		#region public access properties

		public byte ItemCount { get { return itemCount; } }
		public byte WindowType { get { return windowType; } }
		public byte Page { get { return page; } }
		public byte Unk1 { get { return unk1; } }
		public MerchantItem[] Items { get { return items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("\n\tcount:{0,-2} windowType:{1}({4}) page:{2} unk1:{3}", itemCount, windowType, page, unk1, (eMerchantWindowType)windowType);

			for (int i = 0; i < itemCount; i++)
			{
				MerchantItem item = items[i];
				str.AppendFormat("\n\tindex:{0,-2} level:{1,-2} value1:{2,-3} value2:{3,-3} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} canUse:{7} weight:{8,-4} price:{9,-8} model:0x{10:X4} name:\"{11}\"",
					item.index, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.canUse, item.weight, item.price, item.model, item.name);
				if (flagsDescription)
					str.AppendFormat(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
    		}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			itemCount = ReadByte();
			windowType = ReadByte();
			page = ReadByte();
			unk1 = ReadByte();

			items = new MerchantItem[itemCount];

			for (int i = 0; i < itemCount; i++)
			{
				MerchantItem item = new MerchantItem();

				item.index = ReadByte();
				item.level = ReadByte();
				item.value1 = ReadByte();
				item.value2 = ReadByte();
				item.hand = ReadByte(); // >>6 to get hand
				item.damageAndObjectType = ReadByte();
				if (windowType < (byte)eMerchantWindowType.HousingOutsideMenu)
				{
					item.damageType = (byte)(item.damageAndObjectType >> 6);
					item.objectType = (byte)(item.damageAndObjectType & 0x3F);
				}
				else
				{
					item.objectType = item.damageAndObjectType;
				}
				item.canUse = ReadByte();
				item.weight = ReadShort();
				item.price = ReadInt();
				item.model = ReadShort();
				item.name = ReadPascalString();

				items[i] = item;
			}
		}

		public struct MerchantItem
		{
			public byte index;
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte damageAndObjectType; //original bytes
			public byte damageType;
			public byte objectType;
			public byte canUse;
			public ushort weight;
			public uint price;
			public ushort model;
			public string name;
		}

		public enum eMerchantWindowType : byte
		{
			Normal = 0x00,
			Bp = 0x01,
			Count = 0x02,
			HousingOutsideMenu = 0x04,
			HousingMerchant = 0x05,
			HousingInsideShop = 0x06,
			HousingOutsideShop = 0x07,
			HousingVault = 0x08,
			HousingTools = 0x09,
			HousingBindstone = 0x0A,
			HousingInsideMenu = 0x0B,
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x17_MerchantWindow(int capacity) : base(capacity)
		{
		}
	}
}