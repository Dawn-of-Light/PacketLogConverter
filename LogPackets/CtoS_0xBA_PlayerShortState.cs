using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBA, -1, ePacketDirection.ClientToServer, "Player Short State")]
	public class CtoS_0xBA_PlayerShortState : Packet, ISessionIdPacket
	{
		protected ushort sessionId;
		protected ushort heading;
		protected byte unk1;
		protected byte flags;
		protected byte unk2;
		protected byte rideSlot;
		protected byte health;
		protected byte state;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Heading { get { return heading; } }
		public byte Unk1 { get { return unk1; } }
		public byte Flags { get { return flags ; } }
		public byte Unk2 { get { return unk2; } }
		public byte RideSlot { get { return rideSlot; } }
		public byte Health { get { return health; } }
		public byte State { get { return state; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("sessionId:0x{0:X4} heading:0x{1:X4} flags:0x{2:X2} health:{3,3}% unk1:0x{5:X2} unk2:0x{6:X2} bSlot:0x{7:X2} state:{4}",
				sessionId, heading, flags, health & 0x7F, state, unk1, unk2, rideSlot);
			if (flagsDescription)
			{
				string status = state > 0 ? ((StoC_0xBA_PlayerShortState.PlrState)state).ToString() : "";
				if ((flags & 0x01) == 0x01)
					status += ",Wireframe";
				if ((flags & 0x02) == 0x02)
					status += ",Stealth";
				if ((flags & 0x04) == 0x04)
					status += ",PetInView";
				if ((flags & 0x08) == 0x08)
					status += ",GTinView";
				if ((flags & 0x10) == 0x10)
					status += ",CheckTargetInView";
				if ((flags & 0x20) == 0x20)
					status += ",TargetInView";
				if ((flags & 0x40) == 0x40)
					status += ",UNKx40";
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
			unk2 = ReadByte();
			rideSlot = ReadByte();
			health = ReadByte();
			state = ReadByte();
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			// Reinit only on for 190 version and subversion lower 190.1
			if (!log.IgnoreVersionChanges && log.Version >= 190 && log.Version < 190.1f)
			{
				if (Length == 12)
				{
					log.Version = 190.1f;
					log.SubversionReinit = true;
//					log.IgnoreVersionChanges = true;
				}
			}
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xBA_PlayerShortState(int capacity) : base(capacity)
		{
		}
	}
}