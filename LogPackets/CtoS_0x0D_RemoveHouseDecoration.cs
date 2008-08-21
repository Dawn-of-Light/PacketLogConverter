using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0D, -1, ePacketDirection.ClientToServer, "Remove house decoration")]
	public class CtoS_0x0D_RemoveHouseDecoration: Packet, IHouseIdPacket
	{
		protected ushort index;
		protected ushort houseOid;
		protected byte place;
		protected byte unk1; // unused
		protected ushort unk2; // unused

		#region public access properties

		public ushort Index { get { return index; } }
		public ushort HouseId { get { return houseOid; } }
		public byte Place { get { return place; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("index:0x{0:X4} houseOid:0x{1:X4} place:{2}", index, houseOid, place);
			if (flagsDescription)
				text.Write(" ({2}) unk1:0x{0:X2} unk2:0x{1:X4}", unk1, unk2, (CtoS_0x0C_HouseItemPlacementRequest.ePlaceType)place);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			index = ReadShort();
			houseOid = ReadShort();
			place = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x0D_RemoveHouseDecoration(int capacity) : base(capacity)
		{
		}
	}
}