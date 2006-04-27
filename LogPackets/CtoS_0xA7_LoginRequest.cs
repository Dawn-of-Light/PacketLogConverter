namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, -1, ePacketDirection.ClientToServer, "Login request")]
	public class CtoS_0xA7_LoginRequest : Packet
	{
		protected ushort clientType;
		protected byte clientVersionMajor;
		protected byte clientVersionMinor;
		protected byte clientVersionBuild;
		protected string clientAccountPassword;
		protected string clientAccountName;

		#region public access properties

		public ushort ClientType { get { return clientType; } }
		public byte ClientVersionMajor { get { return clientVersionMajor; } }
		public byte ClientVersionMinor { get { return clientVersionMinor; } }
		public byte ClientVersionBuild { get { return clientVersionBuild; } }
		public string ClientAccountPassword { get { return clientAccountPassword; } }
		public string ClientAccountName { get { return clientAccountName; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("version:{0}.{1}.{2} accountName:\"{3}\" accountPassword:\"{4}\" and 50 bytes ignored!",
				clientVersionMajor, clientVersionMinor, clientVersionBuild, clientAccountName, clientAccountPassword);
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			Position = 2;
			int major = ReadByte();
			int minor = ReadByte();
			int build = ReadByte();
			int version = major*100 + minor*10 + build;
			log.Version = version;
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
			clientAccountPassword = ReadString(20);
			Skip(50);
			clientAccountName = ReadString(20);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest(int capacity) : base(capacity)
		{
		}
	}
}