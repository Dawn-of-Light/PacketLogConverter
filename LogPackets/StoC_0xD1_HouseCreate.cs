using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD1, -1, ePacketDirection.ServerToClient, "House create")]
	public class StoC_0xD1_HouseCreate : Packet, IOidPacket
	{
		protected ushort houseOid;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort heading;
		protected byte unk1;
		protected byte colorPorch;
		protected byte unk2;
		protected int modelPorch;
		protected int emblem;
		protected byte modelHouse;
		protected byte modelRoof;
		protected byte modelWall;
		protected byte modelDoor;
		protected byte modelTruss;
		protected byte materialPorch;
		protected byte modelWindow;
		protected string name;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid{ get { return houseOid; } }
		public ushort Z { get { return z; } }
		public uint X{ get { return x; } }
		public uint Y{ get { return y; } }
		public ushort Heading { get { return heading; } }
		public byte Unk1 { get { return unk1; } }
		public byte ColorPorch { get { return colorPorch; } }
		public byte Unk2 { get { return unk2; } }
		public int ModelPorch { get { return modelPorch; } }
		public int Emblem { get { return emblem; } }
		public byte ModelHouse { get { return modelHouse; } }
		public byte ModelRoof { get { return modelRoof; } }
		public byte ModelWall { get { return modelWall; } }
		public byte ModelDoor { get { return modelDoor; } }
		public byte ModelTruss { get { return modelTruss; } }
		public byte MaterialPorch { get { return materialPorch; } }
		public byte ModelWindow { get { return modelWindow; } }
		public string Name { get { return name; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} heading:0x{1:X4} x:{2,-6} y:{3,-6} z:{4,-5} ownerName:\"{17}\"\n\tunk1:0x{5:X2} colorPorch:{6,-2} unk2:0x{7:X2} modelPorch:{8} emblem:0x{9:X4} modelHouse:{10} modelRoof:{11} modelWall:{12} modelDoor:{13} modelTruss:{14} materialPorch:{15} modelWindow:{16,-3}",
				houseOid, heading, x, y, z, unk1, colorPorch, unk2, modelPorch, emblem, modelHouse, modelRoof, modelWall, modelDoor, modelTruss, materialPorch, modelWindow, name);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			heading = ReadShort();
			unk1 = ReadByte();
			colorPorch = ReadByte();
			unk1 = ReadByte();
			modelPorch = ReadByte();
			unk2 = ReadByte();
			emblem = ReadShort();
			modelHouse= ReadByte();
			modelRoof = ReadByte();
			modelWall = ReadByte();
			modelDoor = ReadByte();
			modelTruss = ReadByte();
			materialPorch = ReadByte();
			modelWindow = ReadByte();
			name = ReadPascalString();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD1_HouseCreate(int capacity) : base(capacity)
		{
		}
	}
}