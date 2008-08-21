using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD1, -1, ePacketDirection.ServerToClient, "House create")]
	public class StoC_0xD1_HouseCreate : Packet, IHouseIdPacket
	{
		protected ushort houseOid;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort heading;
		protected byte unk1;
		protected ushort colorPorch;
		protected byte unk2; // unused
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

		#region public access properties

		public ushort HouseId { get { return houseOid; } }
		public ushort Z { get { return z; } }
		public uint X{ get { return x; } }
		public uint Y{ get { return y; } }
		public ushort Heading { get { return heading; } }
		public byte Unk1 { get { return unk1; } }
		public ushort ColorPorch { get { return colorPorch; } }
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
		public byte Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("houseOid:0x{0:X4} heading:0x{1:X4} x:{2,-6} y:{3,-6} z:{4,-5} ownerName:\"{16}\"\n\ttentColor:0x{5:X4} unk1:0x{6:X2} modelPorch:{7,-2} emblem:0x{8:X4} modelHouse:{9,-2} roofMaterial:{10} wallMaterial:{11} doorMaterial:{12} woodColor:{13} porchMaterial:{14} shutterMaterial:{15,-3}",
				houseOid, heading, x, y, z, colorPorch, unk1, modelPorch, emblem, modelHouse, modelRoof, modelWall, modelDoor, modelTruss, materialPorch, materialShutter, name);
			if (flagsDescription)
				text.Write(" unk2:0x{0:X2}", unk2);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();      // 0x00
			z = ReadShort();             // 0x02
			x = ReadInt();               // 0x04
			y = ReadInt();               // 0x08
			heading = ReadShort();       // 0x0C
			colorPorch = ReadShort();    // 0x0E
			unk1 = ReadByte();           // 0x10
			modelPorch = ReadByte();     // 0x11
			emblem = ReadShort();        // 0x12
			modelHouse= ReadByte();      // 0x14
			modelRoof = ReadByte();      // 0x15
			modelWall = ReadByte();      // 0x16
			modelDoor = ReadByte();      // 0x17
			modelTruss = ReadByte();     // 0x18
			materialPorch = ReadByte();  // 0x19
			materialShutter = ReadByte();// 0x1A
			unk2 = ReadByte();           // 0x1B
			name = ReadPascalString();   // 0x1C+
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