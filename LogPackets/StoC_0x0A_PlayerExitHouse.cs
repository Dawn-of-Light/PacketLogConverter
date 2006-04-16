using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0A, -1, ePacketDirection.ServerToClient, "Player exit house")]
	public class StoC_0x0A_ExitHouse: Packet
	{
		protected ushort houseOid;
		protected ushort unk1;

		#region public access properties

		public ushort HouseOid { get { return houseOid; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} unk1:0x{1:X4}", houseOid, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x0A_ExitHouse(int capacity) : base(capacity)
		{
		}
	}
}