using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4B, 172, ePacketDirection.ServerToClient, "Player create v172")]
	public class StoC_0x4B_PlayerCreate_172 : Packet, IObjectIdPacket
	{
		protected ushort sessionId;
		protected ushort oid;
		protected ushort model;
		protected ushort zoneZ;
		protected ushort zoneId;
		protected ushort zoneX;
		protected ushort zoneY;
		protected ushort heading;
		protected byte eyeSize;
		protected byte lipSize;
		protected byte eyeColor;
		protected byte level;
		protected byte hairColor;
		protected byte faceType;
		protected byte flags;
		protected string name;
		protected string guildName;
		protected string lastName;
		protected string unk1;
		protected byte trailingZero;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid, sessionId }; }
		}

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Oid { get { return oid; } }
		public ushort Model { get { return model; } }
		public ushort ZoneZ { get { return zoneZ; } }
		public ushort ZoneId { get { return zoneId; } }
		public ushort ZoneX { get { return zoneX; } }
		public ushort ZoneY { get { return zoneY; } }
		public ushort Heading { get { return heading; } }
		public byte EyeSize { get { return eyeSize; } }
		public byte LipSize { get { return lipSize; } }
		public byte EyeColor { get { return eyeColor; } }
		public byte Level { get { return level; } }
		public byte HairColor { get { return hairColor; } }
		public byte FaceType { get { return faceType; } }
		public byte Flags { get { return flags; } }
		public string Name { get { return name; } }
		public string GuildName { get { return guildName; } }
		public string LastName { get { return lastName; } }
		public string Unk1 { get { return unk1; } }
		public byte TrailingZero { get { return trailingZero; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} oid:0x{1:X4} model:0x{2:X4} zoneId:{3,-3} zoneLoc:({4,-5} {5,-5} {6,-5}) heading:0x{7:X4} eyeSize:0x{8:X4} lipSize:0x{9:X4} eyeColor:0x{10:X4} level:{11,-2} hairColor:0x{12:X4} faceType:0x{13:X4} flags:0x{14:X2} name:\"{15}\" guild:\"{16}\" lastName:\"{17}\" unk1:\"{18}\" trailingZero:{19}",
				sessionId, oid, model, zoneId, zoneX, zoneY, zoneZ, heading, eyeSize, lipSize, eyeColor, level, hairColor, faceType, flags, name, guildName, lastName, unk1, trailingZero);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			oid = ReadShort();
			model = ReadShort();
			zoneZ = ReadShort();
			zoneId = ReadShort();
			zoneX = ReadShort();
			zoneY = ReadShort();
			heading = ReadShort();
			eyeSize = ReadByte();
			lipSize = ReadByte();
			eyeColor = ReadByte();
			level = ReadByte();
			hairColor = ReadByte();
			faceType = ReadByte();
			hairColor = ReadByte();
			flags = ReadByte();
			name = ReadPascalString();
			guildName = ReadPascalString();
			lastName = ReadPascalString();
			unk1 = ReadPascalString();
			trailingZero = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4B_PlayerCreate_172(int capacity) : base(capacity)
		{
		}
	}
}