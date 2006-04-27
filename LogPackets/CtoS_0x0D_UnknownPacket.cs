using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0D, -1, ePacketDirection.ClientToServer, "Modify house decoration ?")]
	public class CtoS_0x0D_UnknownPacket: Packet, IOidPacket
	{
		protected ushort position;
		protected ushort houseOid;
		protected byte itemType;
		protected byte unk1;
		protected ushort unk2;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Pos { get { return position; } }
		public ushort Oid { get { return houseOid; } }
		public byte ItemType { get { return itemType; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public enum eItemType: byte
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
			str.AppendFormat("position:0x{0:X4} houseOid:0x{1:X4} itemType:{2}({3}) unk1:0x{4:X2} unk2:0x{5:X4}",
				position, houseOid, itemType, (eItemType)itemType, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			position = ReadShort();
			houseOid = ReadShort();
			itemType = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x0D_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}