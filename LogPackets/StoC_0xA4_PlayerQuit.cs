using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA4, -1, ePacketDirection.ServerToClient, "Player quit")]
	public class StoC_0xA4_PlayerQuit: Packet
	{
		protected byte flag;
		protected byte level;

		#region public access properties

		public byte Flag { get { return flag; } }
		public byte Level { get { return level; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("code:{0} playerLevel:{1}", flag, level);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flag = ReadByte();
			level = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xA4_PlayerQuit(int capacity) : base(capacity)
		{
		}
	}
}