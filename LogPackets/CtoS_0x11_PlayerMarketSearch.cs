using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x11, -1, ePacketDirection.ClientToServer, "Player market search")]
	public class CtoS_0x11_PlayerMarketSearch: Packet
	{
		protected string filter;
		protected int slot;
		protected int skill;
		protected int resist;
		protected int bonus;
		protected int hp;
		protected int power;
		protected int proc;
		protected int qtyMin;
		protected int qtyMax;
		protected int levelMin;
		protected int levelMax;
		protected int priceMin;
		protected int priceMax;
		protected int visual;
		protected byte page;
		protected byte unk1;
		protected ushort unk2;


		#region public access properties

		public string Filter { get { return filter; } }
		public int Slot { get { return slot; } }
		public int Skill { get { return skill; } }
		public int Resist { get { return resist; } }
		public int Bonus { get { return bonus; } }
		public int HP { get { return hp; } }
		public int Power { get { return power; } }
		public int Proc { get { return proc; } }
		public int QtyMin { get { return qtyMin; } }
		public int QtyMax { get { return qtyMax; } }
		public int LevelMin { get { return levelMin; } }
		public int LevelMax { get { return levelMax; } }
		public int PriceMin { get { return priceMin; } }
		public int PriceMax { get { return priceMax; } }
		public int Visual { get { return visual; } }
		public byte Page { get { return page; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("slot:{0,2} skill:{1,2} resist:{2,2} bonus:{3,2} hp:{4,2} power:{5,2} proc:{6} qtyMin:{7,3} qtyMax:{8,3} levelMin:{9,2} levelMax:{10,2} priceMin:{11,2} priceMax:{12,2} visual:{13} page:{14,2} unk:0x{15:X2}{16:X4} filter:{17}",
				slot, skill, resist, bonus, hp, power, proc, qtyMin, qtyMax, levelMin, levelMax, priceMin, priceMax, visual, page, unk1, unk2, filter);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			filter = ReadString(64);
			slot = (int)ReadInt();
			skill = (int)ReadInt();
			resist = (int)ReadInt();
			bonus = (int)ReadInt();
			hp = (int)ReadInt();
			power = (int)ReadInt();
			proc = (int)ReadInt();
			qtyMin = (int)ReadInt();
			qtyMax = (int)ReadInt();
			levelMin = (int)ReadInt();
			levelMax = (int)ReadInt();
			priceMin = (int)ReadInt();
			priceMax = (int)ReadInt();
			visual = (int)ReadInt();
			page = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x11_PlayerMarketSearch(int capacity) : base(capacity)
		{
		}
	}
}