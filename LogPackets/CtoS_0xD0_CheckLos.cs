using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD0, -1, ePacketDirection.ClientToServer, "Check LOS")]
	public class CtoS_0xD0_CheckLos: Packet, IOidPacket
	{
		protected ushort oid;
		protected ushort targetOid;
		protected ushort code;
		protected ushort unk1;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return targetOid; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort TargetOid { get { return targetOid; } }
		public ushort Answer { get { return code; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("oid:0x{0:X4} targetOid:0x{1:X4} code:0x{2:X4} unk1:0x{3:X4}",
				oid, targetOid, code, unk1);

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
			code = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD0_CheckLos(int capacity) : base(capacity)
		{
		}
	}
}