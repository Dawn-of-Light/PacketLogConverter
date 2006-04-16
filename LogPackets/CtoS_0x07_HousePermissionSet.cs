using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x07, -1, ePacketDirection.ClientToServer, "House set permission")]
	public class CtoS_0x07_HousePermissionSet: Packet, IOidPacket
	{
		protected byte level;
		protected byte unk1;
		protected ushort houseOid;
		protected Access permission;
		protected byte unk2; // trailng zero ?

		public int Oid1 { get { return houseOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public byte Level { get { return level; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Oid { get { return houseOid; } }
		public Access Permission { get { return permission; } }
		public byte Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X2} houseOid:0x{1:X4} unk2:0x{2:X2}", unk1, houseOid, unk2);
			str.AppendFormat("\n\tlevel:{0} unk1:0x{1:X2} unk2:0x{2:X2} unk3:0x{3:X2} unk4:0x{4:X2} unk5:0x{5:X2} unk6:0x{6:X2} unk7:0x{7:X2} unk8:0x{8:X2} unk9:0x{9:X2} unk10:0x{10:X2} unk11:0x{11:X2} unk12:0x{12:X2} unk13:0x{13:X2} unk14:0x{14:X2} unk15:0x{15:X2}",
				level, permission.unk1, permission.unk2, permission.unk3, permission.unk4, permission.unk5, permission.unk6, permission.unk7, permission.unk8,
				permission.unk9, permission.unk10, permission.unk11, permission.unk12, permission.unk13, permission.unk14, permission.unk15);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			level = ReadByte();
			unk1 = ReadByte();
			houseOid = ReadShort();
			permission = new Access();
			permission.unk1 = ReadByte();
			permission.unk2 = ReadByte();
			permission.unk3 = ReadByte();
			permission.unk4 = ReadByte();
			permission.unk5 = ReadByte();
			permission.unk6 = ReadByte();
			permission.unk7 = ReadByte();
			permission.unk8 = ReadByte();
			permission.unk9 = ReadByte();
			permission.unk10 = ReadByte();
			permission.unk11 = ReadByte();
			permission.unk12 = ReadByte();
			permission.unk13 = ReadByte();
			permission.unk14 = ReadByte();
			permission.unk15 = ReadByte();
			unk2 = ReadByte();
		}

		public class Access
		{
			public byte unk1;
			public byte unk2;
			public byte unk3;
			public byte unk4;
			public byte unk5;
			public byte unk6;
			public byte unk7;
			public byte unk8;
			public byte unk9;
			public byte unk10;
			public byte unk11;
			public byte unk12;
			public byte unk13;
			public byte unk14;
			public byte unk15;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x07_HousePermissionSet(int capacity) : base(capacity)
		{
		}
	}
}