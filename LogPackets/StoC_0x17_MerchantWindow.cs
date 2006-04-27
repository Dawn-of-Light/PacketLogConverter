using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x17, -1, ePacketDirection.ServerToClient, "Merchant window")]
	public class StoC_0x17_MerchantWindow : Packet
	{
		protected int itemCount;
		protected int windowType;
		protected int page;
		protected int unk1;
		protected MerchantItem[] items;

		#region public access properties

		public int ItemCount { get { return itemCount; } }
		public int WindowType { get { return windowType; } }
		public int Page { get { return page; } }
		public int Unk1 { get { return unk1; } }
		public MerchantItem[] Items { get { return items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("\n\tcount:{0,-2} windowType:{1} page:{2} unk1:{3}", itemCount, windowType, page, unk1);

			for (int i = 0; i < itemCount; i++)
			{
				MerchantItem item = items[i];
				str.AppendFormat("\n\tindex:{0,-2} level:{1,-2} value1:{2,-3} spd_abs:{3,-3} hand:0x{4:X2} damageAndObjectType:0x{5:X2} canUse:{6} value2:0x{7:X4} price:{8,-8} model:0x{9:X4} name:\"{10}\"",
				                 item.index, item.level, item.value1, item.spd_abs, item.hand, item.damageAndObjectType, item.canUse, item.value2, item.price, item.model, item.name);
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
				item.spd_abs = ReadByte();
				item.hand = ReadByte(); // >>6 to get hand
				item.damageAndObjectType = ReadByte();
				item.canUse = ReadByte();
				item.value2 = ReadShort();
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
			public byte spd_abs;
			public byte hand;
			public byte damageAndObjectType;
			public byte canUse;
			public ushort value2;
			public uint price;
			public ushort model;
			public string name;
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