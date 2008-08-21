using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("sessionId:0x{0:X4} oid:0x{1:X4} model:0x{2:X4} zoneId:{3,-3} zoneLoc:({4,-5} {5,-5} {6,-5}) heading:0x{7:X4} eyeSize:0x{8:X2} lipSize:0x{9:X2} eyeColor:0x{10:X2} level:{11,-2} hairColor:0x{12:X2} faceType:0x{13:X2} moodType:0x{20:X2} hairStyle:0x{14:X2} flags:0x{15:X2} name:\"{16}\" guild:\"{17}\" lastName:\"{18}\" prefixName:\"{19}\" unk1_174:{21} newTitle:\"{22}\"",
				sessionId, oid, model, zoneId, zoneX, zoneY, zoneZ, heading, eyeSize, lipSize, eyeColor, level, hairColor, faceType, hairStyle, flags, name, guildName, lastName, prefixName, moodType, unk1_174, realmMissionTitle);

			if (flagsDescription && flags != 0)
			{
				// playerSize: 1(95%), 2(100%), 3(105%)
				text.Write("\n\trealm:{0}", (flags >> 2) & 3);
				text.Write(" face:{0} playerSize:{1} model:0x{2:X4}", model >> 13, (model >> 11) & 3, model & 0x7FF);
				text.Write("{0}{1}{2}{3}{4}", ((flags & 1) == 1) ? ", DEAD" : "", ((flags & 2) == 2) ? ", Underwater" : "", ((flags & 0x10) == 0x10) ? ", Stealth" : "", ((flags & 0x20) == 0x20) ? ", Wireframe" : "", ((flags & 0x40) == 0x40) ? ", Vampiire" : "", ((flags & 0x80) != 0) ? ", UNK:0x" + (flags & 0x80).ToString("X2") : "");
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort(); // 0x00
			oid = ReadShort();       // 0x02
			model = ReadShort();     // 0x04
			zoneZ = ReadShort();     // 0x06
			zoneId = ReadShort();    // 0x08
			zoneX = ReadShort();     // 0x0A
			zoneY = ReadShort();     // 0x0C
			heading = ReadShort();   // 0x0E
			eyeSize = ReadByte();    // 0x10
			lipSize = ReadByte();    // 0x11
			moodType = ReadByte();   // 0x12
			eyeColor = ReadByte();   // 0x13
			level = ReadByte();      // 0x14
			hairColor = ReadByte();  // 0x15
			faceType = ReadByte();   // 0x16
			hairStyle = ReadByte();  // 0x17
			flags = ReadByte();      // 0x18
			unk1_174 = ReadByte();   // 0x19
			name = ReadPascalString(); // 0x1A+
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