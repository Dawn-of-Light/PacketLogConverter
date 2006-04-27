using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, -1, ePacketDirection.ClientToServer, "Player position update")]
	public class CtoS_0xA9_PlayerPosition : Packet, IOidPacket
	{
		protected ushort sessionId;
		protected ushort status;
		protected ushort currentZoneZ;
		protected ushort currentZoneX;
		protected ushort currentZoneY;
		protected ushort currentZoneId;
		protected byte unk1;
		protected ushort heading;
		protected ushort speed;
		protected byte flag;
		protected byte health;

		public int Oid1 { get { return sessionId; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Status { get { return status; } }
		public ushort CurrentZoneZ { get { return currentZoneZ; } }
		public ushort CurrentZoneX { get { return currentZoneX; } }
		public ushort CurrentZoneY { get { return currentZoneY; } }
		public ushort CurrentZoneId { get { return currentZoneId; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Heading { get { return heading; } }
		public ushort Speed { get { return speed; } }
		public byte Flag { get { return flag; } }
		public byte Health { get { return health; } }

		#endregion

		public enum PlrState : byte
		{
			Stand = 0,
			Swim = 1,
			Jump = 2,
			debugFly= 3,
			Sit = 4,
			Dead = 5,
			Ride = 6,
			Climb = 7,
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} heading:0x{3:X4}(0x{12:X2}) currentZone({4,-3}): ({5,-6} {6,-6} {7,-5}) flyFlags:0x{8:X2} speedZ:{9,-4} flags:0x{10:X2} health:{11,3}%",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, speed & 0x7FF, flag, health & 0x7F, (heading & 0xFFF ^ heading) >> 8);
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
					flags += "StrafeMove";
				if ((flag & 0x02) == 0x02)
					flags += ",Diving";
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
				if (flags.Length > 0)
					str.Append(" ("+flags+")");
			}
			return str.ToString();
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
			currentZoneId= ReadByte();
			unk1 = ReadByte();
			heading = ReadShort();
			speed = ReadShort();
			flag = ReadByte();
			health = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA9_PlayerPosition(int capacity) : base(capacity)
		{
		}
	}
}