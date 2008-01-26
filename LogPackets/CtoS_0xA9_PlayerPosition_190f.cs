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
			text.Write("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} heading:0x{3:X4}(0x{12:X2}) currentZone({4,-3}): ({5,-6} {6,-6} {7,-5}) flyFlags:0x{8:X2} speedZ:{9,-5} flags:0x{10:X2} health:{11,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, zSpeed, flag, health & 0x7F, (heading & 0xFFF ^ heading) >> 8);
			text.Write(" unk2:0x{0:X4}", unk2);
			text.Write(" unk190_1:0x{0:X2} unk190_2:0x{1:X2}", unk190_1, unk190_2);
			if (flagsDescription)
			{
				for (int i = 0; i < 8; i++)
					text.Write(" {0:X8}", u_unk1[i]);
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
				if ((flag & 0x02) == 0x02)
					flags += ",Underwater";
				if ((flag & 0x04) == 0x04)
					flags += ",PetInView";
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
					flags+= ",Combat";
				if ((flag & 0x01) == 0x01)
					flags += ",UNKx01";
				if ((speed & 0x8000) == 0x8000)
					flags += ",FallDown";
				if ((speed & 0x4000) == 0x4000)
					flags += ",speed_UNK_0x4000";
				if ((speed & 0x2000) == 0x2000)
					flags += ",speed_UNK_0x2000";
				if (flags.Length > 0)
					text.Write(" (" + flags + ")");
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
			unk2 = ReadShort();
//			Skip(32); // Unknown (i think it just rest buffer from recieved class name)
			for (int i = 0; i < 8; i++)
				u_unk1[i] = ReadIntLowEndian();
			unk190_1 = ReadByte();
			unk190_2 = ReadByte();
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