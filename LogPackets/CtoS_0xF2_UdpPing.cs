using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF2, -1, ePacketDirection.ClientToServer, "UDP ping")] // sended ~every 25 sec
	public class CtoS_0xF2_UdpPing : Packet
	{
		protected string clientIp;
		protected ushort port;

		#region public access properties

		public string ClientIP { get { return clientIp; } }
		public ushort Port { get { return port; } }

		#endregion

		public override string GetPacketDataString()
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
		public CtoS_0xF2_UdpPing(int capacity) : base(capacity)
		{
		}
	}
}