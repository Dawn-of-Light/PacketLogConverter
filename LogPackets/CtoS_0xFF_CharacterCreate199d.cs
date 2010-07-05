using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	// 199d
	[LogPacket(0xFF, 199.1f, ePacketDirection.ClientToServer, "Create Character v199")]
	public class CtoS_0xFF_CreateCharacter_199d : CtoS_0xFF_CreateCharacter_199
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("account name: \"{0}\"\n", accountName);
			for (int i = 0; i < chars.Length; i++)
			{
				CharData_199 ch = (CharData_199)chars[i];

				text.Write("[{12}]\tname:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:0x{9:X4} regId1:{10} databaseId:{11}",
					ch.charName, ch.zoneDescription, ch.className, ch.raceName, ch.level, ch.classID, ch.realm, ch.gender, ch.race, ch.model, ch.regionID, ch.databaseId, i);
				if (flagsDescription)
					text.Write(" (model:0x{0:X4} face?:{1} size:{2})", ch.model & 0x7FF, ch.model >> 13, (ch.model >> 11) & 3);
				text.Write(" tutorial:{0}", ch.siStartLocation);
				text.Write("\n\tstr:{0} dex:{1} con:{2}({8}) qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", ch.statStr, ch.statDex, ch.statCon, ch.statQui, ch.statInt, ch.statPie, ch.statEmp, ch.statChr, ch.unk2);
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
				text.Write(")\n\tactiveRightSlot:0x{0:X2} activeLeftSlot:0x{1:X2} SIzone:0x{2:X2} clienTypeRequired:{3}\n", ch.activeRightSlot, ch.activeLeftSlot, ch.siZone, ch.regionID2);
				text.Write("\tunk1_199:0x{0:X8}", ch.unk1_199);
				text.Write("\toperation:0x{0:X8}({1})\n", ch.operation, (eOperation)ch.operation);
				if (ch.unk3.Length > 0)
				{
					text.Write("\tunk3:(");
					for (int j = 0; j < ch.unk3.Length ; j++)
					{
						if (j > 0)
							text.Write(',');
						text.Write("0x{0:X2}", ch.unk3[j]);
					}
					text.Write(",0x{0:X2}", ch.unk4);
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
				CharData_199 charData = new CharData_199();

				charData.charName = ReadString(24);

				// new in 173
				charData.customized = ReadByte(); // 0x00
				charData.eyeSize = ReadByte(); // 0x01
				charData.lipSize = ReadByte(); // 0x02
				charData.eyeColor = ReadByte(); // 0x03
				charData.hairColor = ReadByte(); // 0x04
				charData.faceType = ReadByte(); // 0x05
				charData.hairStyle = ReadByte(); // 0x06
				cloakHoodUp = ReadByte(); // 0x07
				charData.extensionBoots = (byte)(cloakHoodUp >> 4);
				charData.extensionGloves = (byte)(cloakHoodUp & 0xF);
				cloakHoodUp = ReadByte(); // 0x08
				charData.cloakHoodUp = (byte)(cloakHoodUp & 0xF);
				charData.extensionTorso = (byte)(cloakHoodUp >> 4);
				charData.customizationStep = ReadByte(); // 0x09
				charData.moodType = ReadByte(); // 0x0A
				charData.newGuildEmblem = ReadByte(); // 0x0B
				ArrayList tmp = new ArrayList(7);
				for (byte j = 0; j < 7; j++)
					tmp.Add(ReadByte()); // 0x0C+
				charData.unk3 = (byte[])tmp.ToArray(typeof (byte));
				charData.operation = ReadInt(); // 0x13
				charData.unk4 = ReadByte(); // not used // 0x17
				charData.zoneDescription = ReadString(24); // 0x18
				charData.className = ReadString(24); // 0x30
				charData.raceName = ReadString(24); // 0x48
				charData.level = ReadByte(); // 0x60
				charData.classID = ReadByte(); // 0x61
				charData.realm = ReadByte(); // 0x62
				charData.temp = ReadByte(); // 0x63
				charData.gender = (byte)((charData.temp >> 4) & 1);
				charData.race = (byte)((charData.temp & 0x0F) + ((charData.temp & 0x40) >> 2));
				charData.siStartLocation = (byte)((charData.temp & 0x80) >> 7);
				charData.model = ReadShortLowEndian(); // 0x64
				charData.regionID = ReadByte(); // 0x66
				charData.regionID2 = ReadByte(); // 0x67
				charData.databaseId = ReadIntLowEndian(); // 0x68
				charData.statStr = ReadByte(); // 0x6C
				charData.statDex = ReadByte(); // 0x6D
				charData.statCon = ReadByte(); // 0x6E
				charData.statQui = ReadByte(); // 0x6F
				charData.statInt = ReadByte(); // 0x70
				charData.statPie = ReadByte(); // 0x71
				charData.statEmp = ReadByte(); // 0x72
				charData.statChr = ReadByte(); // 0x73  154th byte

				charData.armorModelBySlot = new SortedList(0x1D-0x15);
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					charData.armorModelBySlot.Add(slot, ReadShortLowEndian()); // 0x74
				}

				charData.armorColorBySlot = new SortedList(0x1D-0x15);
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					charData.armorColorBySlot.Add(slot, ReadShortLowEndian()); // 0x84
				}


				charData.weaponModelBySlot = new SortedList(0x0E-0x0A);
				for (int slot = 0x0A; slot < 0x0E; slot++)
				{
					charData.weaponModelBySlot.Add(slot, ReadShortLowEndian()); // 0x94
				}

				charData.activeRightSlot = ReadByte(); // 0x9C
				charData.activeLeftSlot = ReadByte(); // 0x9D
				charData.siZone = ReadByte(); // 0x9E
				charData.unk1_199 = ReadInt(); // 0x9F
				charData.unk2 = ReadByte(); // 0xA3

				temp.Add(charData);
			}

			chars = (CharData_199[])temp.ToArray(typeof (CharData_199));
		}

		public class CharData_199 : CharData_173
		{
			public uint unk1_199; // unknown
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter_199d(int capacity) : base(capacity)
		{
		}
	}
}