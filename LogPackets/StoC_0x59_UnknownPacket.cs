using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x59, -1, ePacketDirection.ServerToClient, "Unknown packet")]
	public class StoC_0x59_UnknownPacket: Packet
	{
		protected uint unk1;
		protected uint unk2;

		#region public access properties

		public uint Unk1 { get { return unk1 ; } }
		public uint Unk2 { get { return unk2 ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk:0x{0:X8} {1:X8}", unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadIntLowEndian();
			unk2 = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x59_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}