using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFB, -1, ePacketDirection.ServerToClient, "Char Stats Update")]
	public class StoC_0xFB_CharStatsUpdate : Packet
	{
		protected ushort baseStr;
		protected ushort baseDex;
		protected ushort baseCon;
		protected ushort baseQui;
		protected ushort baseInt;
		protected ushort basePie;
		protected ushort baseEmp;
		protected ushort baseChr;
		protected ushort bonusStr;
		protected ushort bonusDex;
		protected ushort bonusCon;
		protected ushort bonusQui;
		protected ushort bonusInt;
		protected ushort bonusPie;
		protected ushort bonusEmp;
		protected ushort bonusChr;
		protected ushort maxHealth;
		protected byte unk1;
		protected byte unk2;

		#region public access properties

		public ushort BaseStr { get { return baseStr; } }
		public ushort BaseDex { get { return baseDex; } }
		public ushort BaseCon { get { return baseCon; } }
		public ushort BaseQui { get { return baseQui; } }
		public ushort BaseInt { get { return baseInt; } }
		public ushort BasePie { get { return basePie; } }
		public ushort BaseEmp { get { return baseEmp; } }
		public ushort BaseChr { get { return baseChr; } }
		public ushort BonusStr { get { return bonusStr; } }
		public ushort BonusDex { get { return bonusDex; } }
		public ushort BonusCon { get { return bonusCon; } }
		public ushort BonusQui { get { return bonusQui; } }
		public ushort BonusInt { get { return bonusInt; } }
		public ushort BonusPie { get { return bonusPie; } }
		public ushort BonusEmp { get { return bonusEmp; } }
		public ushort BonusChr { get { return bonusChr; } }
		public ushort MaxHealth { get { return maxHealth; } }
		public byte Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("stat base - str:{0,-3} dex:{1,-3} con:{2,-3} qui:{3,-3} int:{4,-3} pie:{5,-3} emp:{6,-3} chr:{7,-3}", baseStr, baseDex, baseCon, baseQui, baseInt, basePie, baseEmp, baseChr);
			str.AppendFormat("; stat mods - str:{0,-3} dex:{1,-3} con:{2,-3} qui:{3,-3} int:{4,-3} pie:{5,-3} emp:{6,-3} chr:{7,-3}", bonusStr, bonusDex, bonusCon, bonusQui, bonusInt, bonusPie, bonusEmp, bonusChr);
			str.AppendFormat("; maxHealth:{0,-4} unk1:{1} unk2:{2}", maxHealth, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			baseStr = ReadShort();
			baseDex = ReadShort();
			baseCon = ReadShort();
			baseQui = ReadShort();
			baseInt = ReadShort();
			basePie = ReadShort();
			baseEmp = ReadShort();
			baseChr = ReadShort();
			bonusStr = ReadShort();
			bonusDex = ReadShort();
			bonusCon = ReadShort();
			bonusQui = ReadShort();
			bonusInt = ReadShort();
			bonusPie = ReadShort();
			bonusEmp = ReadShort();
			bonusChr = ReadShort();
			maxHealth = ReadShort();
			unk1 = ReadByte();
			unk2 = ReadByte();
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