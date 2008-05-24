using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x23, -1, ePacketDirection.ServerToClient, "Client Denied")]
	public class StoC_0x23_ClientDenied: StoC_0x2C_LoginDenied
	{
		public StoC_0x23_ClientDenied(int capacity) : base(capacity)
		{
		}
	}
}