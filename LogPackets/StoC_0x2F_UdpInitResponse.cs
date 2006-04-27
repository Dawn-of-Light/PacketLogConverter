using System.Text;
namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2F, -1, ePacketDirection.ServerToClient, "UDP init response")]
	public class StoC_0x2F_UdpInitResponse: Packet
	{
		protected string ip;
		protected ushort unk1;

		#region public access properties

		public string IP { get { return ip; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("regionIP:\"{0}\" PortFrom:{1}", ip, unk1);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			ip = ReadString(22);
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2F_UdpInitResponse(int capacity) : base(capacity)
		{
		}
	}
}