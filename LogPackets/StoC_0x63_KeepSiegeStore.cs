using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x63, -1, ePacketDirection.ServerToClient, "hookpoint siege store")]
	public class StoC_0x63_KeepSiegeStore : Packet, IKeepIdPacket
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
		protected StoC_0x17_MerchantWindow.MerchantItem[] items;

		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
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
		public StoC_0x17_MerchantWindow.MerchantItem[] Items { get { return items; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keepId:0x{0:X4} componentId:{1,-2} hookPointId:0x{2:X4} buyMoney:{3} buyBP:{4} buyGBP:{5} count:{6} unk1:{7} unk2:{8}",
				keepId, componentId, hookPointId, flag1, flag2, flag3, itemCount, unk1, unk2);

			for (int i = 0; i < itemCount; i++)
			{
				StoC_0x17_MerchantWindow.MerchantItem item = items[i];
				text.Write("\n\tindex:{0,-2} level:{1,-2} value1:{2,-3} value2:{3,-3} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} canUse:{7} weight:{8,-4} price:{9,-8} model:0x{10:X4} name:\"{11}\"",
					item.index, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.canUse, item.weight, item.price, item.model, item.name);
				if (flagsDescription)
					text.Write(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
			}
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

			items = new StoC_0x17_MerchantWindow.MerchantItem[itemCount];

			for (int i = 0; i < itemCount; i++)
			{
				StoC_0x17_MerchantWindow.MerchantItem item = new StoC_0x17_MerchantWindow.MerchantItem();

				item.index = ReadByte();
				item.level = ReadByte();
				item.value1 = ReadByte();
				item.value2 = ReadByte();
				item.hand = ReadByte(); // >>6 to get hand
				item.damageAndObjectType = ReadByte();
//				item.damageType = (byte)(item.damageAndObjectType >> 6);
//				item.objectType = (byte)(item.damageAndObjectType & 0x3F);
				item.objectType = item.damageAndObjectType; // KeepSiegeStore can't sold weapon
				item.canUse = ReadByte();
				item.weight = ReadShort();
				item.price = ReadInt();
				item.model = ReadShort();
				item.name = ReadPascalString();

				items[i] = item;
			}
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