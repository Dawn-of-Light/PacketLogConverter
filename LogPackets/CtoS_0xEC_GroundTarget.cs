using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEC, -1, ePacketDirection.ClientToServer, "Set ground target")]
	public class CtoS_0xEC_GroundTarget: Packet
	{
		protected uint x;
		protected uint y;
		protected uint z; // or short ukn + short z ?
		protected byte flag;
		protected ushort unk1;
		protected byte unk2;

		#region public access properties

		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public uint Z { get { return z; } }
		public byte Flag { get { return flag; } }
		public ushort Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("x:{0,-6} y:{1,-6} z:{2,6} flag:0x{3:X2} unk1:0x{4:X4} unk2:0x{5:X2}",
				x, y, z, flag, unk1, unk2);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			x = ReadInt();      // 0x00
			y = ReadInt();      // 0x04
			z = ReadInt();      // 0x08
			flag = ReadByte();  // 0x0C
			unk1 = ReadShort(); // 0x0D
			unk2 = ReadByte();  // 0x0E
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xEC_GroundTarget(int capacity) : base(capacity)
		{
		}
	}
}