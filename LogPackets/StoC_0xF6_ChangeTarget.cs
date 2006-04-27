namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF6, -1, ePacketDirection.ServerToClient, "Change target")]
	public class StoC_0xF6_ChangeTarget : Packet, IOidPacket
	{
		protected ushort oid;
		protected byte type;
		protected byte unk1;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public byte Type { get { return type; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("oid:0x{0:X4} type:{1} unk1:0x{2:X2}", oid, type, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadShort();
			type = ReadByte();
			unk1= ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF6_ChangeTarget(int capacity) : base(capacity)
		{
		}
	}
}