using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9F, -1, ePacketDirection.ClientToServer, "Disband from group")]
	public class CtoS_0x9F_DisbandFromGroup : Packet
	{
		#region public access properties

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
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
		public CtoS_0x9F_DisbandFromGroup(int capacity) : base(capacity)
		{
		}
	}
}