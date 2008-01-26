using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x47, 187, ePacketDirection.ServerToClient, "Earthquake")]
	public class StoC_0x47_Earthquake: Packet
	{
		protected uint unk1;
		protected uint x;
		protected uint y;
		protected uint unk4;
		protected uint unk5;
		protected uint unk6;
		protected uint unk7;
		protected uint unk8;

		#region public access properties

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk:0x{0:X8} x:{1,-6} y:{2,-6} 0x{3:X8} 0x{4:X8} 0x{5:X8} 0x{6:X8} 0x{7:X8}",
				unk1, x, y, unk4, unk5, unk6, unk7, unk8);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadIntLowEndian();
			x = ReadIntLowEndian();
			y = ReadIntLowEndian();
			unk4 = ReadIntLowEndian();
			unk5 = ReadInt();
			unk6 = ReadInt();
			unk7 = ReadInt();
			unk8 = ReadInt();
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