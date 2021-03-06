using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xCC, 1104, ePacketDirection.ServerToClient, "Name dublicate check response v1104")]
	public class StoC_0xCC_NameDublicateCheckResponse_1104: StoC_0xCC_NameDublicateCheckResponse
	{
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			charName = ReadString(30);
			loginName = ReadString(24);
			code = ReadByte();
			Skip(3);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xCC_NameDublicateCheckResponse_1104(int capacity) : base(capacity)
		{
		}
	}
}