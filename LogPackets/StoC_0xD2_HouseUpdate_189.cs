using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD2, 189, ePacketDirection.ServerToClient, "House update v189")]
	public class StoC_0xD2_HouseUpdate_189 : StoC_0xD2_HouseUpdate
	{
		protected ushort scheduled;

		public override string GetPacketDataString(bool flagsDescription)
		{

			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} scheduled:{1} count:{2} code:0x{3:X2}", houseOid, scheduled, count, code);
			if (flagsDescription)
				if (scheduled > 0)
					str.AppendFormat(" ({0} days)", scheduled / 24);

			for (int i = 0; i < m_items.Length; i++)
			{
				Item item = (Item)m_items[i];
				str.AppendFormat("\n\tindex:{0,-2} model:0x{1:X4} place:0x{2:X2} rotation:{3}",
					item.index, item.model, item.place, item.rotation);
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
			scheduled = ReadShort();
			count = ReadByte();
			code = ReadByte();
			m_items = new Item[count];

			for (int i = 0; i < count; i++)
			{
				Item item = new Item();

				item.index = ReadByte();
				item.model = ReadShort();
				item.place = ReadByte();
				item.rotation = ReadByte();

				m_items[i] = item;
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD2_HouseUpdate_189(int capacity) : base(capacity)
		{
		}
	}
}