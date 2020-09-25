using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB1, 1126, ePacketDirection.ServerToClient, "Start Arena 1126")]
	public class StoC_0xB1_StartArena1126 : StoC_0xB1_StartArena
	{
		ushort fromPort;
		ushort toPort;

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("serverIp:\"{0}\" portFrom:{1} portTo:{2}",
				 serverIp, fromPort, toPort);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			serverIp = ReadPascalStringIntLowEndian();
			fromPort = ReadShort();
			Skip(2);
			toPort = ReadShort();
			Skip(2);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xB1_StartArena1126(int capacity) : base(capacity)
		{
		}
	}
}