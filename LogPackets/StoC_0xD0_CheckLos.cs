using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD0, -1, ePacketDirection.ServerToClient, "Check LOS")]
	public class StoC_0xD0_CheckLos: Packet, IOidPacket
	{
		protected ushort oid;
		protected ushort targetOid;
		protected ushort unk1;
		protected ushort unk2;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return targetOid; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort TargetOid { get { return targetOid; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("oid:0x{0:X4} targetOid:0x{1:X4} unk1:0x{2:X4} unk2:0x{3:X4}",
				oid, targetOid, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			targetOid = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD0_CheckLos(int capacity) : base(capacity)
		{
		}
	}
}