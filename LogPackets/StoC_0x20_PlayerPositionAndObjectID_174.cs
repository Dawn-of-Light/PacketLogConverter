namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x20, 174, ePacketDirection.ServerToClient, "Set player position and OID v174")]
	public class StoC_0x20_PlayerPositionAndObjectID_174 : StoC_0x20_PlayerPositionAndObjectID_171
	{
		protected string server;

		#region public access properties

		public string Server { get { return server; } }

		#endregion

		public override string GetPacketDataString()
		{
			return string.Format("oid:0x{0:X4} x:{1,-6} y:{2,-6} z:{3,-5} heading:0x{4:X4} region:{5,-3} zoneXOffset:{6,-2} zoneYOffset:{7,-2} flags:0x{8:X2} server:\"{10}\" instance:0x{9:X2} unk2:0x{11:X2}",
				playerOid, x, y, z, heading, region, zoneXOffset, zoneYOffset, flags, unk1, server, unk2);
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
			zoneXOffset = ReadShort();
			zoneYOffset = ReadShort();
			region = ReadShort();
			server = ReadPascalString();
			unk2 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x20_PlayerPositionAndObjectID_174(int capacity) : base(capacity)
		{
		}
	}
}