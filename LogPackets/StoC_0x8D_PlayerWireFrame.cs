namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x8D, -1, ePacketDirection.ServerToClient, "Player model change (wireframe)")]
	public class StoC_0x8D_PlayerWireframe: Packet, IOidPacket
	{
		protected ushort oid;
		protected byte flag;
		protected byte unk1;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public byte Flag { get { return flag; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("oid:0x{0:X4} flag:0x{1:X2} unk1:0x{2:X2}", oid, flag, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadShort();
			flag = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x8D_PlayerWireframe(int capacity) : base(capacity)
		{
		}
	}
}