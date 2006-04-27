namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 174, ePacketDirection.ClientToServer, "Login request v174")]
	public class CtoS_0xA7_LoginRequest_174 : CtoS_0xA7_LoginRequest
	{
		protected byte cryptKeyRequests;

		#region public access properties

		public byte CryptKeyRequests { get { return cryptKeyRequests; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("version:{0}.{1}.{2} accountName:\"{3}\" accountPassword:\"{4}\" cryptKeyRequests:0x{5:X2}",
				clientVersionMajor, clientVersionMinor, clientVersionBuild, clientAccountName, clientAccountPassword, cryptKeyRequests);
		}

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
			Skip(28);
			cryptKeyRequests = ReadByte();
			Skip(22);
			clientAccountName = ReadString(20);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_174(int capacity) : base(capacity)
		{
		}
	}
}