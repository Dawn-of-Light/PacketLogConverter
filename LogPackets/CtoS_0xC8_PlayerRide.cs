using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC8, -1, ePacketDirection.ClientToServer, "Player ride")]
	public class CtoS_0xC8_PlayerRide: Packet, IOidPacket
	{
		protected ushort riderOid;
		protected ushort mountOid;
		protected byte flag;
		protected byte slot;
		protected ushort unk1;

		public int Oid1 { get { return riderOid; } }
		public int Oid2 { get { return mountOid; } }

		#region public access properties

		public ushort RiderOid { get { return riderOid; } }
		public ushort MountOid { get { return mountOid; } }
		public byte Flag { get { return flag; } }
		public byte Slot { get { return slot; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("riderOid:0x{0:X4} mountOid:0x{1:X4} flag:{2} slot:0x{3:X2} unk1:0x{4:X4}",
				riderOid, mountOid, flag, slot, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			riderOid= ReadShort();
			mountOid= ReadShort();
			flag = ReadByte();
			slot = ReadByte();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xC8_PlayerRide(int capacity) : base(capacity)
		{
		}
	}
}