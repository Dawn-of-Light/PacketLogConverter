using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x99, -1, ePacketDirection.ServerToClient, "Set door open state")]
	public class StoC_0x99_SetDoorOpenState : Packet
	{
		protected uint doorId;
		protected byte openState;
		protected byte doorSoundId;
		protected byte unk1;

		#region public access properties

		public uint DoorId { get { return doorId; } }
		public byte OpenState { get { return openState; } }
		public byte SoundId { get { return doorSoundId; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public enum eDoorSoundId: byte
		{
			metalHingedDoorOneSecond = 1,
			metalHingedDoorTwoSeconds = 2,
			metalHingedGateTwoSeconds = 3,
			metalHingedGateThreeSeconds = 4,
			metalPortcullisThreeSeconds = 5,
			metalHingedGate6ThreeSeconds = 6,
			thickVineRootsGateThreeSeconds = 7,
			templeOfKingsDoor = 8,
			minotaurDoorsInVolcanus = 9,
			playersDestroyBridgeInVolcanusEncounters = 10,
			playersDestroyBridge2InVolcanusEncounters = 11,
			openingSmallChest = 12,
			openingMediumChest = 13,
			openingLargeChest = 14,
			largeCrocodileStatueTurnsHeadToTheSideFourSeconds = 15,
			slowTurnRelicPlatformInTheKeepTowers = 16,
			labyrinthSecretDoor = 17
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("doorId:0x{0:X8} openState:{1} soundId:{2} unk1:0x{3:X2}", doorId, openState, doorSoundId, unk1);
			if (flagsDescription)
			{
				uint doorType = doorId / 100000000;
				if (doorType == 7)
				{
					uint keepId = (doorId - 700000000) / 100000;
					uint keepPiece = (doorId - 700000000 - keepId * 100000) / 10000;
					uint componentId = (doorId - 700000000 - keepId * 100000 - keepPiece * 10000) / 100;
					int doorIndex = (int)(doorId - 700000000 - keepId * 100000 - keepPiece * 10000 - componentId * 100);
					text.Write(" (keepID:{0} componentId:{1} doorIndex:{2})", keepId + keepPiece * 256, componentId, doorIndex);
				}
				else if(doorType == 9)
				{
					doorType = doorId / 10000000;
					uint doorIndex = doorId - doorType * 10000000;
					text.Write(" (doorType:{0} houseDoorId:{1})", doorType, doorIndex);
				}
				else
				{
					int zoneDoor = (int)(doorId / 1000000);
					int fixture = (int)(doorId - zoneDoor * 1000000);
					int fixturePiece = fixture;
					fixture /= 100;
					fixturePiece = fixturePiece - fixture * 100;
					text.Write(" (zone:{0} fixture:{1} fixturePeace:{2})", zoneDoor, fixture, fixturePiece);
				}
				if (doorSoundId > 0)
					text.Write(" soundDoor:{0}", (eDoorSoundId)doorSoundId);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			doorId = ReadInt();      // 0x00
			openState = ReadByte();  // 0x04
			doorSoundId = ReadByte();// 0x05
			unk1 = ReadByte();       // 0x06
			Skip(1); // unused
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x99_SetDoorOpenState(int capacity) : base(capacity)
		{
		}
	}
}