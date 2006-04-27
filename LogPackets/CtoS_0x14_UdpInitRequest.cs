using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x14, -1, ePacketDirection.ClientToServer, "UDP Init Request")]
	public class CtoS_0x14_UdpInitRequest : Packet
	{
		protected string clientIp;
		protected ushort port;

		#region public access properties

		public string ClientIP { get { return clientIp; } }
		public ushort Port { get { return port; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("client IP:\"{0}\":{1}", clientIp, port);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			clientIp = ReadString(22);
			port = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x14_UdpInitRequest(int capacity) : base(capacity)
		{
		}
	}
}