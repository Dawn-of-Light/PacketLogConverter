using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFD, 199, ePacketDirection.ServerToClient, "Character Overview v199")]
	public class StoC_0xFD_CharacterOverview_199 : StoC_0xFD_CharacterOverview_189
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(8192);

			text.Write("account name: \"{0}\"\n", accountName);
			
			GetPacketCharacters(text, flagsDescription);

		}

		public virtual void GetPacketCharacters(TextWriter text, bool flagsDescription)
		{
			for (int i = 0; i < chars.Length; i++)
			{
				CharData_199 ch = (CharData_199)chars[i];

				text.Write("[{12}]\tname:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:0x{9:X4} regId1:{10} databaseId:{11}",
					ch.charName, ch.zoneDescription, ch.className, ch.raceName, ch.level, ch.classID, ch.realm, ch.gender, ch.race, ch.model, ch.regionID, ch.databaseId, i);
				text.Write(" tutorial:{0}",ch.siStartLocation);
				if (flagsDescription)
					text.Write(" (model:0x{0:X4} face:{1} size:{2})", ch.model & 0x7FF, ch.model >> 13, (ch.model >> 11) & 3);
				text.Write("\n\tstr:{0} dex:{1} con:{2}({8}) qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", ch.statStr, ch.statDex, ch.statCon, ch.statQui, ch.statInt, ch.statPie, ch.statEmp, ch.statChr, ch.unk2);
				text.Write("\n\teyeSize:0x{0:X2} lipSize:0x{1:X2} eyeColor:0x{2:X2} hairColor:0x{3:X2} faceType:0x{4:X2} hairStyle:0x{5:X2} cloakHoodUp:0x{6:X2} custStep:0x{7:X2} moodType:0x{8:X2} customized:0x{9:X2}",
					ch.eyeSize, ch.lipSize, ch.eyeColor, ch.hairColor, ch.faceType, ch.hairStyle, ch.cloakHoodUp, ch.customizationStep, ch.moodType, ch.customized);
				text.Write("\n\tnewGuildEmblem:0x{0:X2} helmAndCloakVisibility:{1}", ch.newGuildEmblem, ch.unk3[0]);

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
				if (ch.unk3.Length > 1)
				{
					text.Write("\tunk3:(");
					for (int j = 1; j < ch.unk3.Length ; j++)
					{
						if (j > 1)
							text.Write(',');
						text.Write("0x{0:X2}", ch.unk3[j]);
					}
					text.Write(")\n");
				}
				text.Write("unk1_199:{0}\n",ch.unk1_199);
			}
			if (flagsDescription)
			{
				text.Write("unused:");
				for (int i = 0; i < unused.Length; i++)
				{
					text.Write("{0:X2}", unused[i]);
				}
			}
			else
				text.Write("and {0} bytes more unused", unused.Length);

		}

		public override void Init()
		{
			Position = 0;
			accountName = ReadString(24);
			ReadCharacters();
			ReadUnused(90);
		}

		public override void ReadCharacters()
		{
			ArrayList temp = new ArrayList(10);
			byte cloakHoodUp;

			while (Position + 184 < Length)
			{
				CharData_199 charData = new CharData_199();

				charData.charName = ReadString(24);        // 0x18, 0x00

				// new in 173
				charData.customized = ReadByte();          // 0x30, 0x18
				charData.eyeSize = ReadByte();             // 0x31, 0x19
				charData.lipSize = ReadByte();             // 0x32, 0x1A
				charData.eyeColor = ReadByte();            // 0x33, 0x1B
				charData.hairColor = ReadByte();           // 0x34, 0x1C
				charData.faceType = ReadByte();            // 0x35, 0x1D
				charData.hairStyle = ReadByte();           // 0x36, 0x1E
				cloakHoodUp = ReadByte();                  // 0x37, 0x1F
				charData.extensionBoots = (byte)(cloakHoodUp >> 4);
				charData.extensionGloves = (byte)(cloakHoodUp & 0xF);
				cloakHoodUp = ReadByte();                  // 0x38, 0x20
				charData.cloakHoodUp = (byte)(cloakHoodUp & 0xF);
				charData.extensionTorso = (byte)(cloakHoodUp >> 4);
				charData.customizationStep = ReadByte();   // 0x39, 0x21
				charData.moodType = ReadByte();            // 0x3A, 0x22
				charData.newGuildEmblem = ReadByte();      // 0x3B, 0x23
				ArrayList tmp = new ArrayList(12);
				for (byte j = 0; j < 12; j++)
					tmp.Add(ReadByte());                   // 0x3C-0x47, 0x24-0x2F
				charData.unk3 = (byte[])tmp.ToArray(typeof (byte));
				charData.zoneDescription = ReadString(24); // 0x48, 0x30
				charData.className = ReadString(24);       // 0x60, 0x48
				charData.raceName = ReadString(24);        // 0x78, 0x60
				charData.level = ReadByte();               // 0x90, 0x78 (if = 200 not allow enter)
				charData.classID = ReadByte();             // 0x91, 0x79
				charData.realm = ReadByte();               // 0x92, 0x7A
				charData.temp = ReadByte();                // 0x93, 0x7B
				charData.gender = (byte)((charData.temp >> 4) & 3);
				charData.race = (byte)((charData.temp & 0x0F) + ((charData.temp & 0x40) >> 2));
				charData.siStartLocation = (byte)(charData.temp >> 7);
				charData.model = ReadShortLowEndian();     // 0x94, 0x7C
				charData.regionID = ReadByte();            // 0x96, 0x7E
				charData.regionID2 = ReadByte();           // 0x97, 0x7F
				charData.databaseId = ReadIntLowEndian();  // 0x98, 0x80 // if level == 200 then this is time not allowed enter
				charData.statStr = ReadByte();             // 0x9C, 0x84
				charData.statDex = ReadByte();             // 0x9D, 0x85
				charData.statCon = ReadByte();             // 0x9E, 0x86
				charData.statQui = ReadByte();             // 0x9F, 0x87
				charData.statInt = ReadByte();             // 0xA0, 0x88
				charData.statPie = ReadByte();             // 0xA1, 0x89
				charData.statEmp = ReadByte();             // 0xA2, 0x8A
				charData.statChr = ReadByte(); // 154th byte  0xA3, 0x8B

				charData.armorModelBySlot = new SortedList(0x1D-0x15);
				for (int slot = 0x15; slot < 0x1D; slot++) // 0xA4, 0x8C+
				{
					charData.armorModelBySlot.Add(slot, ReadShortLowEndian());
				}

				charData.armorColorBySlot = new SortedList(0x1D-0x15); // 0xB4, 0x9C+
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					charData.armorColorBySlot.Add(slot, ReadShortLowEndian());
				}

				charData.weaponModelBySlot = new SortedList(0x0E-0x0A); // 0xC4, 0xAC+
				for (int slot = 0x0A; slot < 0x0E; slot++)
				{
					charData.weaponModelBySlot.Add(slot, ReadShortLowEndian());
				}

				charData.activeRightSlot = ReadByte();     // 0xCC, 0xB4
				charData.activeLeftSlot = ReadByte();      // 0xCD, 0xB5
				charData.siZone = ReadByte();              // 0xCE, 0xB6
				charData.unk2 = ReadByte();                // 0xCF, 0xB7
				charData.unk1_199 = ReadInt();             // 0xD0, 0xB8
				temp.Add(charData);
			}

			chars = (CharData_199[])temp.ToArray(typeof (CharData_199));

		}

		public class CharData_199 : CharData_173
		{
			public uint unk1_199;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFD_CharacterOverview_199(int capacity) : base(capacity)
		{
		}
	}
}