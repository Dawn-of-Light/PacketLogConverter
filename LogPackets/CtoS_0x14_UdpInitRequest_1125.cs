using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x14, 1125, ePacketDirection.ClientToServer, "UDP Init Request 1125")]
	public class CtoS_0x14_UdpInitRequest_1125 : CtoS_0x14_UdpInitRequest
	{
		byte code;
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("code {0}", code);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			code = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x14_UdpInitRequest_1125(int capacity) : base(capacity)
		{
		}
	}
}