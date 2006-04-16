using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFF, 174, ePacketDirection.ClientToServer, "Create Character v174")]
	public class CtoS_0xFF_CreateCharacter_174 : Packet
	{
		public override string GetPacketDataString()
		{
			Position = 0;

			StringBuilder str = new StringBuilder(8192);

			string accountName = ReadString(24);

			str.AppendFormat("account name: \"{0}\"\n", accountName);
			while (Position + 184 < Length)
			{
				string charName = ReadString(24);

				// new in 173
				int unk3 = ReadByte();
				int eyeSize = ReadByte();
				int lipSize = ReadByte();
				int eyeColor = ReadByte();
				int hairColor = ReadByte();
				int faceType = ReadByte();
				int hairStyle = ReadByte();
				int unk4 = ReadByte();
				int cloakHoodUp = ReadByte();
				int customizationStep = ReadByte();
				int moodType = ReadByte();
				Skip(13); //0 String

				string zoneDescription = ReadString(24);
				string className = ReadString(24);
				string raceName = ReadString(24);
				int level = ReadByte();
				int classID = ReadByte();
				int realm = ReadByte();
				int temp = ReadByte();
				int gender = (temp >> 4) & 1;
				int race = (temp & 0x0F) | (temp >> 5);
				int model = ReadShortLowEndian();
				int regionID = ReadByte();
				int regionID2 = ReadByte();
				uint unk1 = ReadInt();
				int _str = ReadByte();
				int _dex = ReadByte();
				int _con = ReadByte();
				int _qui = ReadByte();
				int _int = ReadByte();
				int _pie = ReadByte();
				int _emp = ReadByte();
				int _chr = ReadByte();

				str.AppendFormat("name:\"{0}\" zone:\"{1}\" class:\"{2}\" race:\"{3}\" level:{4} classId:{5} realm:{6} gender:{7} race:{8} model:{9} regId1:{10} regId2:{11} unk1:{12}",
				                 charName, zoneDescription, className, raceName, level, classID, realm, gender, race, model, regionID, regionID2, unk1);
				str.AppendFormat("\n\tstr:{0} dex:{1} con:{2} qui:{3} int:{4} pie:{5} emp:{6} chr:{7}", _str, _dex, _con, _qui, _int, _pie, _emp, _chr);
				str.AppendFormat("\n\teyeSize:0x{0:X2} lipSize:0x{1:X2} eyeColor:0x{2:X2} hairColor:0x{3:X2} faceType:0x{4:X2} hairStyle:0x{5:X2} moodType:0x{10:X2} cloakHoodUp:0x{6:X2} custStep:0x{7:X2} unk3:0x{8:X2} unk4:0x{9:X2}",
				                 eyeSize, lipSize, eyeColor, hairColor, faceType, hairStyle, cloakHoodUp, customizationStep, unk3, unk4, moodType);

				str.Append("\n\tarmor models: (");
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					int armorModel = ReadShortLowEndian();
					if (slot != 0x15) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, armorModel);
				}

				str.Append(")\n\tarmor colors: (");
				for (int slot = 0x15; slot < 0x1D; slot++)
				{
					int armorColor = ReadShortLowEndian();
					if (slot != 0x15) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, armorColor);
				}

				str.Append(")\n\tweapon model: (");
				for (int slot = 0x0A; slot < 0x0E; slot++)
				{
					int weaponModel = ReadShortLowEndian();
					if (slot != 0x0A) str.Append("; ");
					str.AppendFormat("slot:0x{0:X2} model:0x{1:X4}", slot, weaponModel);
				}
				str.Append(")\n\t");

				int activeRightSlot = ReadByte();
				int activeLeftSlot = ReadByte();
				int siZone = ReadByte();
				int unk2 = ReadByte();
				str.AppendFormat("activeRightSlot:0x{0:X2} activeLeftSlot:0x{1:X2} SIzone:0x{2:X2} unk2:0x{3:X2}\n", activeRightSlot, activeLeftSlot, siZone, unk2);

			}

			// 0x68 more bytes?

			return str.ToString();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter_174(int capacity) : base(capacity)
		{
		}
	}
}