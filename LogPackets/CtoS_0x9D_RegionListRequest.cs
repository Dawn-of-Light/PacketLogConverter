using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, -1, ePacketDirection.ClientToServer, "Region list request")]
	public class CtoS_0x9D_RegionListRequest : Packet
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "unk1:0x"+unk1.ToString("X2");
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
		public CtoS_0x9D_RegionListRequest(int capacity) : base(capacity)
		{
		}
	}
}