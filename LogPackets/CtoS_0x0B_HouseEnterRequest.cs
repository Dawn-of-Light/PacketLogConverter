using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0B, -1, ePacketDirection.ClientToServer, "House Enter/Leave request")]
	public class CtoS_0x0B_HouseEnterRequest: Packet, IObjectIdPacket
	{
		protected ushort unk1;
		protected ushort houseOid;
		protected byte code;
		protected byte unk2;
		protected byte unk3;
		protected byte unk4; // Trailing zero ?

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { houseOid }; }
		}

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort Oid { get { return houseOid; } }
		public byte EnterCode { get { return code; } }
		public byte Unk2 { get { return unk2; } }
		public byte Unk3 { get { return unk3; } }
		public byte Unk4 { get { return unk4; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string code_type = "UNKNOWN";
			if (code==0)
				code_type = "leave";
			else if (code == 1)
				code_type = "enter";
			str.AppendFormat("unk1:0x{0:X4} houseOid:0x{1:X4} code:{2}({3}) unk2:0x{4:X2} unk3:0x{5:X2} unk4:0x{6:X2}",
				unk1, houseOid, code, code_type, unk2, unk3, unk4);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			houseOid = ReadShort();
			code = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadByte();
			unk4 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x0B_HouseEnterRequest(int capacity) : base(capacity)
		{
		}
	}
}