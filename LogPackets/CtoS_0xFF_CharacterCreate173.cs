using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFF, 173, ePacketDirection.ClientToServer, "Create Character v173")]
	public class CtoS_0xFF_CreateCharacter_173 : CtoS_0xFF_CreateCharacter
	{

		#region public access properties
		public CharData_173[] Chars_173 { get { return chars as CharData_173[]; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("account name: \"{0}\"\n", accountName);
			for (int i = 0; i < chars.Length; i++)
			{
				CharData_173 ch = (CharData_173)chars[i];

				text.Write("[{12}]\tname:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:0x{9:X4} regId1:{10} databaseId:{11}",
					ch.charName, ch.zoneDescription, ch.className, ch.raceName, ch.level, ch.classID, ch.realm, ch.gender, ch.race, ch.model, ch.regionID, ch.databaseId, i);
				if (flagsDescription)
					text.Write(" (model:0x{0:X4} face?:{1} size:{2})", ch.model & 0x7FF, ch.model >> 13, (ch.model >> 11) & 3);
				text.Write(" tutorial:{0}", ch.siStartLocation);
				text.Write("\n\tstr:{0} dex:{1} con:{2} qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", ch.statStr, ch.statDex, ch.statCon, ch.statQui, ch.statInt, ch.statPie, ch.statEmp, ch.statChr);
				text.Write("\n\teyeSize:0x{0:X2} lipSize:0x{1:X2} eyeColor:0x{2:X2} hairColor:0x{3:X2} faceType:0x{4:X2} hairStyle:0x{5:X2} cloakHoodUp:0x{6:X2} custStep:0x{7:X2} moodType:0x{8:X2} customized:0x{9:X2}",
					ch.eyeSize, ch.lipSize, ch.eyeColor, ch.hairColor, ch.faceType, ch.hairStyle, ch.cloakHoodUp, ch.customizationStep, ch.moodType, ch.customized);
				text.Write("\n\tnewGuildEmblem:0x{0:X2}", ch.newGuildEmblem);

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
				text.Write("\n\textensionTorso:0x{0:X2} extensionGloves:0x{1:X2} extensionBoots:0x{2:X2}", ch.extensionTorso, ch.extensionGloves, ch.extensionBoots);
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
			byte cloakHoodUp;

			while (Position < Length)
			{
				CharData_173 charData = new CharData_173();

				charData.charName = ReadString(24);

				// new in 173
				charData.customized = ReadByte();
				charData.eyeSize = ReadByte();
				charData.lipSize = ReadByte();
				charData.eyeColor = ReadByte();
				charData.hairColor = ReadByte();
				charData.faceType = ReadByte();
				charData.hairStyle = ReadByte();
				cloakHoodUp = ReadByte();
				charData.extensionBoots = (byte)(cloakHoodUp >> 4);
				charData.extensionGloves = (byte)(cloakHoodUp & 0xF);
				cloakHoodUp = ReadByte();
				charData.cloakHoodUp = (byte)(cloakHoodUp & 0xF);
				charData.extensionTorso = (byte)(cloakHoodUp >> 4);
				charData.customizationStep = ReadByte();
				charData.moodType = ReadByte();
				charData.newGuildEmblem = ReadByte();
				ArrayList tmp = new ArrayList(12);
				for (byte j = 0; j < 12; j++)
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
				charData.statChr = ReadByte(); // 154th byte

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

			chars = (CharData_173[])temp.ToArray(typeof (CharData_173));
		}

		public class CharData_173 : CharData
		{
			public byte customized;
			public byte eyeSize;
			public byte lipSize;
			public byte eyeColor;
			public byte hairColor;
			public byte faceType;
			public byte hairStyle;
			public byte cloakHoodUp;
			public byte extensionTorso;
			public byte extensionBoots;
			public byte extensionGloves;
			public byte customizationStep;
			public byte moodType;
			public byte newGuildEmblem;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter_173(int capacity) : base(capacity)
		{
		}
	}
}