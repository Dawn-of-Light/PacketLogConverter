namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x28, -1, ePacketDirection.ServerToClient, "Set session ID")]
	public class StoC_0x28_SetSessionId : Packet
	{
		protected ushort sessionId;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "sessionId:0x" + sessionId.ToString("X4");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			sessionId = ReadShortLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x28_SetSessionId(int capacity) : base(capacity)
		{
		}
	}
}