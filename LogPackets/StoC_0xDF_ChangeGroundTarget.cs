namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDF, -1, ePacketDirection.ServerToClient, "Assist ground target")]
	public class StoC_0xDF_ChangeGroundTarget : Packet
	{
		protected uint x;
		protected uint y;
		protected uint z;

		#region public access properties

		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public uint Z { get { return z; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("x:{0:X8} y:{1:X8} z:0x{2:X8}", x, y, z);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDF_ChangeGroundTarget(int capacity) : base(capacity)
		{
		}
	}
}