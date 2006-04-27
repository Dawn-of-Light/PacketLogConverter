namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2B, -1, ePacketDirection.ServerToClient, "Player world initialize response")]
	public class StoC_0x2B_PlayerWorldInitializeResponse: Packet
	{
		protected byte count;

		#region public access properties

		public byte Count { get { return count; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "mobs sended:" + count.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			count = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2B_PlayerWorldInitializeResponse(int capacity) : base(capacity)
		{
		}
	}
}