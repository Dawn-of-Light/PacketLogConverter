using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE1, -1, ePacketDirection.ServerToClient, "Object delete")]
	public class StoC_0xE1_ObjectDelete : Packet, IOidPacket
	{
		protected ushort oid;
		protected ushort unk1;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} unk1:0x{1:X4}", oid, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xE1_ObjectDelete(int capacity) : base(capacity)
		{
		}
	}
}