using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAC, 1126, ePacketDirection.ClientToServer, "Reset Character 1126")]
	public class CtoS_0xAC_PlayerResetCharacter_1126 : CtoS_0xAC_PlayerResetCharacter
	{ 
		byte code;

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("code:0x{0:X2} ", code);
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
		public CtoS_0xAC_PlayerResetCharacter_1126(int capacity) : base(capacity)
		{
		}
	}
}