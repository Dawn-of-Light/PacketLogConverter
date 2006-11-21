using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x40, -1, ePacketDirection.ClientToServer, "Unknown packet")]
	public class CtoS_0x40_UnknownPacket: Packet
	{
		protected uint unk1;
		protected uint unk2;
		protected uint unk3;
		protected uint unk4;
		protected uint unk5;

		#region public access properties

		public uint Unk1 { get { return unk1 ; } }
		public uint Unk2 { get { return unk2 ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk:0x{0:X8} {1:X8} {2:X8} {3:X8} {4:X8}", unk1, unk2, unk3, unk4, unk5);

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
			unk3 = ReadIntLowEndian();
			unk4 = ReadIntLowEndian();
			unk5 = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x40_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}