namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x04, -1, ePacketDirection.ServerToClient, "Character Jump")]
	public class StoC_0x04_CharacterJump : Packet, IObjectIdPacket, IHouseIdPacket
	{
		private uint x;
		private uint y;
		private ushort playerOid;
		private ushort z;
		private ushort heading;
		private ushort house;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { playerOid }; }
		}

		#region public access properties

		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public ushort PlayerOid { get { return playerOid; } }
		public ushort Z { get { return z; } }
		public ushort Heading { get { return heading; } }
		public ushort HouseId { get { return house; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			// SendPlayerJump()

			return string.Format("OID:0x{0:X4} x:{1,-6} y:{2,-6} z:{3,-5} heading:0x{4:X4} houseId:{5}", playerOid, x, y, z, heading, house);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			x = ReadInt();
			y = ReadInt();
			playerOid = ReadShort(); //This is the player's objectid not Sessionid!!!
			z = ReadShort();
			heading = ReadShort();
			house = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x04_CharacterJump(int capacity) : base(capacity)
		{
		}
	}
}