using System.IO;
using System.Text;
namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2F, -1, ePacketDirection.ServerToClient, "UDP init response")]
	public class StoC_0x2F_UdpInitResponse: Packet
	{
		protected string ip;
		protected uint unk1;
		protected ushort unk2;
		protected ushort port;

		#region public access properties

		public string IP { get { return ip; } }
		public uint Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Port { get { return port; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("regionIP:\"{0}\" PortFrom:{1}", ip, port);
			if (flagsDescription)
			{
				text.Write(" unk1:0x{0:X8} unk2:0x{1:X4}", unk1, unk2);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			ip = ReadString(16); // 0x00
			unk1 = ReadInt();    // 0x10
			unk2 = ReadShort();  // 0x14
			port = ReadShort();  // 0x16
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