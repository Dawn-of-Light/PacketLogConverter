using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF0, 1104, ePacketDirection.ServerToClient, "Char create reply v1104")]
	public class StoC_0xF0_CharCreateReply_1104 : StoC_0xF0_CharCreateReply
	{
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			accountName = ReadString(24);
			Skip(4);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF0_CharCreateReply_1104(int capacity) : base(capacity)
		{
		}
	}
}