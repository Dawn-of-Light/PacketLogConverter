using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, -1, ePacketDirection.ClientToServer, "Region list request")]
	public class CtoS_0x9D_RegionListRequest : Packet
	{
		protected byte flag;

		#region public access properties

		public byte Flag { get { return flag; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("flag:{0}", flag);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flag = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest(int capacity) : base(capacity)
		{
		}
	}
}