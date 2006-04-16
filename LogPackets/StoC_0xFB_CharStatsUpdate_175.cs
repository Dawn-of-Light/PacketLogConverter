using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFB, 175, ePacketDirection.ServerToClient, "Char Stats Update 175")]
	public class StoC_0xFB_CharStatsUpdate_175 : Packet
	{
		#region protected fields

		protected ushort _str;
		protected ushort _dex;
		protected ushort _con;
		protected ushort _qui;
		protected ushort _int;
		protected ushort _pie;
		protected ushort _emp;
		protected ushort _chr;
		protected ushort unk1;
		protected short b_str;
		protected short b_dex;
		protected short b_con;
		protected short b_qui;
		protected short b_int;
		protected short b_pie;
		protected short b_emp;
		protected short b_chr;
		protected short b_unk1;
		protected ushort i_str;
		protected ushort i_dex;
		protected ushort i_con;
		protected ushort i_qui;
		protected ushort i_int;
		protected ushort i_pie;
		protected ushort i_emp;
		protected ushort i_chr;
		protected ushort i_unk1;
		protected byte c_str;
		protected byte c_dex;
		protected byte c_con;
		protected byte c_qui;
		protected byte c_int;
		protected byte c_pie;
		protected byte c_emp;
		protected byte c_chr;
		protected byte c_unk1;
		protected byte r_str;
		protected byte r_dex;
		protected byte r_con;
		protected byte r_qui;
		protected byte r_int;
		protected byte r_pie;
		protected byte r_emp;
		protected byte r_chr;
		protected byte r_unk1;
		protected byte flag;
		protected byte conLost;
		protected ushort maxHealth;
		protected ushort unk2;

		#endregion

		#region public access properties

		public ushort str { get { return _str; } }
		public ushort dex { get { return _dex; } }
		public ushort con { get { return _con; } }
		public ushort qui { get { return _qui; } }
		public ushort @int { get { return _int; } }
		public ushort pie { get { return _pie; } }
		public ushort emp { get { return _emp; } }
		public ushort chr { get { return _chr; } }
		public ushort Unk1 { get { return unk1; } }
		public short B_str { get { return b_str; } }
		public short B_dex { get { return b_dex; } }
		public short B_con { get { return b_con; } }
		public short B_qui { get { return b_qui; } }
		public short B_int { get { return b_int; } }
		public short B_pie { get { return b_pie; } }
		public short B_emp { get { return b_emp; } }
		public short B_chr { get { return b_chr; } }
		public short B_unk1 { get { return b_unk1; } }
		public ushort I_str { get { return i_str; } }
		public ushort I_dex { get { return i_dex; } }
		public ushort I_con { get { return i_con; } }
		public ushort I_qui { get { return i_qui; } }
		public ushort I_int { get { return i_int; } }
		public ushort I_pie { get { return i_pie; } }
		public ushort I_emp { get { return i_emp; } }
		public ushort I_chr { get { return i_chr; } }
		public ushort I_unk1 { get { return i_unk1; } }
		public byte C_str { get { return c_str; } }
		public byte C_dex { get { return c_dex; } }
		public byte C_con { get { return c_con; } }
		public byte C_qui { get { return c_qui; } }
		public byte C_int { get { return c_int; } }
		public byte C_pie { get { return c_pie; } }
		public byte C_emp { get { return c_emp; } }
		public byte C_chr { get { return c_chr; } }
		public byte C_unk1 { get { return c_unk1; } }
		public byte R_str { get { return r_str; } }
		public byte R_dex { get { return r_dex; } }
		public byte R_con { get { return r_con; } }
		public byte R_qui { get { return r_qui; } }
		public byte R_int { get { return r_int; } }
		public byte R_pie { get { return r_pie; } }
		public byte R_emp { get { return r_emp; } }
		public byte R_chr { get { return r_chr; } }
		public byte R_unk1 { get { return r_unk1; } }
		public byte Flag { get { return flag; } }
		public byte ConLost { get { return conLost; } }
		public ushort MaxHealth { get { return maxHealth; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			if (flag==0)	str.Append("\n\t      stat |str|dex|con|qui|int|pie|emp|chr|");
			else if (flag==0xFF)	str.Append("\n\t    resist |cru|sla|thr|hea|col|mat|bod|spi|ene");
			else str.Append("UNKNOWN SUBCODE");

			str.AppendFormat("\n\tbase       |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				_str, _dex, _con, _qui, _int, _pie, _emp, _chr, unk1);

			str.AppendFormat("\n\tbuf        |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				b_str, b_dex, b_con, b_qui, b_int, b_pie, b_emp, b_chr, b_unk1);

			str.AppendFormat("\n\titem bonus |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				i_str, i_dex, i_con, i_qui, i_int, i_pie, i_emp, i_chr, i_unk1);

			str.AppendFormat("\n\titem cap   |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				c_str, c_dex, c_con, c_qui, c_int, c_pie, c_emp, c_chr, c_unk1);

			str.AppendFormat("\n\tra bonus   |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}|{8,-3}",
				r_str, r_dex, r_con, r_qui, r_int, r_pie, r_emp, r_chr, r_unk1);

			if (flag != 0xFF) str.AppendFormat("\n\tsubCode:{0} conLost:{1,-2} maxHealth:{2,-4} unk2:0x{3:X4}", flag, conLost, maxHealth, unk2);
			else str.AppendFormat("\n\tsubCode:{0} unk1:0x{1:X4} unk2:0x{2:X4} unk3:0x{3:X4}",  flag, conLost, maxHealth, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			_str = ReadShort();
			_dex = ReadShort();
			_con = ReadShort();
			_qui = ReadShort();
			_int = ReadShort();
			_pie = ReadShort();
			_emp = ReadShort();
			_chr = ReadShort();
			unk1 = ReadShort();
			b_str = (short)ReadShort();
			b_dex = (short)ReadShort();
			b_con = (short)ReadShort();
			b_qui = (short)ReadShort();
			b_int = (short)ReadShort();
			b_pie = (short)ReadShort();
			b_emp = (short)ReadShort();
			b_chr = (short)ReadShort();
			b_unk1 = (short)ReadShort();
			i_str = ReadShort();
			i_dex = ReadShort();
			i_con = ReadShort();
			i_qui = ReadShort();
			i_int = ReadShort();
			i_pie = ReadShort();
			i_emp = ReadShort();
			i_chr = ReadShort();
			i_unk1 = ReadShort();
			c_str = ReadByte();
			c_dex = ReadByte();
			c_con = ReadByte();
			c_qui = ReadByte();
			c_int = ReadByte();
			c_pie = ReadByte();
			c_emp = ReadByte();
			c_chr = ReadByte();
			c_unk1 = ReadByte();
			r_str = ReadByte();
			r_dex = ReadByte();
			r_con = ReadByte();
			r_qui = ReadByte();
			r_int = ReadByte();
			r_pie = ReadByte();
			r_emp = ReadByte();
			r_chr = ReadByte();
			r_unk1 = ReadByte();
			flag = ReadByte();
			conLost = ReadByte();

			maxHealth = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFB_CharStatsUpdate_175(int capacity) : base(capacity)
		{
		}
	}
}