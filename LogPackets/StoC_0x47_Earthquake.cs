using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x47, 187, ePacketDirection.ServerToClient, "Earthquake")]
	public class StoC_0x47_Earthquake: Packet
	{
		protected uint unk1; // (must be 0 or 1, if == 1, x and y will be recalculated on client as player x,y)
		protected uint x;
		protected uint y;
		protected uint unk4;
		protected uint unk5; // in client as unk5 * unk5
		protected uint unk6;
		protected uint unk7;
		protected uint unk8;

		#region public access properties

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			if (!flagsDescription)
			{
				text.Write("unk1:{0} x:{1,-6} y:{2,-6} z?:0x{3,-6} unk5:0x{4:X2} unk6:0x{5:X2} unk7:0x{6:X2} unk8:0x{7:X2}",
				(byte)unk1, x, y, (byte)unk4, (byte)unk5, (byte)unk6, (byte)unk7, (byte)unk8);
			}
			else
			{
				text.Write("unk1:0x{0:X8} x:{1,-6} y:{2,-6} z?:{3,-6} unk5:0x{4:X8} unk6:0x{5:X8} unk7:0x{6:X8} unk8:0x{7:X8}",
				unk1, x, y, unk4, unk5, unk6, unk7, unk8);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadIntLowEndian(); // 0x00
			x = ReadIntLowEndian();    // 0x04
			y = ReadIntLowEndian();    // 0x08
			unk4 = ReadIntLowEndian(); // 0x0C
			unk5 = ReadInt();          // 0x10
			unk6 = ReadInt();          // 0x14
			unk7 = ReadInt();          // 0x18
			unk8 = ReadInt();          // 0x1C
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