using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2A, 1111, ePacketDirection.ServerToClient, "Login granted v1111")]
	public class StoC_0x2A_LoginGranted_1111 : StoC_0x2A_LoginGranted_175
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("clientAccountName:\"{0}\" serverName:\"{1}\" serverId:0x{2:X2} colorHandling:{3} trial:{4}",
				clientAccountName, serverName, serverId, colorHandling, trial);
			text.Write(" serverExpantion:0x{0:X2}", serverExpantion);
		}

		public override void Init()
		{
			Position = 0;

			clientAccountName = ReadPascalString();
			serverName = ReadPascalString();
			serverId = ReadByte();
			colorHandling = ReadByte();
			trial = ReadByte();
			serverExpantion = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2A_LoginGranted_1111(int capacity) : base(capacity)
		{
		}
	}
}