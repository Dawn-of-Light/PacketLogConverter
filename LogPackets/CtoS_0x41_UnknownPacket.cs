using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x41, -1, ePacketDirection.ClientToServer, "Unknown packet")]
	public class CtoS_0x41_UnknownPacket: Packet
	{
		protected ushort unk1;

		#region public access properties

		public ushort Unk1 { get { return unk1 ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X4}", unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShortLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x41_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}