using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x45, -1, ePacketDirection.ServerToClient, "Warmap mino relic hide?")]
	public class StoC_0x45_UnknownPacket: Packet
	{
		protected uint id;

		#region public access properties

		public uint Id { get { return id; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("id:{0}", id);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			id = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x45_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}