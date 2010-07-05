using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF0, 204, ePacketDirection.ServerToClient, "Char create reply v204")]
	public class StoC_0xF0_CharCreateReply_204 : StoC_0xF0_CharCreateReply
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
		public StoC_0xF0_CharCreateReply_204(int capacity) : base(capacity)
		{
		}
	}
}