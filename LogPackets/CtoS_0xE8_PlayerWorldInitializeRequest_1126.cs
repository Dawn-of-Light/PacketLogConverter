using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE8, 1126, ePacketDirection.ClientToServer, "Player Wolrd initialize request 1126")]
	public class CtoS_0xE8_PlayerWorldInializeRequest_1126 : CtoS_0xE8_PlayerWorldInializeRequest
	{
		byte code;
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("code: 0x{0:2} ", code);
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
		public CtoS_0xE8_PlayerWorldInializeRequest_1126(int capacity) : base(capacity)
		{
		}
	}
}