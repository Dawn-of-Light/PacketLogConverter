using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFF, 189, ePacketDirection.ClientToServer, "Create Character v189")]
	public class CtoS_0xFF_CreateCharacter_189 : CtoS_0xFF_CreateCharacter_173
	{
		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(8192);

			str.AppendFormat("account name: \"{0}\"\n", accountName);
			for (int i = 0; i < chars.Length; i++)
			{
				CharData_173 ch = (CharData_173)chars[i];

				str.AppendFormat("name:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:0x{9:X4} regId1:{10} databaseId:{11}",
					ch.charName, ch.zoneDescription, ch.className, ch.raceName, ch.level, ch.classID, ch.realm, ch.gender, ch.race, ch.model, ch.regionID, ch.databaseId);
				if (flagsDescription)
					str.AppendFormat(" (model:0x{0:X4} face?:{1} size:{2})", ch.model & 0x7FF, ch.model >> 13, (ch.model >> 11) & 3);
				str.AppendFormat(" tutorial:{0}",ch.siStartLocation);
				str.AppendFormat("\n\tstr:{0} dex:{1} con:{2}({8}) qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", ch.statStr, ch.statDex, ch.statCon, ch.statQui, ch.statInt, ch.statPie, ch.statEmp, ch.statChr, ch.unk2);
				str.AppendFormat("\n\teyeSize:0x{0:X2} lipSize:0x{1:X2} eyeColor:0x{2:X2} hairColor:0x{3:X2} faceType:0x{4:X2} hairStyle:0x{5:X2} cloakHoodUp:0x{6:X2} custStep:0x{7:X2} moodType:0x{8:X2} customized:0x{9:X2}",
					ch.eyeSize, ch.lipSize, ch.eyeColor, ch.hairColor, ch.faceType, ch.hairStyle, ch.cloakHoodUp, ch.customizationStep, ch.moodType, ch.customized);
				str.AppendFormat("\n\tnewGuildEmblem:0x{0:X2}", ch.newGuildEmblem);

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
					str.AppendFormat("slot:0x{0:X2} color:0x{1:X4}", slot, color);
				}

				str.Append(")\n\tweapon model: (");
				foreach (DictionaryEntry entry in ch.weaponModelBySlot)
				{
					int slot = (int)entry.Key;
					ushort model = (ushort)entry.Value;
					if (slot != 0x0A) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, model);
				}
				str.AppendFormat("\n\textensionTorso:0x{0:X2} extensionGloves:0x{1:X2} extensionBoots:0x{2:X2}", ch.extensionTorso, ch.extensionGloves, ch.extensionBoots);
				str.AppendFormat(")\n\tactiveRightSlot:0x{0:X2} activeLeftSlot:0x{1:X2} SIzone:0x{2:X2} clienTypeRequired:{3}\n", ch.activeRightSlot, ch.activeLeftSlot, ch.siZone, ch.regionID2);

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
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter_189(int capacity) : base(capacity)
		{
		}
	}
}