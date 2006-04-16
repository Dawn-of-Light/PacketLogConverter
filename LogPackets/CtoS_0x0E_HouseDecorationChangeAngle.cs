using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0E, -1, ePacketDirection.ClientToServer, "House decoration change angle")]
	public class CtoS_0x0E_HouseDecorationChangeAngle: Packet, IOidPacket
	{
		protected ushort index;
		protected ushort houseOid;
		protected ushort rotateAngle;
		protected ushort unk1;

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Index { get { return index; } }
		public ushort HouseOid { get { return houseOid; } }
		public ushort RotateAngle { get { return rotateAngle; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("index:{0} houseOid:0x{1:X4} rotateAngle:{2} unk1:0x{3:X4}", index, houseOid, rotateAngle,unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			index = ReadShort();
			houseOid = ReadShort();
			rotateAngle = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x0E_HouseDecorationChangeAngle(int capacity) : base(capacity)
		{
		}
	}
}