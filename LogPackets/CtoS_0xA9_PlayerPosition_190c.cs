using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 190.1f, ePacketDirection.ClientToServer, "Player position update v190c")]
	public class CtoS_0xA9_PlayerPosition_190c : CtoS_0xA9_PlayerPosition_172
	{
		protected ushort unk2;
		protected uint[] u_unk1 = new uint[8];

		#region public access properties

		public ushort Unk2 { get { return unk2; } }
		public uint[] U_Unk1 { get { return u_unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			int zSpeed = speed & 0xFFF;
			if ((speed & 0x1000) == 0x1000)
				zSpeed *= -1;
			text.Write("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} heading:0x{3:X4}(0x{12:X1}) currentZone({4,-3}): ({5,-6} {6,-6} {7,-5}) flyFlags:0x{8:X2} speedZ:{9,-5} flags:0x{10:X2} health:{11,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0xFFF ^ speed) >> 8, zSpeed, flag, health & 0x7F, (heading & 0xFFF ^ heading) >> 13);
			text.Write(" unk2:0x{0:X4}", unk2);
			if (flagsDescription)
			{
				for (int i = 0; i < 8; i++)
					text.Write(" {0:X8}", u_unk1[i]);
				byte plrState = (byte)((status >> 10) & 7);
				string flags = plrState > 0 ? ((PlrState)plrState).ToString() : "";
				if (((heading >> 12) & 1) == 1)
					flags += ",OnGround";
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
			for (int i = 0; i < 8; i++)
				u_unk1[i] = ReadIntLowEndian();
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			// Reinit only on for 190 version and subversion lower 190.2
			if (!log.IgnoreVersionChanges && log.Version >= 190 && log.Version < 190.2f)
			{
				if (Length == 54)
				{
					log.Version = 190.2f;
					log.SubversionReinit = true;
//					log.IgnoreVersionChanges = true;
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA9_PlayerPosition_190c(int capacity) : base(capacity)
		{
		}
	}
}