using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x45, -1, ePacketDirection.ServerToClient, "Unknown packet")]
	public class StoC_0x45_UnknownPacket: Packet
	{
		protected uint unk1;

		#region public access properties

		public uint Unk1 { get { return unk1 ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk:0x{0:X8}", unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x45_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}