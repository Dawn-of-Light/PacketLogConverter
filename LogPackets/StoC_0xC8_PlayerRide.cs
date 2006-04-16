using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC8, -1, ePacketDirection.ServerToClient, "Player ride")]
	public class StoC_0xC8_PlayerRide: Packet, IOidPacket
	{
		protected ushort riderOid;
		protected ushort mountOid;
		protected byte flag;
		protected byte unk1;
		protected ushort unk2;

		public int Oid1 { get { return riderOid; } }
		public int Oid2 { get { return mountOid; } }

		#region public access properties

		public ushort RiderOid { get { return riderOid; } }
		public ushort MountOid { get { return mountOid; } }
		public byte Flag { get { return flag; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("riderOid:0x{0:X4} mountOid:0x{1:X4} flag:{2} unk1:0x{3:X2} unk2:0x{4:X4}",
				riderOid, mountOid, flag, unk1, unk2);

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
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC8_PlayerRide(int capacity) : base(capacity)
		{
		}
	}
}