using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFB, -1, ePacketDirection.ServerToClient, "Char Stats Update")]
	public class StoC_0xFB_CharStatsUpdate : Packet
	{
		protected short baseStr;
		protected short baseDex;
		protected short baseCon;
		protected short baseQui;
		protected short baseInt;
		protected short basePie;
		protected short baseEmp;
		protected short baseChr;
		protected short bonusStr;
		protected short bonusDex;
		protected short bonusCon;
		protected short bonusQui;
		protected short bonusInt;
		protected short bonusPie;
		protected short bonusEmp;
		protected short bonusChr;
		protected short maxHealth;
		protected ushort unk1;

		#region public access properties

		public short BaseStr { get { return baseStr; } }
		public short BaseDex { get { return baseDex; } }
		public short BaseCon { get { return baseCon; } }
		public short BaseQui { get { return baseQui; } }
		public short BaseInt { get { return baseInt; } }
		public short BasePie { get { return basePie; } }
		public short BaseEmp { get { return baseEmp; } }
		public short BaseChr { get { return baseChr; } }
		public short BonusStr { get { return bonusStr; } }
		public short BonusDex { get { return bonusDex; } }
		public short BonusCon { get { return bonusCon; } }
		public short BonusQui { get { return bonusQui; } }
		public short BonusInt { get { return bonusInt; } }
		public short BonusPie { get { return bonusPie; } }
		public short BonusEmp { get { return bonusEmp; } }
		public short BonusChr { get { return bonusChr; } }
		public short MaxHealth { get { return maxHealth; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.Append("\n\t      stat |str|dex|con|qui|int|pie|emp|chr|");
			str.AppendFormat("\n\tbase       |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}",
				baseStr, baseDex, baseCon, baseQui, baseInt, basePie, baseEmp, baseChr);
			str.AppendFormat("\n\tbuf        |{0,-3}|{1,-3}|{2,-3}|{3,-3}|{4,-3}|{5,-3}|{6,-3}|{7,-3}",
				bonusStr, bonusDex, bonusCon, bonusQui, bonusInt, bonusPie, bonusEmp, bonusChr);
			str.AppendFormat("\n\tmaxHealth:{0,-4} unk1:{1} (0x{2:X4})", maxHealth, unk1, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			baseStr = (short)ReadShort();
			baseDex = (short)ReadShort();
			baseCon = (short)ReadShort();
			baseQui = (short)ReadShort();
			baseInt = (short)ReadShort();
			basePie = (short)ReadShort();
			baseEmp = (short)ReadShort();
			baseChr = (short)ReadShort();
			bonusStr = (short)ReadShort();
			bonusDex = (short)ReadShort();
			bonusCon = (short)ReadShort();
			bonusQui = (short)ReadShort();
			bonusInt = (short)ReadShort();
			bonusPie = (short)ReadShort();
			bonusEmp = (short)ReadShort();
			bonusChr = (short)ReadShort();
			maxHealth = (short)ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFB_CharStatsUpdate(int capacity) : base(capacity)
		{
		}
	}
}