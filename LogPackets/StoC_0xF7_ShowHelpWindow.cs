using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF7, -1, ePacketDirection.ServerToClient, "Show help window")]
	public class StoC_0xF7_ShowHelp: Packet
	{
		protected ushort index;
		protected ushort lot;

		#region public access properties

		public ushort Index { get { return index; } }
		public ushort Lot { get { return lot; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("topicIndex:{0} houseLot?:{1}", index, lot);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			index = ReadShort();
			lot = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF7_ShowHelp(int capacity) : base(capacity)
		{
		}
	}
}