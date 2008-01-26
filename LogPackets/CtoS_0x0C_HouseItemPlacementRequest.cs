using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0C, -1, ePacketDirection.ClientToServer, "House item placement request")]
	public class CtoS_0x0C_HouseItemPlacementRequest: Packet, IHouseIdPacket
	{
		protected ushort slot;
		protected ushort houseOid;
		protected ushort surface;
		protected byte place;
		protected byte rotation;
		protected short x;
		protected short y;

		#region public access properties

		public ushort Slot { get { return slot; } }
		public ushort HouseId { get { return houseOid; } }
		public ushort Surface { get { return surface; } }
		public byte Place { get { return place; } }
		public byte Rotation { get { return rotation; } }
		public short X { get { return x; } }
		public short Y { get { return y; } }

		#endregion

		public enum ePlaceType: byte
		{
			gardenDecoration = 1,
			wallDecoration = 2,
			floorDecoration = 3,
			exteriorDecoration = 4,
			hookPoints = 5
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("slot:{0,-3} houseOid:0x{1:X4} surface:{2,-3} place:{3}({4}) rotation:{5} (x:{6} y:{7})",
				slot, houseOid, surface, place, (ePlaceType)place, rotation, x, y);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			slot = ReadShort();
			houseOid = ReadShort();
			surface = ReadShort();
			place = ReadByte();
			rotation = ReadByte();
			x = (short)ReadShort();
			y = (short)ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x0C_HouseItemPlacementRequest(int capacity) : base(capacity)
		{
		}
	}
}