using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD1, -1, ePacketDirection.ServerToClient, "House create")]
	public class StoC_0xD1_HouseCreate : Packet, IObjectIdPacket
	{
		protected ushort houseOid;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort heading;
		protected byte unk1;
		protected ushort colorPorch;
		protected byte unk2;
		protected byte modelPorch;
		protected ushort emblem;
		protected byte modelHouse;
		protected byte modelRoof;
		protected byte modelWall;
		protected byte modelDoor;
		protected byte modelTruss;
		protected byte materialPorch;
		protected byte materialShutter;
		protected string name;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { houseOid }; }
		}

		#region public access properties

		public ushort Oid{ get { return houseOid; } }
		public ushort Z { get { return z; } }
		public uint X{ get { return x; } }
		public uint Y{ get { return y; } }
		public ushort Heading { get { return heading; } }
		public byte Unk1 { get { return unk1; } }
		public ushort ColorPorch { get { return colorPorch; } }
		public byte Unk2 { get { return unk2; } }
		public byte ModelPorch { get { return modelPorch; } }
		public ushort Emblem { get { return emblem; } }
		public byte ModelHouse { get { return modelHouse; } }
		public byte ModelRoof { get { return modelRoof; } }
		public byte ModelWall { get { return modelWall; } }
		public byte ModelDoor { get { return modelDoor; } }
		public byte ModelTruss { get { return modelTruss; } }
		public byte MaterialPorch { get { return materialPorch; } }
		public byte MaterialShutter { get { return materialShutter; } }
		public string Name { get { return name; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} heading:0x{1:X4} x:{2,-6} y:{3,-6} z:{4,-5} ownerName:\"{17}\"\n\ttentColor:0x{5:X4} unk1:0x{6:X2} modelPorch:{7,-2} emblem:0x{8:X4} modelHouse:{9,-2} roofMaterial:{10} wallMaterial:{11} doorMaterial:{12} woodColor:{13} porchMaterial:{14} shutterMaterial:{15,-3} unk2:0x{16:X2}",
				houseOid, heading, x, y, z, colorPorch, unk1, modelPorch, emblem, modelHouse, modelRoof, modelWall, modelDoor, modelTruss, materialPorch, materialShutter, unk2, name);

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
			colorPorch = ReadShort();
			unk1 = ReadByte();
			modelPorch = ReadByte();
			emblem = ReadShort();
			modelHouse= ReadByte();
			modelRoof = ReadByte();
			modelWall = ReadByte();
			modelDoor = ReadByte();
			modelTruss = ReadByte();
			materialPorch = ReadByte();
			materialShutter = ReadByte();
			unk2 = ReadByte();
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