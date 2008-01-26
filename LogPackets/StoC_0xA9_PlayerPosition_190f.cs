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
			text.Write("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} {3}:0x{4:X4}(0x{14:X2}) currentZone({5,-3}): ({6,-6} {7,-6} {8,-5}) flyFlags:0x{9:X2} {10}:{11,-5} flags:0x{12:X2} health:{13,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, isRaided ? "mountId" : "heading", isRaided ? heading : heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, (isRaided ? "bSlot " : "SpeedZ") , zSpeed, flag, health & 0x7F, isRaided ? 0 : (heading & 0xFFF ^ heading) >> 8);
			text.Write(" mana:{0,3}% endurance:{1,3}%", manaPercent, endurancePercent);
			text.Write(" RP:{0} unk190_1:0x{1:X2} className:{2}", flagRP, unk190_1, className);
			if (flagsDescription)
			{
				byte plrState = (byte)((status >> 10) & 7);
				string flags = plrState > 0 ? ((PlrState)plrState).ToString() : "";
				if ((status & 0x200) == 0x200)
					flags += ",Backward";
				if ((status & 0x8000) == 0x8000)
					flags += ",StrafeRight";
				if ((status & 0x4000) == 0x4000)
					flags += ",StrafeLeft";
				if ((status & 0x2000) == 0x2000)
					flags += "Move";
				if ((flag & 0x01) == 0x01)
					flags += ",Wireframe";
				if ((flag & 0x02) == 0x02)
					flags += ",Stealth";
				if ((flag & 0x04) == 0x04)
					flags += ",Underwater";
				if ((flag & 0x08) == 0x08)
					flags += ",GT";
				if ((flag & 0x10) == 0x10)
					flags += ",CheckTargetInView";
				if ((flag & 0x20) == 0x20)
					flags += ",TargetInView";
				if ((flag & 0x40) == 0x40)
					flags += ",MoveTo";
				if ((flag & 0x80) == 0x80)
					flags += ",Torch";
				if ((health & 0x80) == 0x80)
					flags += ",Combat";
				if ((speed & 0x8000) == 0x8000)
					flags += ",FallDown";
				if ((speed & 0x4000) == 0x4000)
					flags += ",speed_UNK_0x4000";
				if ((speed & 0x2000) == 0x2000)
					flags += ",speed_UNK_0x2000";
				if (flags.Length > 0)
					text.Write(" ("+flags+")");
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			status = ReadShort();
			currentZoneZ = ReadShort();
			currentZoneX = ReadShort();
			currentZoneY = ReadShort();
			currentZoneId= ReadShort();
			heading = ReadShort();
			speed = ReadShort();
			flag = ReadByte();
			health = ReadByte();
			manaPercent = ReadByte();
			endurancePercent = ReadByte();
			className = ReadString(32);
			flagRP = ReadByte();
			unk190_1 = ReadByte();
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