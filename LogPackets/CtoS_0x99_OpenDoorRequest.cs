using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x99, -1, ePacketDirection.ClientToServer, "Open door request  ")]
	public class CtoS_0x99_OpenDoorRequest : Packet
	{
		protected uint doorId;
		protected byte openState;
		protected byte unk1;
		protected ushort unk2;

		#region public access properties

		public uint DoorId { get { return doorId; } }
		public byte OpenState { get { return openState; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("doorId:0x{0:X8} openState:{1} unk1:0x{2:X2} unk2:0x{3:X4}", doorId, openState, unk1, unk2);
			if (flagsDescription)
			{
				uint doorType = doorId / 100000000;
				if (doorType == 7)
				{
					uint keepId = (doorId - 700000000) / 100000;
					uint keepPiece = (doorId - 700000000 - keepId * 100000) / 10000;
					uint componentId = (doorId - 700000000 - keepId * 100000 - keepPiece * 10000) / 100;
					int doorIndex = (int)(doorId - 700000000 - keepId * 100000 - keepPiece * 10000 - componentId * 100);
					str.AppendFormat(" (keepID:{0} componentId:{1} doorIndex:{2})", keepId + keepPiece * 256, componentId, doorIndex);
				}
				else if(doorType == 9)
				{
					doorType = doorId / 10000000;
					uint doorIndex = doorId - doorType * 10000000;
					str.AppendFormat(" (doorType:{0} houseDoorId:{1})", doorType, doorIndex);
				}
				else
				{
					int zoneDoor = (int)(doorId / 1000000);
					int fixture = (int)(doorId - zoneDoor * 1000000);
					int fixturePiece = fixture;
					fixture /= 100;
					fixturePiece = fixturePiece - fixture * 100;
					str.AppendFormat(" (zone:{0} fixture:{1} fixturePeace:{2})", zoneDoor, fixture, fixturePiece);
				}
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			doorId = ReadInt();
			openState = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x99_OpenDoorRequest(int capacity) : base(capacity)
		{
		}
	}
}