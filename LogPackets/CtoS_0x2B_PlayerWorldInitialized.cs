using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2B, -1, ePacketDirection.ClientToServer, "Player Wolrd initialized")]
	public class CtoS_0x2B_PlayerWorldInialized: Packet
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1 ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:{0}", unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x2B_PlayerWorldInialized(int capacity) : base(capacity)
		{
		}
	}
}