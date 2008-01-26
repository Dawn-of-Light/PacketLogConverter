using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
//	[LogPacket(0x, -1, ePacketDirection.ClientToServer, )]
	public class CtoS_0xXX_Template : Packet
	{
		#region public access properties

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xXX_Template(int capacity) : base(capacity)
		{
		}
	}
}