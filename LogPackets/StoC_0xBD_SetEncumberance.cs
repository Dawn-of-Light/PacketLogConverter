using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBD, -1, ePacketDirection.ServerToClient, "Set encumberance")]
	public class StoC_0xBD_SetEncumberance : Packet
	{
		protected ushort maxEncumberance;
		protected ushort encumberance;

		#region public access properties

		public ushort MaxEncumberance { get { return maxEncumberance; } }
		public ushort Encumberance { get { return encumberance; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("{0}/{1}", encumberance, maxEncumberance);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			maxEncumberance = ReadShort();
			encumberance = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xBD_SetEncumberance(int capacity) : base(capacity)
		{
		}
	}
}