using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x90, 1127, ePacketDirection.ClientToServer, "Region change request 1127")]
	public class CtoS_0x90_RegionChangeRequest_1127 : CtoS_0x90_RegionChangeRequest
	{
		
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("jumpSpotId:{0,-3} unk1:0x{1:X4}", jumpSpotId, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			jumpSpotId = ReadShortLowEndian();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x90_RegionChangeRequest_1127(int capacity) : base(capacity)
		{
		}
	}
}