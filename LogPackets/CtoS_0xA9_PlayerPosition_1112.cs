using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 1112, ePacketDirection.ClientToServer, "Player position update v1112")]
	public class CtoS_0xA9_PlayerPosition_1112 : CtoS_0xA9_PlayerPosition_190f
	{
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();    // 0x00
			status = ReadShort();       // 0x02
			currentZoneZ = ReadShort(); // 0x04
			currentZoneX = ReadShort(); // 0x06
			currentZoneY = ReadShort(); // 0x08
			currentZoneId= ReadShort(); // 0x0A
			heading = ReadShort();      // 0x0C
			speed = ReadShort();        // 0x0E
			flag = ReadByte();          // 0x10
			health = ReadByte();        // 0x11
			unk2 = ReadShort();         // 0x12
			unk190_1 = ReadByte();      // 0x34
			unk190_2 = ReadByte();      // 0x35
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA9_PlayerPosition_1112(int capacity) : base(capacity)
		{
		}
	}
}