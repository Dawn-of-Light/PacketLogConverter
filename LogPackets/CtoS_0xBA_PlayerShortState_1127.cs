using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBA, 1127, ePacketDirection.ClientToServer, "Player Short State v1127")]
	public class CtoS_0xBA_PlayerShortState_1127 : CtoS_0xBA_PlayerShortState_190c
	{
		ushort currentTargetId;
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("sessionId:0x{0:X4} heading:{1:X4} flags:0x{2:X2} health:{3,3}% unk1:0x{5:X2} innerCounter:0x{6:X2} bSlot:0x{7:X2} state:{4} unk2:0x{8:X4} TargetId:0x{9:X4}",
				sessionId, heading, flags, health & 0x7F, state, unk1, innerCounter, rideSlot, unk2, currentTargetId);
			if (flagsDescription)
			{
				string status = state > 0 ? ((CtoS_0xA9_PlayerPosition.PlrState)state).ToString() : "";
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
			currentTargetId = ReadShort();
			heading = ReadShort();
			unk1 = ReadByte();
			flags = ReadByte();
			innerCounter = ReadByte();
			rideSlot = ReadByte(); 
			health = ReadByte();
			state = ReadByte(); 
			unk2 = ReadShort();
			heading = (ushort)(heading & 0xFFF);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xBA_PlayerShortState_1127(int capacity) : base(capacity)
		{
		}
	}
}