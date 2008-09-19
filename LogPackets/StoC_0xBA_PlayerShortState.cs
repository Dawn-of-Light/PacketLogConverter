using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBA, -1, ePacketDirection.ServerToClient, "Player Short State")]
	public class StoC_0xBA_PlayerShortState : Packet, IObjectIdPacket, ISessionIdPacket
	{
		protected ushort sessionId;
		protected ushort heading;
		protected byte unk1;
		protected byte flags;
		protected byte innerCounter;
		protected byte rideSlot;
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

		public ushort[] ObjectIds
		{
			get
			{
				if (state == (byte)PlrState.Ride)
				{
					return new ushort[] { heading };
				}
				return new ushort[] {};
			}
		}
		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Heading { get { return heading; } }
		public byte Unk1 { get { return unk1; } }
		public byte Flags { get { return flags ; } }
		public byte InnerCounter { get { return innerCounter; } }
		public byte RideSlot { get { return rideSlot; } }
		public byte Health { get { return health; } }
		public byte State { get { return state; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			bool isRaided = (state == (byte)PlrState.Ride);
			text.Write("sessionId:0x{0:X4} {1}:0x{2:X4} flags:0x{3:X2} health:{4,3}% unk1:0x{6:X2} innerCounter:0x{7:X2} bSlot:0x{8:X2} state:{5}",
				sessionId, isRaided ? "mountId" : "heading", heading, flags, health & 0x7F, state, unk1, innerCounter, rideSlot);
			if (flagsDescription)
			{
				string status = state > 0 ? ((PlrState)state).ToString() : "";
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
		public StoC_0xBA_PlayerShortState(int capacity) : base(capacity)
		{
		}
	}
}