using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x20, 171, ePacketDirection.ServerToClient, "Set player position and OID v171")]
	public class StoC_0x20_PlayerPositionAndObjectID_171 : StoC_0x20_PlayerPositionAndObjectID
	{
		protected ushort region;
		protected ushort zoneXOffset;
		protected ushort zoneYOffset;
		protected ushort unk2;

		#region public access properties

		public ushort Region { get { return region; } }
		public ushort ZoneXOffset { get { return zoneXOffset; } }
		public ushort ZoneYOffset { get { return zoneYOffset; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} x:{1,-6} y:{2,-6} z:{3,-5} heading:0x{4:X4} region:{5,-3} zoneXOffset:{6,-2} zoneYOffset:{7,-2} flags:0x{8:X2} unk1:0x{9:X2} unk2:{10:X4}",
				playerOid, x, y, z, heading, region, zoneXOffset, zoneYOffset, flags, unk1, unk2);
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
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x20_PlayerPositionAndObjectID_171(int capacity) : base(capacity)
		{
		}
	}
}