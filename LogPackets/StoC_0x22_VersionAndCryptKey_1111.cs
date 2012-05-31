using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x22, 1111, ePacketDirection.ServerToClient, "Version And Crypt Key v1111")]
	public class StoC_0x22_VersionAndCryptKey_1111 : StoC_0x22_VersionAndCryptKey_186
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("majorVersion:{0} minorVersion:{1} build:{2} keyLenght:{3}",
			                 majorVersion, minorVersion, build, keyLenght);
		}
		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			majorVersion = ReadByte();
			minorVersion = ReadByte();
			build = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x22_VersionAndCryptKey_1111(int capacity) : base(capacity)
		{
		}
	}
}