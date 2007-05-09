namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFC, 177, ePacketDirection.ClientToServer, "Character overview request v177")]
	public class CtoS_0xFC_CharacterOverviewRequest_177 : CtoS_0xFC_CharacterOverviewRequest
	{

		protected uint unk2;

		public override string GetPacketDataString(bool flagsDescription)
		{
			return (flagsDescription ? string.Format("unk1:0x{0:X8} unk2:0x{1:X8} ", unk1, unk2) : "") + "clientAccountName:\"" + clientAccountName + '"';
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientAccountName = ReadString(20);
			unk1 = ReadIntLowEndian();
			unk2 = ReadIntLowEndian();
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
