using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF0, -1, ePacketDirection.ServerToClient, "Char create reply")]
	public class StoC_0xF0_CharCreateReply : Packet
	{
		protected string accountName;

		#region public access properties

		public string AccountName { get { return accountName; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "accountName:\"" + accountName + '"';
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			accountName = ReadString(24);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF0_CharCreateReply(int capacity) : base(capacity)
		{
		}
	}
}