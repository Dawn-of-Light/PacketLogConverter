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
		protected float radius;
		protected float intensity;
		protected float duration;
		protected float delay;

		#region public access properties

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:{0} x:{1,-6} y:{2,-6} z?:{3,-5} radius:{4,-4} intensity:{5,-4} duration:{6,-4} delay:{7}",
			unk1, x, y, unk4, radius, intensity, duration, delay);
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
			radius = ReadFloatLowEndian(); // 0x10
			intensity = ReadFloatLowEndian(); // 0x14
			duration = ReadFloatLowEndian(); // 0x18
			delay = ReadFloatLowEndian(); // 0x1C
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