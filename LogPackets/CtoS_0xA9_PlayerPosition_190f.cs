using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 190.2f, ePacketDirection.ClientToServer, "Player position update v190f")]
	public class CtoS_0xA9_PlayerPosition_190f : CtoS_0xA9_PlayerPosition_190c
	{
		protected byte unk190_1;
		protected byte unk190_2;

		#region public access properties

		public byte Unk190_1 { get { return unk190_1; } }
		public byte Unk190_2 { get { return unk190_2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			int zSpeed = speed & 0xFFF;
			if ((speed & 0x1000) == 0x1000)
				zSpeed *= -1;
			text.Write("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} heading:0x{3:X4}(0x{12:X1}) currentZone({4,-3}): ({5,-6} {6,-6} {7,-5}) flyFlags:0x{8:X2} speedZ:{9,-5} flags:0x{10:X2} health:{11,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0xFFF ^ speed) >> 8, zSpeed, flag, health & 0x7F, (heading & 0xFFF ^ heading) >> 13);
			text.Write(" unk2:0x{0:X4}", unk2);
			text.Write(" unk190_1:0x{0:X2} unk190_2:0x{1:X2}", unk190_1, unk190_2);
			if (flagsDescription)
			{
				AddDescription(text);
			}
		}
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
//			Skip(32); // Unknown (i think it just rest buffer from recieved class name)
// 0x28 ?
			for (int i = 0; i < 4; i++)
				u_unk1[i] = ReadInt();
			for (int i = 0; i < 4; i++)
				u_unk2[i] = ReadFloatLowEndian();
			unk190_1 = ReadByte();      // 0x34
			unk190_2 = ReadByte();      // 0x35 // 0xFC
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA9_PlayerPosition_190f(int capacity) : base(capacity)
		{
		}
	}
}