using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDA, -1, ePacketDirection.ServerToClient, "Npc create")]
	public class StoC_0xDA_NpcCreate : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected short speed;
		protected ushort heading;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected short speedZ;
		protected ushort model; // ID from monsters.csv
		protected byte size;
		protected byte level;
		protected byte flags;
		protected byte maxStick;
		protected string name;
		protected string guildName;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public short Speed { get { return speed; } }
		public ushort Heading { get { return heading; } }
		public ushort Z { get { return z; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public short SpeedZ { get { return speedZ; } }
		public ushort Model { get { return model; } }
		public byte Size { get { return size; } }
		public byte Level { get { return level; } }
		public byte Flags { get { return flags; } }
		public byte MaxStick { get { return maxStick; } }
		public string Name { get { return name; } }
		public string GuildName { get { return guildName; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} speed:{1,-4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} speedZ:{6,-4} model:0x{7:X4} size:{8,-3}({15,-3}) level:{9,-3} flags:0x{10:X2} maxStick:{11,-3} name:\"{12}\" guild:\"{13}\" unk1:{14}",
			                 oid, speed, heading, x, y, z, speedZ, model & 0x3FFF, size, level, flags & 0x3F, maxStick, name, guildName, unk1, size * 2);
			if (flagsDescription)
			{
				text.Write(" (realm:{0}", (flags >> 6) & 3);
				if ((flags & 0x01) == 0x01)
					text.Write(",Ghost");
				if ((flags & 0x02) == 0x02)
					text.Write(",Inventory");
				if ((flags & 0x04) == 0x04)
					text.Write(",Torch");
				if ((flags & 0x08) == 0x08)
					text.Write(",LongRangeVisible"); // 4000, 5500, 8000
				if ((flags & 0x10) == 0x10)
					text.Write(",Peace");
				if ((flags & 0x20) == 0x20)
					text.Write(",Fly");
				if ((model & 0x4000) == 0x4000)
					text.Write(",UNK_Model_0x4000");
				if ((model & 0x8000) == 0x8000)
					text.Write(",Underwater");
				text.Write(')');
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();          // 0x00
			speed = (short)ReadShort(); // 0x02
			heading = ReadShort();      // 0x04
			z = ReadShort();            // 0x06
			x = ReadInt();              // 0x08
			y = ReadInt();              // 0x0C
			speedZ = (short)ReadShort();// 0x10
			model = ReadShort();        // 0x12
			size = ReadByte();          // 0x14
			level = ReadByte();         // 0x15
			flags = ReadByte();         // 0x16
			maxStick = ReadByte();      // 0x17
			name = ReadPascalString();  // 0x18
			guildName = ReadPascalString();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDA_NpcCreate(int capacity) : base(capacity)
		{
		}
	}
}