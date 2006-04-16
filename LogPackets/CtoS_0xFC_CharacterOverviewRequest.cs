using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFC, -1, ePacketDirection.ClientToServer, "Character overview request")]
	public class CtoS_0xFC_CharacterOverviewRequest : Packet
	{
		protected string clientAccountName;

		#region public access properties

		public string ClientAccountName { get { return clientAccountName; } }

		#endregion

		public override string GetPacketDataString()
		{
			return "clientAccountName:\"" + clientAccountName + '"';
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientAccountName = ReadString(24);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFC_CharacterOverviewRequest(int capacity) : base(capacity)
		{
		}
	}
}