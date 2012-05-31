using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD8, -1, ePacketDirection.ServerToClient, "detail display")]
	public class StoC_0xD8_DetailDisplay : Packet
	{
		protected string msg;

		#region public access properties

		public string Text { get { return msg; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("\"{0}\"", msg);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			msg = ReadString(2048);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD8_DetailDisplay(int capacity) : base(capacity)
		{
		}
	}
}