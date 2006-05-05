using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0D, -1, ePacketDirection.ClientToServer, "Remove house decoration")]
	public class CtoS_0x0D_RemoveHouseDecoration: Packet, IOidPacket
	{
		protected ushort index;
		protected ushort houseOid;
		protected byte place;
		protected byte unk1;
		protected ushort unk2;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Index { get { return index; } }
		public ushort Oid { get { return houseOid; } }
		public byte Place { get { return place; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public enum ePlaceType: byte
		{
			gardenDecoration = 1,
			wallDecoration = 2,
			floorDecoration = 3,
			exteriorDecoration = 4,
			hookPoints = 5
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("index:0x{0:X4} houseOid:0x{1:X4} place:{2}({5}) unk1:0x{3:X2} unk2:0x{4:X4}",
				index, houseOid, place, unk1, unk2, (ePlaceType)place);

			return str.ToString();
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