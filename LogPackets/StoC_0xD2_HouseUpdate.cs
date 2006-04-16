using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD2, -1, ePacketDirection.ServerToClient, "House update")]
	public class StoC_0xD2_HouseUpdate : Packet, IOidPacket
	{
		protected int houseOid;
		protected int count;
		protected int code;
		protected Item[] m_items;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public int Oid { get { return houseOid; } }
		public int Count { get { return count; } }
		public int UpdateCode { get { return code; } }
		public Item[] Items { get { return m_items; } }

		#endregion

		public override string GetPacketDataString()
		{

			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} count:{1} code:0x{2:X2}", houseOid, count, code);

			for (int i = 0; i < m_items.Length; i++)
			{
				Item item = (Item)m_items[i];
				str.AppendFormat("\n\tindex:{0,-2} model:0x{1:X4} position:0x{2:X2} rotation:{3}",
					item.index, item.model, item.position, item.rotation);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			count = ReadByte();
			code = ReadByte();
			m_items = new Item[count];

			for (int i = 0; i < count; i++)
			{
				Item item = new Item();

				item.index = ReadByte();
				item.model = ReadShort();
				item.position = ReadByte();
				item.rotation = ReadByte();

				m_items[i] = item;
			}
		}

		public struct Item
		{
			public byte index;
			public ushort model;
			public byte position;
			public byte rotation;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD2_HouseUpdate(int capacity) : base(capacity)
		{
		}
	}
}