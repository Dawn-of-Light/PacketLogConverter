using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAF, 1127, ePacketDirection.ClientToServer, "cmd")]
	public class CtoS_0xAF_PlayerCommand_1127 : CtoS_0xAF_PlayerCommand
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{			
			text.Write(" \"{0}\"", command);
			if (flagsDescription && (flag >= 0))
				text.Write(" flag:0x{0:X2}", flag);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			flag = -1;

			Skip(1);
			
			command = ReadString();
			if (Position < Length)
				flag = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xAF_PlayerCommand_1127(int capacity) : base(capacity)
		{
		}
	}
}