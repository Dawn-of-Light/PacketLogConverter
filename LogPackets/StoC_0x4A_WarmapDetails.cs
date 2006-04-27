using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4A, -1, ePacketDirection.ServerToClient, "Warmap Details")]
	public class StoC_0x4A_WarmapDetails: Packet
	{
		protected byte count;
		protected byte countGroups;
		protected Item[] m_items;

		public enum IconColor: byte
		{
			Gray = 0,
			RedBlue = 1,
			RedGreen = 2,
			GreenBlue = 3
		}

		#region public access properties

		public byte Count { get { return count; } }
		public byte CountGroups { get { return countGroups; } }
		public Item[] Items { get { return m_items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{

			StringBuilder str = new StringBuilder();

			str.AppendFormat("countFights:{0} countGroups:{1}", count, countGroups);
			for (int i = 0; i < count; i++)
			{
				Item item = m_items[i];
				str.AppendFormat("\n\tzone:{0,-3} offset:0x{1:X2}(X:{2}, Y:{3}) color:{4}({6}) size:{5}",
					item.zone, item.loc, item.loc >> 4, item.loc & 0x0F, item.color, item.type, ((IconColor)item.color).ToString());
			}
			for (int i = count; i < count + countGroups; i++)
			{
				Item item = m_items[i];
				string realmInfo = "";
				if ((item.color & 0x01) == 0x01) // albs
				{
					realmInfo += string.Format("AlbSize:{0}", (item.type >> 0) & 0x03);
				}
				if ((item.color & 0x02) == 0x2) // mids
				{
					if (realmInfo !="") realmInfo += ", ";
					realmInfo += string.Format("MidSize:{0}", (item.type >> 2) & 0x03);
				}
				if ((item.color & 0x04) == 0x04) // hibs
				{
					if (realmInfo !="") realmInfo += ", ";
					realmInfo += string.Format("HibSize:{0}", (item.type >> 4) & 0x03);
				}
				str.AppendFormat("\n\tzone:{0,-3} offset:0x{1:X2}(X:{2}, Y:{3}) realmBitMask:{4} icon:0x{5:X2}({6})",
					item.zone, item.loc, item.loc >> 4, item.loc & 0x0F, item.color, item.type, realmInfo);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			count = ReadByte();
			countGroups = ReadByte();
			m_items = new Item[count + countGroups];

			for (int i = 0; i < count + countGroups; i++)
			{
				Item item = new Item();

				item.zone = ReadByte();
				item.loc = ReadByte();
				item.color = ReadByte();
				item.type = ReadByte();

				m_items[i] = item;
			}
		}

		public struct Item
		{
			public byte zone;
			public byte loc; // XY offset, 1/4 zone map (0x00 - upper left corner, 0x33 - right bottom)
			public byte color;
			public byte type;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4A_WarmapDetails(int capacity) : base(capacity)
		{
		}
	}
}