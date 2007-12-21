using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x09, -1, ePacketDirection.ServerToClient, "House decoration update")]
	public class StoC_0x09_HouseDecorationUpdate : Packet, IHouseIdPacket
	{
		protected ushort houseOid;
		protected byte count;
		protected byte decorationCode;
		protected Furniture[] m_furnitures;

		#region public access properties

		public ushort HouseId { get { return houseOid; } }
		public byte Count { get { return count; } }
		public byte DecorationCode { get { return decorationCode; } }
		public Furniture[] Furnitures { get { return m_furnitures; } }

		#endregion

		public enum ePlaceType: byte
		{
			wallDecoration = 0,
			floorDecoration = 1,
			exteriorDecoration = 2,
			hookPoints = 3
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} count:{1} code:0x{2:X2}", houseOid, count, decorationCode);

			for (int i = 0; i < count; i++)
			{
				Furniture furniture = (Furniture)m_furnitures[i];
				if (furniture.flagRemove)
					str.AppendFormat("\n\tindex:{0,2} remove", furniture.index);
				else
					str.AppendFormat("\n\tindex:{0,2} model:0x{1:X4} color:0x{2:X4} unk1:0x{3:X2}{4:X2} (x:{5,-5} y:{6,-5}) angle:{7,-3} size:{8,3}% surface:{9,-2} place:{10}({11})",
					furniture.index, furniture.model, furniture.color, furniture.type, furniture.unk1, furniture.x, furniture.y, furniture.angle, furniture.size, furniture.surface, furniture.place, (ePlaceType)furniture.place);
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
				furniture.flagRemove = false;
				furniture.index = ReadByte();
				if (Position < Length)
				{
					furniture.model = ReadShort();
					furniture.color = ReadShort();
					furniture.type = ReadByte();
					furniture.unk1 = ReadByte();
					furniture.x = (short)ReadShort();
					furniture.y = (short)ReadShort();
					furniture.angle = ReadShort();
					furniture.size = ReadByte();
					furniture.surface = ReadByte();
					furniture.place = ReadByte();
				}
				else
					furniture.flagRemove = true;
				m_furnitures[i] = furniture;
			}
		}

		public struct Furniture
		{
			public byte index;
			public ushort model;
			public ushort color;
			public byte type;
			public byte unk1;
			public short x;
			public short y;
			public ushort angle;
			public byte size;
			public byte surface;
			public byte place;
			public bool flagRemove;
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