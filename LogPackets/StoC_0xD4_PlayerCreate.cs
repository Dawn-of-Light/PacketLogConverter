using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD4, -1, ePacketDirection.ServerToClient, "Player create")]
	public class StoC_0xD4_PlayerCreate : Packet, IObjectIdPacket
	{
		protected ushort sessionId;
		protected ushort oid;
		protected ushort zoneX;
		protected ushort zoneY;
		protected byte zoneId;
		protected byte unk1;
		protected ushort zoneZ;
		protected ushort heading;
		protected ushort model;
		protected byte alive;
		protected byte unk2;
		protected byte realm;
		protected byte level;
		protected byte stealthed;
		protected byte unk3;
		protected string name;
		protected string guildName;
		protected string lastName;
		protected byte trailingZero;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Oid { get { return oid; } }
		public ushort ZoneX { get { return zoneX; } }
		public ushort ZoneY { get { return zoneY; } }
		public byte ZoneId { get { return zoneId; } }
		public byte Unk1 { get { return unk1; } }
		public ushort ZoneZ { get { return zoneZ; } }
		public ushort Heading { get { return heading; } }
		public ushort Model { get { return model; } }
		public byte Alive { get { return alive; } }
		public byte Unk2 { get { return unk2; } }
		public byte Realm { get { return realm; } }
		public byte Level { get { return level; } }
		public byte Stealthed { get { return stealthed; } }
		public byte Unk3 { get { return unk3; } }
		public string Name { get { return name; } }
		public string GuildName { get { return guildName; } }
		public string LastName { get { return lastName; } }
		public byte TrailingZero { get { return trailingZero; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} oid:0x{1:X4} zoneId:{2,-3} zoneLoc:({3,-5} {4,-5} {5,-5}) heading:0x{6:X4} model:0x{7:X4} alive:{8} realm:{9} level:{10,-2} stealthed:{11} unk1:{12} unk2:{13} unk3:{14} name:\"{15}\" guild:\"{16}\" lastName:\"{17}\" trailingZero:{18}",
				sessionId, oid, zoneId, zoneX, zoneY, zoneZ, heading, model, alive, realm, level, stealthed, unk1, unk2, unk3, name, guildName, lastName, trailingZero);

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
			zoneX = ReadShort();
			zoneY = ReadShort();
			zoneId = ReadByte();
			unk1 = ReadByte();
			zoneZ = ReadShort();
			heading = ReadShort();
			model = ReadShort();
			alive = ReadByte();
			unk2 = ReadByte();
			realm = ReadByte();
			level = ReadByte();
			stealthed = ReadByte();
			unk3 = ReadByte();
			name = ReadPascalString();
			guildName = ReadPascalString();
			lastName = ReadPascalString();
			trailingZero = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD4_PlayerCreate(int capacity) : base(capacity)
		{
		}
	}
}