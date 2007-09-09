using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBA, 190.1f, ePacketDirection.ServerToClient, "Player Short State v190c")]
	public class StoC_0xBA_PlayerShortState_190c : StoC_0xBA_PlayerShortState
	{
		protected byte manaPercent;
		protected byte endurancePercent;

		#region public access properties

		public byte ManaPercent { get { return manaPercent; } }
		public byte EndurancePercent { get { return endurancePercent; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} heading:0x{1:X4} flags:0x{2:X2} health:{3,3}% unk1:0x{5:X2} unk2:0x{6:X4} state:{4} mana:{7,3}% endurance:{8,3}%",
				sessionId, heading, flags, health & 0x7F, state, unk1, unk2, manaPercent, endurancePercent);
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
			manaPercent = ReadByte();
			endurancePercent = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xBA_PlayerShortState_190c(int capacity) : base(capacity)
		{
		}
	}
}