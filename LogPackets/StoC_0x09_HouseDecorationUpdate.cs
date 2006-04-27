using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x09, -1, ePacketDirection.ServerToClient, "House decoration update")]
	public class StoC_0x09_HouseDecorationUpdate : Packet, IOidPacket
	{
		protected ushort houseOid;
		protected byte count;
		protected byte decorationCode;
		protected Furniture[] m_furnitures;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return houseOid; } }
		public byte Count { get { return count; } }
		public byte DecorationCode { get { return decorationCode; } }
		public Furniture[] Furnitures { get { return m_furnitures; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} count:{1} code:0x{2:X2}", houseOid, count, decorationCode);

			for (int i = 0; i < count; i++)
			{
				Furniture furniture = (Furniture)m_furnitures[i];
				str.AppendFormat("\n\tindex:{0,2} model:0x{1:X4} color:0x{2:X4} unk1:0x{3:X4} (x:{4,-5} y:{5,-5}) angle:{6} size:{7,3}% position:{8,-2} placemode:{9}",
				furniture.index, furniture.model, furniture.color, furniture.unk1, furniture.x, furniture.y, furniture.angle, furniture.size, furniture.position, furniture.placemode);
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
			decorationCode = ReadByte();
			m_furnitures = new Furniture[count];
			for (int i = 0; i < count; i++)
			{
				Furniture furniture = new Furniture();

				furniture.index = ReadByte();
				furniture.model = ReadShort();
				furniture.color = ReadShort();
				furniture.unk1 = ReadShort();
				furniture.x = (short)ReadShort();
				furniture.y = (short)ReadShort();
				furniture.angle = ReadShort();
				furniture.size = ReadByte();
				furniture.position = ReadByte();
				furniture.placemode = ReadByte();

				m_furnitures[i] = furniture;
			}
		}

		public struct Furniture
		{
			public byte index;
			public ushort model;
			public ushort color;
			public ushort unk1;
			public short x;
			public short y;
			public ushort angle;
			public byte size;
			public byte position;
			public byte placemode;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x09_HouseDecorationUpdate(int capacity) : base(capacity)
		{
		}
	}
}