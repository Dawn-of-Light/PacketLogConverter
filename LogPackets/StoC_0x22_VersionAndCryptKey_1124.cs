using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x22, 1124, ePacketDirection.ServerToClient, "Version And Crypt Key v1124")]
	public class StoC_0x22_VersionAndCryptKey_1124 : StoC_0x22_VersionAndCryptKey_1111
	{
		string clientVersion;
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("Client version: " + clientVersion);
		}
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			clientVersion = ReadPascalStringShortLowEndian();
			Skip(2);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x22_VersionAndCryptKey_1124(int capacity) : base(capacity)
		{
		}
	}
}