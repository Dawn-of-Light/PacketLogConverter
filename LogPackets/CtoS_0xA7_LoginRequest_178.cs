namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 178, ePacketDirection.ClientToServer, "Login request v178")]
	public class CtoS_0xA7_LoginRequest_178 : CtoS_0xA7_LoginRequest_174
	{

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientType = ReadShort();
			clientVersionMajor = ReadByte();
			clientVersionMinor = ReadByte();
			clientVersionBuild = ReadByte();
			clientAccountPassword = ReadString(19);
			Skip(32);
			cryptKeyRequests = ReadByte();
			Skip(18);
			clientAccountName = ReadString(20);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_178(int capacity) : base(capacity)
		{
		}
	}
}