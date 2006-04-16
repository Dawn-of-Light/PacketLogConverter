using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x98, -1, ePacketDirection.ClientToServer, "Player accept group invite")]
	public class CtoS_0x98_PlayerAcceptGroupInvite: Packet
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1 ; } }

		#endregion

		public override string GetPacketDataString()
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
		public CtoS_0x98_PlayerAcceptGroupInvite(int capacity) : base(capacity)
		{
		}
	}
}