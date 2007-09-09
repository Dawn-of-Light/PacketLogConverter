namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x22, 186, ePacketDirection.ServerToClient, "Version And Crypt Key v186")]
	public class StoC_0x22_VersionAndCryptKey_186 : StoC_0x22_VersionAndCryptKey
	{
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			encryption = ReadByte();
			isSI = ReadByte();
			majorVersion = ReadByte();
			minorVersion = ReadByte();
			build = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x22_VersionAndCryptKey_186(int capacity) : base(capacity)
		{
		}
	}
}