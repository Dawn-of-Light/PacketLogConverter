using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x49, -1, ePacketDirection.ServerToClient, "Show warmap")]
	public class StoC_0x49_WarmapShow : Packet, IObjectIdPacket
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
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
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

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("templesBitMask:0x{0:X4} AlbStr:0x{1:X2} MidStr:0x{2:X2} HibStr:0x{3:X2} AlbPow:0x{4:X2} MidPow:0x{5:X2} HibPow:0x{6:X2}",
				templeBitMask, r1, r2, r3, r4, r5, r6);
			str.AppendFormat(" keeps:{0} towers:{1,-2}", countKeep, countTower);

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
				str.AppendFormat("\n\tid:0x{0:X2}(map:{3},index:{4},tower:{5},keepId=0x{6:X4}) flag:0x{1:X2}(realm:{7}) guild:\"{2}\"{8}",
					keep.id, keep.flag, keep.guild, keepRealmMap, keepIndexOnMap, keepTower, (keepRealmMap*25) + 25+ keepIndexOnMap + (keepTower << 8), keep.flag & 0x3, flags_desc);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			templeBitMask = ReadShort();
			countKeep = ReadByte();
			countTower = ReadByte();
			r1 = ReadByte();
			r2 = ReadByte();
			r3 = ReadByte();
			r4 = ReadByte();
			r5 = ReadByte();
			r6 = ReadByte();
			m_keeps = new Keep[countKeep + countTower];

			ArrayList arr = new ArrayList(countKeep + countTower);
			for (int i = 0; i < (countKeep + countTower); i++)
			{
				Keep keep = new Keep();

				keep.id = ReadByte();
				keep.flag = ReadByte();
				keep.guild = ReadPascalString();

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