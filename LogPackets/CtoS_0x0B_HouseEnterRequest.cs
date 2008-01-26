using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0B, -1, ePacketDirection.ClientToServer, "House Enter/Leave request")]
	public class CtoS_0x0B_HouseEnterRequest: Packet, IHouseIdPacket
	{
		protected ushort unk1;
		protected ushort houseOid;
		protected byte code;
		protected byte unk2;
		protected ushort unk3;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort HouseId { get { return houseOid; } }
		public byte EnterCode { get { return code; } }
		public byte Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			string code_type = "UNKNOWN";
			if (code==0)
				code_type = "leave";
			else if (code == 1)
				code_type = "enter";
			text.Write("unk1:0x{0:X4} houseOid:0x{1:X4} code:{2}({3}) unk2:0x{4:X2} unk3:0x{5:X4}",
				unk1, houseOid, code, code_type, unk2, unk3);
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
			unk3 = ReadShort();
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