using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD4, 172, ePacketDirection.ClientToServer, "World init request_172")]
	public class CtoS_0xD4_WorldInitRequest_172 : CtoS_0xD4_WorldInitRequest
	{

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadInt();
			unk2 = ReadInt();
			regionId = (short)ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD4_WorldInitRequest_172(int capacity) : base(capacity)
		{
		}
	}
}