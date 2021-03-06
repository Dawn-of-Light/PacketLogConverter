using System.Collections;
using System.IO;
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

		public enum eOperation: uint
		{
			Delete = 0x12345678,
			Create = 0x23456789,
			Customize = 0x3456789A,
			Unknown = 0x456789AB,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("account name: \"{0}\"\n", accountName);
			for (int i = 0; i < chars.Length; i++)
			{
				CharData ch = chars[i];

				text.Write("[{12}]\tname:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:0x{9:X4} regId1:{10} databaseId:{11}",
					ch.charName, ch.zoneDescription, ch.className, ch.raceName, ch.level, ch.classID, ch.realm, ch.gender, ch.race, ch.model, ch.regionID, ch.databaseId, i);
				if (flagsDescription)
					text.Write(" (model:0x{0:X4} face:{1} size:{2})", ch.model & 0x7FF, ch.model >> 13, (ch.model >> 11) & 3);
				// race + gender + face + hair style = icon (fig3tofigmain.csv)
				text.Write("\n\tstr:{0} dex:{1} con:{2} qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", ch.statStr, ch.statDex, ch.statCon, ch.statQui, ch.statInt, ch.statPie, ch.statEmp, ch.statChr);

				text.Write("\n\tarmor models: (");
				foreach (DictionaryEntry entry in ch.armorModelBySlot)
				{
					int slot = (int)entry.Key;
					ushort model = (ushort)entry.Value;
					if (slot != 0x15) text.Write("; ");
					text.Write("slot:0x{0:X2} model:0x{1:X4}", slot, model);
				}

				text.Write(")\n\tarmor colors: (");
				foreach (DictionaryEntry entry in ch.armorColorBySlot)
				{
					int slot = (int)entry.Key;
					ushort color = (ushort)entry.Value;
					if (slot != 0x15) text.Write("; ");
					text.Write("slot:0x{0:X2} color:0x{1:X4}", slot, color);
				}

				text.Write(")\n\tweapon model: (");
				foreach (DictionaryEntry entry in ch.weaponModelBySlot)
				{
					int slot = (int)entry.Key;
					ushort model = (ushort)entry.Value;
					if (slot != 0x0A) text.Write("; ");
					text.Write("slot:0x{0:X2} model:0x{1:X4}", slot, model);
				}

				text.Write(")\n\tactiveRightSlot:0x{0:X2} activeLeftSlot:0x{1:X2} SIzone:0x{2:X2} clienTypeRequired:{3} unk2:0x{4:X2}\n", ch.activeRightSlot, ch.activeLeftSlot, ch.siZone, ch.regionID2, ch.unk2);
				if (ch.unk3.Length > 0)
				{
					text.Write("\tunk3:(");
					for (int j = 0; j < ch.unk3.Length ; j++)
					{
						if (j > 0)
							text.Write(',');
						text.Write("0x{0:X2}", ch.unk3[j]);
					}
					text.Write(")\n");
				}
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			ArrayList temp = new ArrayList(10);

			Position = 0;

			accountName = ReadString(24);

			while (Position < Length)
			{
				CharData charData = new CharData();

				charData.charName = ReadString(24);
				Skip(11); // 173 customize info
				ArrayList tmp = new ArrayList(13);
				for (byte j = 0; j < 13; j++)
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
				charData.race = (byte)((charData.temp & 0x0F) + ((charData.temp & 0x40) >> 2));
				charData.siStartLocation = (byte)((charData.temp & 0x80) >> 7);
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
			public byte siStartLocation;
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
			public uint operation;
			public byte unk4;
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