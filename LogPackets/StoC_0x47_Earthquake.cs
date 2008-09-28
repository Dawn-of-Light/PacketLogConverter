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
		protected float unk5; // radius ?
		protected float unk6;
		protected float unk7;
		protected float unk8;

		#region public access properties

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:{0} x:{1,-6} y:{2,-6} z?:0x{3,-6} unk5:{4,-4} unk6:{5,-4} unk7:{6,-4} unk8:{7}",
			unk1, x, y, unk4, unk5, unk6, unk7, unk8);
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
			unk5 = ReadFloatLowEndian(); // 0x10
			unk6 = ReadFloatLowEndian(); // 0x14
			unk7 = ReadFloatLowEndian(); // 0x18
			unk8 = ReadFloatLowEndian(); // 0x1C
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