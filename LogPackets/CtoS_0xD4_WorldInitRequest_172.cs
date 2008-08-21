using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD4, 172, ePacketDirection.ClientToServer, "World init request v172")]
	public class CtoS_0xD4_WorldInitRequest_172 : CtoS_0xD4_WorldInitRequest
	{

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadInt();             // 0x00
			unk2 = ReadInt();             // 0x04
			regionId = (short)ReadShort();// 0x08
			unk3 = ReadInt();             // 0x0A
			model = ReadShort();          // 0x0E
			unk4 = ReadByte();            // 0x10
			slot = ReadByte();            // 0x11
			unk5 = ReadShortLowEndian();  // 0x12
			unk6 = ReadShortLowEndian();  // 0x14
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