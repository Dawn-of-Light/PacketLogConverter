namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFC, 177, ePacketDirection.ClientToServer, "Character overview request v177")]
	public class CtoS_0xFC_CharacterOverviewRequest_177 : CtoS_0xFC_CharacterOverviewRequest
	{

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientAccountName = ReadString(28);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFC_CharacterOverviewRequest_177(int capacity) : base(capacity)
		{
		}
	}
}
