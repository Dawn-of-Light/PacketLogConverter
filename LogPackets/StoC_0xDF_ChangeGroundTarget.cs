using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDF, -1, ePacketDirection.ServerToClient, "Assist ground target")]
	public class StoC_0xDF_ChangeGroundTarget : Packet
	{
		protected uint x;
		protected uint y;
		protected uint z;

		#region public access properties

		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public uint Z { get { return z; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("x:{0:X8} y:{1:X8} z:0x{2:X8}", x, y, z);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			x = ReadInt(); // 0x00
			y = ReadInt(); // 0x04
			z = ReadInt(); // 0x08
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDF_ChangeGroundTarget(int capacity) : base(capacity)
		{
		}
	}
}