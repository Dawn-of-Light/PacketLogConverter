using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2A, 1125, ePacketDirection.ServerToClient, "Login granted v1125")]
	public class StoC_0x2A_LoginGranted_1125 : StoC_0x2A_LoginGranted_1111
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("clientAccountName:\"{0}\" serverName:\"{1}\" serverId:0x{2:X2} colorHandling:{3} trial:{4}",
				clientAccountName, serverName, serverId, colorHandling, trial);			
		}

		public override void Init()
		{
			Position = 0;

			clientAccountName = ReadPascalString();
			serverName = ReadPascalString();
			serverId = ReadByte();
			colorHandling = ReadByte();
			serverExpantion = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2A_LoginGranted_1125(int capacity) : base(capacity)
		{
		}
	}
}