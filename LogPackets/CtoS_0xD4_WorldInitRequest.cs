using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD4, -1, ePacketDirection.ClientToServer, "World init request")]
	public class CtoS_0xD4_WorldInitRequest : Packet
	{
		#region public access properties

		#endregion

		public override string GetPacketDataString()
		{
			return "empty packet";
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
		public CtoS_0xD4_WorldInitRequest(int capacity) : base(capacity)
		{
		}
	}
}