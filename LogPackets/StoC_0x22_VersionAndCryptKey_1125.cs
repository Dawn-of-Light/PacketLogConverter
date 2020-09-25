using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x22, 1125, ePacketDirection.ServerToClient, "Version And Crypt Key v1125")]
	public class StoC_0x22_VersionAndCryptKey_1125 : StoC_0x22_VersionAndCryptKey_1124
	{
		string clientVersion;
		ushort buildId;
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("Client version: {0} Build Version: {2}", clientVersion, buildId);
		}
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			clientVersion = ReadPascalStringIntLowEndian();
			buildId = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x22_VersionAndCryptKey_1125(int capacity) : base(capacity)
		{
		}
	}
}