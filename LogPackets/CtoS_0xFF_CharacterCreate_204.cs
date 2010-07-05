using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFF, 204, ePacketDirection.ClientToServer, "Create Character v204")]
	public class CtoS_0xFF_CreateCharacter_204 : CtoS_0xFF_CreateCharacter_199d
	{

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

				charData.unk1_199 = ReadInt();
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
				ArrayList tmp = new ArrayList(7);
				for (byte j = 0; j < 7; j++)
					tmp.Add(ReadByte());
				charData.unk3 = (byte[])tmp.ToArray(typeof (byte));
				charData.operation = ReadInt();
				charData.unk4 = ReadByte();
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

			chars = (CharData_199[])temp.ToArray(typeof (CharData_199));
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter_204(int capacity) : base(capacity)
		{
		}
	}
}