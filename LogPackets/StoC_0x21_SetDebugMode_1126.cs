using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x21, 1126, ePacketDirection.ServerToClient, "Set debug mode 1126")]
	public class StoC_0x21_SetDebugMode_1126 : StoC_0x21_SetDebugMode
	{

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("state:{0}", state);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			state = ReadByte();			
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x21_SetDebugMode_1126(int capacity) : base(capacity)
		{
		}
	}
}