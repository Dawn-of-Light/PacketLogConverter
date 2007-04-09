namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x20, -1, ePacketDirection.ServerToClient, "Set player position and OID")]
	public class StoC_0x20_PlayerPositionAndObjectID : Packet, IObjectIdPacket
	{
		protected ushort playerOid;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort heading;
		protected byte flags;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { playerOid }; }
		}

		#region public access properties

		public ushort PlayerOid { get { return playerOid; } }
		public ushort Z { get { return z; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public ushort Heading { get { return heading; } }
		public byte Flags { get { return flags; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("oid:0x{0:X4} x:{1,-6} y:{2,-6} z:{3,-5} heading:0x{4:X4} flags:0x{5:X2} unk1:0x{6:X2}", playerOid, x, y, z, heading, flags, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerOid = ReadShort(); //This is the player's objectid not Sessionid!!!
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			heading = ReadShort();
			flags = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x20_PlayerPositionAndObjectID(int capacity) : base(capacity)
		{
		}
	}
}