using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2A, -1, ePacketDirection.ServerToClient, "Login granted")]
	public class StoC_0x2A_LoginGranted : Packet
	{
		protected byte isSI;
		protected byte serverVersionMajor;
		protected byte serverVersionMinor;
		protected byte serverVersionBuild;
		protected string clientAccountName;
		protected string serverName;
		protected byte serverId;
		protected byte colorHandling;
		protected byte trial; // if = 1 show trial info on exit

		#region public access properties

		public byte IsSi { get { return isSI; } }
		public byte ServerVersionMajor { get { return serverVersionMajor; } }
		public byte ServerVersionMinor { get { return serverVersionMinor; } }
		public byte ServerVersionBuild { get { return serverVersionBuild; } }
		public string ClientAccountName { get { return clientAccountName; } }
		public string ServerName { get { return serverName; } }
		public byte ServerId { get { return serverId; } }
		public byte ColorHandling { get { return colorHandling; } }
		public byte Trial { get { return trial; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("isSI:0x{0:X4} serverVersion:{1}.{2}.{3} clientAccountName:\"{4}\" serverName:\"{5}\" serverId:0x{6:X2} colorHandling:{7} trial:{8}",
				isSI, serverVersionMajor, serverVersionMinor, serverVersionBuild, clientAccountName, serverName, serverId, colorHandling, trial);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			isSI = ReadByte();
			serverVersionMajor = ReadByte();
			serverVersionMinor = ReadByte();
			serverVersionBuild = ReadByte();
			clientAccountName = ReadPascalString();
			serverName = ReadPascalString();
			serverId = ReadByte();
			colorHandling = ReadByte();
			trial = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2A_LoginGranted(int capacity) : base(capacity)
		{
		}
	}
}