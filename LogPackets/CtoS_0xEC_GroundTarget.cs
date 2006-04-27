using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEC, -1, ePacketDirection.ClientToServer, "Set ground target")]
	public class CtoS_0xEC_GroundTarget: Packet
	{
		protected uint x;
		protected uint y;
		protected uint z; // or short ukn + short z ?
		protected ushort len;
		protected byte flag;
		protected byte unk1;

		#region public access properties

		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public uint Z { get { return z; } }
		public ushort Len { get { return len; } }
		public byte Flag { get { return flag; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("x:{0,-6} y:{1,-6} z:{2,6} length:{3,-4} flag:0x{4:X2} unk1:{5}",
				x, y, z, len, flag, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			x = ReadInt();
			y = ReadInt();
			z = ReadInt();
			len = ReadShort();
			flag = ReadByte();
			unk1 = ReadByte();
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