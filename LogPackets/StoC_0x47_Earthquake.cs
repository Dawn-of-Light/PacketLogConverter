using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x47, 187, ePacketDirection.ServerToClient, "Earthquake")]
	public class StoC_0x47_Earthquake: Packet
	{
		protected uint unk1;
		protected uint unk2;
		protected uint unk3;
		protected uint unk4;
		protected uint unk5;
		protected uint unk6;
		protected uint unk7;
		protected uint unk8;

		#region public access properties

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk:0x{0:X8} 0x{1:X8} 0x{2:X8} 0x{3:X8} 0x{4:X8} 0x{5:X8} 0x{6:X8} 0x{7:X8}",
				unk1, unk2, unk3, unk4, unk5, unk6, unk7, unk8);
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
			unk6 = ReadIntLowEndian();
			unk7 = ReadIntLowEndian();
			unk8 = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x47_Earthquake(int capacity) : base(capacity)
		{
		}
	}
}