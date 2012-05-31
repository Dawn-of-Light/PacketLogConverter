using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 190.2f, ePacketDirection.ServerToClient, "Player position update v190f")]
	public class StoC_0xA9_PlayerPosition_190f : StoC_0xA9_PlayerPosition_190c
	{
		protected byte flagRP;// show player as (RP) after name, and gray color (RolePlay for Tintagel cluster ?)
		protected byte unk190_1;

		#region public access properties

		public byte RP { get { return flagRP; } }
		public byte Unk190_1 { get { return unk190_1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			bool isRaided = IsRaided == 1;
			int zSpeed = speed & 0xFFF;
			if ((speed & 0x1000) == 0x1000)
				zSpeed *= -1;
			text.Write("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} {3}:0x{4:X4}(0x{14:X1}) currentZone({5,-3}): ({6,-6} {7,-6} {8,-5}) flyFlags:0x{9:X2} {10}:{11,-5} flags:0x{12:X2} health:{13,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, isRaided ? "mountId" : "heading", isRaided ? heading : heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, (isRaided ? "bSlot " : "SpeedZ") , zSpeed, flag, health & 0x7F, isRaided ? 0 : (heading & 0xFFF ^ heading) >> 13);
			text.Write(" mana:{0,3}% endurance:{1,3}%", manaPercent, endurancePercent);
			text.Write(" RP:{0} unk190_1:0x{1:X2} className:{2}", flagRP, unk190_1, className);
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

			sessionId = ReadShort();      // 0x00
			status = ReadShort();         // 0x02
			currentZoneZ = ReadShort();   // 0x04
			currentZoneX = ReadShort();   // 0x06
			currentZoneY = ReadShort();   // 0x08
			currentZoneId= ReadShort();   // 0x0A
			heading = ReadShort();        // 0x0C
			speed = ReadShort();          // 0x0E
			flag = ReadByte();            // 0x10
			health = ReadByte();          // 0x11
			manaPercent = ReadByte();     // 0x12
			endurancePercent = ReadByte();// 0x13
			className = ReadString(32);   // 0x14
			flagRP = ReadByte();          // 0x34
			unk190_1 = ReadByte();        // 0x35
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xA9_PlayerPosition_190f(int capacity) : base(capacity)
		{
		}
	}
}