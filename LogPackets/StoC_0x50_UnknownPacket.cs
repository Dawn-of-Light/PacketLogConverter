using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x50, 199, ePacketDirection.ServerToClient, "Unknown packet")]
	public class StoC_0x50_UnknownPacket: Packet
	{
		protected uint unk1;

		#region public access properties

		public uint Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:0x{0:X8}", unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x50_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}