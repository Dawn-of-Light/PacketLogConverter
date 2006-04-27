using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x00, -1, ePacketDirection.ClientToServer, "House Menu request")]
	public class CtoS_0x00_HouseMenuRequest: Packet, IOidPacket
	{
		protected ushort houseOid;
		protected byte code;
		protected byte unk1; // Trailing zero ?

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return houseOid; } }
		public byte MenuCode { get { return code; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string code_type;
			switch (code)
			{
				case 2:
					code_type = "outside";
					break;
				case 8:
					code_type = "inside";
					break;
				default:
					code_type = "UNKNOWN";
					break;
			}
			str.AppendFormat("houseOid:0x{0:X4} code:{1}({3}) unk1:0x{2:X2}",
				houseOid, code, unk1, code_type);

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
		public CtoS_0x00_HouseMenuRequest(int capacity) : base(capacity)
		{
		}
	}
}