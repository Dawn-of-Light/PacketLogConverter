using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x09, 176, ePacketDirection.ServerToClient, "House decoration update 176")]
	public class StoC_0x09_HouseDecorationUpdate_176 : StoC_0x09_HouseDecorationUpdate
	{

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} count:{1} code:0x{2:X2}", houseOid, count, decorationCode);

			for (int i = 0; i < count; i++)
			{
				Furniture furniture = (Furniture)m_furnitures[i];
				str.AppendFormat("\n\tindex:{0,2} type:{1:X2} model:0x{2:X4} color:0x{3:X4} (x:{4,-5} y:{5,-5}) angle:{6,-3} size:{7,3}% surface:{8,-2} place:{9}({10})",
				furniture.index, furniture.type, furniture.model, furniture.color, furniture.x, furniture.y, furniture.angle, furniture.size, furniture.surface, furniture.place, (ePlaceType)furniture.place);
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
				furniture.type = ReadByte();
				switch(furniture.type)
				{
					case 0:
						furniture.model = ReadShort();
						furniture.x = (short)ReadShort();
						furniture.y = (short)ReadShort();
						furniture.angle = ReadShort();
						break;
					case 8:
						furniture.model = ReadShort();
						furniture.x = (short)ReadShort();
						furniture.y = (short)ReadShort();
						furniture.angle = ReadShort();
						furniture.size = ReadByte();
						break;
					case 1:
					default:
						furniture.model = ReadShort();
						furniture.color = ReadByte();
						furniture.x = (short)ReadShort();
						furniture.y = (short)ReadShort();
						furniture.angle = ReadShort();
						break;
				}
				furniture.surface = ReadByte();
				furniture.place = ReadByte();

				m_furnitures[i] = furniture;
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x09_HouseDecorationUpdate_176(int capacity) : base(capacity)
		{
		}
	}
}