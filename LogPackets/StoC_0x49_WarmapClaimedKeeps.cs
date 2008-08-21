using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x49, -1, ePacketDirection.ServerToClient, "Show warmap")]
	public class StoC_0x49_WarmapShow : Packet, IKeepIdPacket
	{
		protected ushort templeBitMask;
		protected byte countKeep;
		protected byte countTower;
		protected byte r1;
		protected byte r2;
		protected byte r3;
		protected byte r4;
		protected byte r5;
		protected byte r6;
		protected Keep[] m_keeps;

		protected ushort[] m_oids;
		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
		{
			get { return m_oids; }
		}
		#region public access properties

		public int TempleBitMask { get { return templeBitMask; } }
		public byte CountKeep { get { return countKeep; } }
		public byte CountTower { get { return countTower; } }
		public byte R1 { get { return r1; } }
		public byte R2 { get { return r2; } }
		public byte R3 { get { return r3; } }
		public byte R4 { get { return r4; } }
		public byte R5 { get { return r5; } }
		public byte R6 { get { return r6; } }
		public Keep[] Keeps { get { return m_keeps; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("templesBitMask:0x{0:X4} AlbStr:0x{1:X2} MidStr:0x{2:X2} HibStr:0x{3:X2} AlbPow:0x{4:X2} MidPow:0x{5:X2} HibPow:0x{6:X2}",
				templeBitMask, r1, r2, r3, r4, r5, r6);
			text.Write(" keeps:{0} towers:{1,-2}", countKeep, countTower);

			for (int i = 0; i < countKeep + countTower; i++)
			{
				Keep keep = Keeps[i];
				int keepRealmMap = keep.id >> 6;
				int keepIndexOnMap = (keep.id >> 3) & 0x7;
				int keepTower = keep.id & 7;
				string flags_desc = "";
				if ((keep.flag & 0x4) == 0x4) flags_desc+=",claimed";
				if ((keep.flag & 0x8) == 0x8) flags_desc+=",under siege";
				if ((keep.flag & 0x10) == 0x10) flags_desc+=",TP";
				text.Write("\n\tid:0x{0:X2}(map:{3},index:{4},tower:{5},keepId=0x{6:X4}) flag:0x{1:X2}(realm:{7}) guild:\"{2}\"{8}",
					keep.id, keep.flag, keep.guild, keepRealmMap, keepIndexOnMap, keepTower, (keepRealmMap*25) + 25+ keepIndexOnMap + (keepTower << 8), keep.flag & 0x3, flags_desc);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			templeBitMask = ReadShort(); // 0x00
			countKeep = ReadByte();      // 0x02
			countTower = ReadByte();     // 0x03
			r1 = ReadByte();             // 0x04
			r2 = ReadByte();             // 0x05
			r3 = ReadByte();             // 0x06
			r4 = ReadByte();             // 0x07
			r5 = ReadByte();             // 0x08
			r6 = ReadByte();             // 0x09
			m_keeps = new Keep[countKeep + countTower];

			ArrayList arr = new ArrayList(countKeep + countTower);
			for (int i = 0; i < (countKeep + countTower); i++)
			{
				Keep keep = new Keep();

				keep.id = ReadByte();             // 0x0A+
				keep.flag = ReadByte();           // 0x0B+
				keep.guild = ReadPascalString();  // 0x0C+

				m_keeps[i] = keep;
				int keepRealmMap = keep.id >> 6;
				int keepIndexOnMap = (keep.id >> 3) & 0x7;
				int keepTower = keep.id & 7;
				arr.Add((ushort)((keepRealmMap*25) + 25+ keepIndexOnMap + (keepTower << 8)));
			}
			m_oids = (ushort[])arr.ToArray(typeof (ushort));
		}

		public struct Keep
		{
			public byte id;
			public byte flag;
			public string guild;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x49_WarmapShow(int capacity) : base(capacity)
		{
		}
	}
}