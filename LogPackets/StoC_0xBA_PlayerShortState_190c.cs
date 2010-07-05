using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			bool isRaided = (state == (byte)PlrState.Ride);
			text.Write("sessionId:0x{0:X4} {1}:0x{2:X4} flags:0x{3:X2} health:{4,3}% unk1:0x{6:X2} innerCounter:0x{7:X2} bSlot:0x{8:X2} state:{5} mana:{9,3}% endurance:{10,3}%",
				sessionId, isRaided ? "mountId" : "heading", heading, flags, health & 0x7F, state, unk1, innerCounter, rideSlot, manaPercent, endurancePercent);
			if (flagsDescription)
			{
				string status = state > 0 ? ((CtoS_0xA9_PlayerPosition.PlrState)state).ToString() : "";
				if ((flags & 0x01) == 0x01)
					status += ",Wireframe";
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
					text.Write(" ({0})", status);
			}
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
			innerCounter = ReadByte();
			rideSlot = ReadByte();
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