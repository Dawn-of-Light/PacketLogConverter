using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0F, -1, ePacketDirection.ServerToClient, "Housepoint view")]
	public class StoC_0x0F_HousePointView: Packet, IObjectIdPacket
	{
		protected ushort houseOid;
		protected byte code;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { houseOid }; }
		}

		#region public access properties

		public ushort Oid { get { return houseOid; } }
		public byte PointViewCode{ get { return code; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			string code_type;
			switch (code)
			{
//				case 2:
//					code_type = "disable";
//					break;
				case 3:
					code_type = "enable";
					break;
				case 4:
					code_type = "toggle";
					break;
				default:
					code_type = "UNKNOWN";
					break;
			}
			str.AppendFormat("houseOid:0x{0:X4} code:{1}({3}) unk1:0x{2:X2}", houseOid, code, unk1, code_type);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			code = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x0F_HousePointView(int capacity) : base(capacity)
		{
		}
	}
}