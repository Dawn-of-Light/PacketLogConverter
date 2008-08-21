using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x20, 174, ePacketDirection.ServerToClient, "Set player position and OID v174")]
	public class StoC_0x20_PlayerPositionAndObjectID_174 : StoC_0x20_PlayerPositionAndObjectID_171
	{
		protected string server;

		#region public access properties

		public string Server { get { return server; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} x:{1,-6} y:{2,-6} z:{3,-5} heading:0x{4:X4} region:{5,-3} zoneXOffset:{6,-2} zoneYOffset:{7,-2} flags:0x{8:X2} server:\"{10}\" instance:0x{9:X2} unk2:0x{11:X2}",
				playerOid, x, y, z, heading, region, zoneXOffset, zoneYOffset, flags, unk1, server, unk2);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerOid = ReadShort();    // 0x00
			z = ReadShort();            // 0x02
			x = ReadInt();              // 0x04
			y = ReadInt();              // 0x08
			heading = ReadShort();      // 0x0C
			flags = ReadByte();         // 0x0E
			unk1 = ReadByte();          // 0x0F
			zoneXOffset = ReadShort();  // 0x10
			zoneYOffset = ReadShort();  // 0x12
			region = ReadShort();       // 0x14
			server = ReadPascalString();// 0x16
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