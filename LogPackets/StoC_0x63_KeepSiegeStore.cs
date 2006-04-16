using System.Text;
using System.Collections;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x63, -1, ePacketDirection.ServerToClient, "hookpoint siege store")]
	public class StoC_0x63_KeepSiegeStore : Packet
	{
		protected ushort keepId;
		protected ushort componentId;
		protected ushort hookPointId;
		protected byte flag1;
		protected byte flag2;
		protected byte flag3;
		protected byte count;
		protected byte unk1;
		protected byte unk2;
		protected Item[] siegeItems;

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort ComponentId { get { return componentId; } }
		public ushort HookPointId { get { return hookPointId; } }
		public byte Flag1 { get { return flag1; } }
		public byte Flag2 { get { return flag2; } }
		public byte Flag3 { get { return flag3; } }
		public byte Count { get { return count; } }
		public byte Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }
		public Item[] SiegeItems { get { return siegeItems; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} componentId:0x{1:X4} hookPointId:0x{2:X4} f1:{3} f2:{4} f3:{5} count:{6} unk1:{7} unk2:{8}",
				keepId, componentId, hookPointId, flag1, flag2, flag3, count, unk1, unk2);
			for (int i = 0; i < count; i++)
			{
				Item item = siegeItems[i];
				str.AppendFormat("\n\tid:0x{0:X2} unk1:0x{1:X4} unk2:0x{2:X4} unk3:0x{3:X4} unk4:0x{4:X4} price:{5,-3} icon:0x{6:X4} name:{7}",
					item.id, item.unk1, item.unk2, item.unk3, item.unk4, item.price, item.icon, item.name);
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
			count = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadByte();
			siegeItems = new Item[count];
			for(int i = 0; i < count; i++)
			{
				Item item = new Item();
				item.id = ReadByte();
				item.unk1 = ReadShort();
				item.unk2 = ReadShort();
				item.unk3 = ReadShort();
				item.unk4 = ReadShort();
				item.price = ReadInt();
				item.icon = ReadShort();
				item.name = ReadPascalString();
				siegeItems[i] = item;
			}
		}

		public struct Item
		{
			public byte id;
			public ushort unk1;
			public ushort unk2;
			public ushort unk3;
			public ushort unk4;
			public uint price;
			public ushort icon;
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