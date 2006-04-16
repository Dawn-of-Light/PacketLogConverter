using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDA, -1, ePacketDirection.ServerToClient, "Npc create")]
	public class StoC_0xDA_NpcCreate : Packet, IOidPacket
	{
		protected ushort oid;
		protected ushort speed;
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

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort Speed { get { return speed; } }
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

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} speed:{1,-3} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} speedZ:{6,-4} model:0x{7:X4} size:{8,-3} level:{9,-3} flags:0x{10:X2} maxStick:{11,-3} name:\"{12}\" guild:\"{13}\" unk1:{14}",
			                 oid, speed, heading, x, y, z, speedZ, model, size, level, flags, maxStick, name, guildName, unk1);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			speed = ReadShort();
			heading = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			speedZ = (short)ReadShort();
			model = ReadShort();
			size = ReadByte();
			level = ReadByte();
			flags = ReadByte();
			maxStick = ReadByte();
			name = ReadPascalString();
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