using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x00, -1, ePacketDirection.ClientToServer, "House Menu request")]
	public class CtoS_0x00_HouseMenuRequest: Packet, IObjectIdPacket
	{
		protected ushort houseOid;
		protected byte code;
		protected byte unk1; // Trailing zero ?

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
		public byte MenuCode { get { return code; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string code_type;
			switch (code)
			{
				case 0:
					code_type = "gardenStore";
					break;
				case 1:
					code_type = "InteriorDecorationStore";
					break;
				case 2:
					code_type = "outsideMenu";
					break;
				case 3:
					code_type = "houseMerchantStore";
					break;
				case 4:
					code_type = "houseVaultStore";
					break;
				case 5:
					code_type = "houseToolsStore";
					break;
				case 6:
					code_type = "houseBindstoneStore";
					break;
				case 7:
					code_type = "houseInfo";
					break;
				case 8:
					code_type = "insideMenu";
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