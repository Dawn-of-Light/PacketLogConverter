using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBA, -1, ePacketDirection.ServerToClient, "Player Short State")]
	public class StoC_0xBA_PlayerShortState : Packet, IOidPacket
	{
		protected ushort sessionId;
		protected ushort heading;
		protected byte unk1;
		protected byte flags;
		protected ushort unk2;
		protected byte health;
		protected byte state;

		public enum PlrState : byte
		{
			Stand = 0,
			Swim = 1,
			Jump_land = 2,
			Jump_rise = 3,
			Sit = 4,
			Dead = 5,
			Ride = 6,
			Climb = 7,
		}

		public int Oid1 { get { return sessionId; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Heading { get { return heading; } }
		public byte Unk1 { get { return unk1; } }
		public byte Flags { get { return flags ; } }
		public ushort Unk2 { get { return unk2; } }
		public byte Health { get { return health; } }
		public byte State { get { return state; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} heading:0x{1:X4} flags:0x{2:X2} health:{3,3}% unk1:0x{5:X2} unk2:0x{6:X4} state:{4}",
				sessionId, heading, flags, health & 0x7F, state, unk1, unk2);
			if (flagsDescription)
			{
				string status = state > 0 ? ((PlrState)state).ToString() : "";
				if ((flags & 0x01) == 0x01)
					status += ",UNKx01";
				if ((flags & 0x02) == 0x02)
					status += ",Stealth";
				if ((flags & 0x40) == 0x40)
					status += ",UNKx40";
				if ((flags & 0x04) == 0x04)
					status += ",PetInView";
				if ((flags & 0x08) == 0x08)
					status += ",GTinView";
				if ((flags & 0x10) == 0x10)
					status += ",CheckTargetInView";
				if ((flags & 0x20) == 0x20)
					status += ",TargetInView";
				if ((flags & 0x80) == 0x80)
					status += ",Torch";
				if ((health & 0x80) == 0x80)
					status += ",Combat";
				if (status.Length > 0)
					str.AppendFormat(" ({0})", status);
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
			heading = ReadShort();
			unk1 = ReadByte();
			flags = ReadByte();
			unk2 = ReadShort();
			health = ReadByte();
			state = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xBA_PlayerShortState(int capacity) : base(capacity)
		{
		}
	}
}