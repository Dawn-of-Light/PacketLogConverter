using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x018, -1, ePacketDirection.ClientToServer, "House decoration rotate request")]
	public class CtoS_0x18_HouseDecorationRotateRequest: Packet, IOidPacket
	{
		protected ushort houseOid;
		protected byte index;
		protected byte unk1;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort HouseOid { get { return houseOid; } }
		public byte Index { get { return index; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("houseOid:0x{0:X4} index:{1} unk1:0x{2:X2}", houseOid, index, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			index = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x18_HouseDecorationRotateRequest(int capacity) : base(capacity)
		{
		}
	}
}