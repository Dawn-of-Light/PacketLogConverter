using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFF, -1, ePacketDirection.ClientToServer, "Create Character")]
	public class CtoS_0xFF_CreateCharacter : Packet
	{
		protected string accountName;
		protected CharData[] chars;

		#region public access properties

		public string AccountName { get { return accountName; } }
		public CharData[] Chars { get { return chars; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(8192);

			str.AppendFormat("account name: \"{0}\"\n", accountName);
			for (int i = 0; i < chars.Length; i++)
			{
				CharData ch = chars[i];

				str.AppendFormat("name:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:{9} regId1:{10} databaseId:{11}",
					ch.charName, ch.zoneDescription, ch.className, ch.raceName, ch.level, ch.classID, ch.realm, ch.gender, ch.race, ch.model, ch.regionID, ch.databaseId);
				str.AppendFormat("\n\tstr:{0} dex:{1} con:{2} qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", ch.statStr, ch.statDex, ch.statCon, ch.statQui, ch.statInt, ch.statPie, ch.statEmp, ch.statChr);

				str.Append("\n\tarmor models: (");
				foreach (DictionaryEntry entry in ch.armorModelBySlot)
				{
					int slot = (int)entry.Key;
					ushort model = (ushort)entry.Value;
					if (slot != 0x15) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, model);
				}

				str.Append(")\n\tarmor colors: (");
				foreach (DictionaryEntry entry in ch.armorColorBySlot)
				{
					int slot = (int)entry.Key;
					ushort color = (ushort)entry.Value;
					if (slot != 0x15) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, color);
				}

				str.Append(")\n\tweapon model: (");
				foreach (DictionaryEntry entry in ch.weaponModelBySlot)
				{
					int slot = (int)entry.Key;
					ushort model = (ushort)entry.Value;
					if (slot != 0x0A) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, model);
				}

				str.AppendFormat(")\n\tactiveRightSlot:0x{0:X2} activeLeftSlot:0x{1:X2} SIzone:0x{2:X2} clienTypeRequired:{3} unk2:0x{4:X2}\n", ch.activeRightSlot, ch.activeLeftSlot, ch.siZone, ch.regionID2, ch.unk2);
				if (ch.unk3.Length > 0)
				{
					str.Append("\tunk3:(");
					for (int j = 0; j < ch.unk3.Length ; j++)
					{
						if (j > 0)
							str.Append(',');
						str.AppendFormat("0x{0:X2}", ch.unk3[j]);
					}
					str.Append(")\n");
				}
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			ArrayList temp = new ArrayList(10);

			Position = 0;

			accountName = ReadString(24);
			byte cloakHoodUp;

			while (Position < Length)
			{
				CharData charData = new CharData();

				charData.charName = ReadString(24);
				Skip(11); // ?
				ArrayList tmp = new ArrayList(13);
				for (byte j=0; j<13; j++)
					tmp.Add(ReadByte());
				charData.unk3 = (byte[])tmp.ToArray(typeof (byte));
				charData.zoneDescription = ReadString(24);
				charData.className = ReadString(24);
				charData.raceName = ReadString(24);
				charData.level = ReadByte();
				charData.classID = ReadByte();
				charData.realm = ReadByte();
				charData.temp = ReadByte();
				charData.gender = (byte)((charData.temp >> 4) & 1);
				charData.race = (byte)((charData.temp & 0x0F) | (charData.temp >> 5));
				charData.model = ReadShortLowEndian();
				charData.regionID = ReadByte();
				charData.regionID2 = ReadByte();
				charData.databaseId = ReadIntLowEndian();
				charData.statStr = ReadByte();
				charData.statDex = ReadByte();
				charData.statCon = ReadByte();
				charData.statQui = ReadByte();
				charData.statInt = ReadByte();
				charData.statPie = ReadByte();
				charData.statEmp = ReadByte();
				charData.statChr = ReadByte();

				charData.armorModelBySlot = new SortedList(0x1D-0x15);
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					charData.armorModelBySlot.Add(slot, ReadShortLowEndian());
				}

				charData.armorColorBySlot = new SortedList(0x1D-0x15);
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					charData.armorColorBySlot.Add(slot, ReadShortLowEndian());
				}

				charData.weaponModelBySlot = new SortedList(0x0E-0x0A);
				for (int slot = 0x0A; slot < 0x0E; slot++)
				{
					charData.weaponModelBySlot.Add(slot, ReadShortLowEndian());
				}

				charData.activeRightSlot = ReadByte();
				charData.activeLeftSlot = ReadByte();
				charData.siZone = ReadByte();
				charData.unk2 = ReadByte();

				temp.Add(charData);
			}

			chars = (CharData[])temp.ToArray(typeof (CharData));
		}

		public class CharData
		{
			public string charName;
			public string zoneDescription;
			public string className;
			public string raceName;
			public byte level;
			public byte classID;
			public byte realm;
			public byte temp;
			public byte gender;
			public byte race;
			public ushort model;
			public byte regionID;
			public byte regionID2;
			public uint databaseId; // http://www.camelotherald.com/chardisplay.php?s=&ServerName&c=&databaseId
			public byte statStr;
			public byte statDex;
			public byte statCon;
			public byte statQui;
			public byte statInt;
			public byte statPie;
			public byte statEmp;
			public byte statChr;
			public SortedList armorModelBySlot;
			public SortedList armorColorBySlot;
			public SortedList weaponModelBySlot;
			public byte activeRightSlot;
			public byte activeLeftSlot;
			public byte siZone;
			public byte unk2;
			public byte[] unk3;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter(int capacity) : base(capacity)
		{
		}
	}
}