using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4B, 174, ePacketDirection.ServerToClient, "Player create v174")]
	public class StoC_0x4B_PlayerCreate_174 : StoC_0x4B_PlayerCreate_172
	{
		protected byte moodType;
		protected byte unk1_174;
		protected string prefixName;
		protected string realmMissionTitle;

		#region public access properties

		public byte MoodType { get { return moodType; } }
		public byte Unk1_174 { get { return unk1_174; } }
		public string PrefixName { get { return prefixName; } }
		public string RealmMissionTitle { get { return realmMissionTitle; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} oid:0x{1:X4} model:0x{2:X4} zoneId:{3,-3} zoneLoc:({4,-5} {5,-5} {6,-5}) heading:0x{7:X4} eyeSize:0x{8:X4} lipSize:0x{9:X4} eyeColor:0x{10:X4} level:{11,-2} hairColor:0x{12:X4} faceType:0x{13:X4} moodType:0x{19:X4} flags:0x{14:X2} name:\"{15}\" guild:\"{16}\" lastName:\"{17}\" prefixName:\"{18}\" unk1_174:{20} newTitle:\"{21}\"",
				sessionId, oid, model, zoneId, zoneX, zoneY, zoneZ, heading, eyeSize, lipSize, eyeColor, level, hairColor, faceType, flags, name, guildName, lastName, prefixName, moodType, unk1_174, realmMissionTitle);

			if (flagsDescription && flags != 0)
			{
				str.AppendFormat("\n\trealm:{0}", (flags >> 2) & 3);
				str.AppendFormat(" face:{0} playerSize:{1} model:0x{2:X4}", model >> 13, (model >> 11) & 3, model & 0x7FF);
				str.AppendFormat("{0}{1}{2}{3}{4}", ((flags & 1) == 1) ? ", DEAD" : "", ((flags & 2) == 2) ? ", Underwater" : "", ((flags & 0x10) == 0x10) ? ", Stealth" : "", ((flags & 0x20) == 0x20) ? ", Wireframe" : "", ((flags & 0x40) == 0x40) ? ", Vampiire" : "", ((flags & 0x80) != 0) ? ", UNK:0x"+(flags & 0x80).ToString("X2") : "");
			}
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			oid = ReadShort();
			model = ReadShort();
			zoneZ = ReadShort();
			zoneId = ReadShort();
			zoneX = ReadShort();
			zoneY = ReadShort();
			heading = ReadShort();
			eyeSize = ReadByte();
			lipSize = ReadByte();
			moodType = ReadByte();
			eyeColor = ReadByte();
			level = ReadByte();
			hairColor = ReadByte();
			faceType = ReadByte();
			hairColor = ReadByte();
			flags = ReadByte();
			unk1_174 = ReadByte();
			name = ReadPascalString();
			guildName = ReadPascalString();
			lastName = ReadPascalString();
			prefixName = ReadPascalString();
			realmMissionTitle = ReadPascalString();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4B_PlayerCreate_174(int capacity) : base(capacity)
		{
		}
	}
}