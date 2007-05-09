using System.Text;
using System.Collections;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x63, -1, ePacketDirection.ServerToClient, "hookpoint siege store")]
	public class StoC_0x63_KeepSiegeStore : Packet, IObjectIdPacket
	{
		protected ushort keepId;
		protected ushort componentId;
		protected ushort hookPointId;
		protected byte flag1; // can Buy for Money
		protected byte flag2; // can Buy for BP
		protected byte flag3; // cab Buy for GBP
		protected byte itemCount;
		protected byte unk1; // windowType ?
		protected byte unk2; // page ?
		protected MerchantItem[] items;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort ComponentId { get { return componentId; } }
		public ushort HookPointId { get { return hookPointId; } }
		public byte Flag1 { get { return flag1; } }
		public byte Flag2 { get { return flag2; } }
		public byte Flag3 { get { return flag3; } }
		public byte ItemCount { get { return itemCount; } }
		public MerchantItem[] Items { get { return items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} componentId:0x{1:X4} hookPointId:0x{2:X4} buyMoney:{3} buyBP:{4} buyGBP:{5} count:{6} unk1:{7} unk2:{8}",
				keepId, componentId, hookPointId, flag1, flag2, flag3, itemCount, unk1, unk2);

			for (int i = 0; i < itemCount; i++)
			{
				MerchantItem item = items[i];
				str.AppendFormat("\n\tindex:{0,-2} level:{1,-2} value1:{2,-3} spd_abs:{3,-3} hand:0x{4:X2} damageAndObjectType:0x{5:X2} canUse:{6} value2:0x{7:X4} price:{8,-8} model:0x{9:X4} name:\"{10}\"",
					item.index, item.level, item.value1, item.spd_abs, item.hand, item.damageAndObjectType, item.canUse, item.value2, item.price, item.model, item.name);
			}
			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();
			componentId = ReadShort();
			hookPointId = ReadShort();
			flag1 = ReadByte();
			flag2 = ReadByte();
			flag3 = ReadByte();
			itemCount = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadByte();

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
		public StoC_0x63_KeepSiegeStore(int capacity) : base(capacity)
		{
		}
	}
}