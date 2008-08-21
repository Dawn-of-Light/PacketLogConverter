using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, 190.1f, ePacketDirection.ClientToServer, "Character select request v190c")]
	public class CtoS_0x10_CharacterSelectRequest_190c : CtoS_0x10_CharacterSelectRequest_174
	{
		protected string language;

		#region public access properties

		public string Language { get { return language; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" lng:");
			text.Write(language);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();   // 0x00
			regionIndex = ReadByte();  // 0x02
			unk1 = ReadByte();         // 0x03
			serverId = ReadByte();     // 0x04
			charName = ReadString(24); // 0x05
			unks0 = ReadShort();       // 0x1D
			unks1 = ReadByte();        // 0x1F
			unk0 = ReadIntLowEndian(); // 0x20
			loginName = ReadString(20);// 0x24
			u1 = ReadIntLowEndian();   // 0x38
			u2 = ReadIntLowEndian();   // 0x3C
			u3 = ReadIntLowEndian();   // 0x40
			u4 = ReadIntLowEndian();   // 0x44
			u5 = ReadIntLowEndian();   // 0x48
			u6 = ReadIntLowEndian();   // 0x4C
			u7 = ReadIntLowEndian();   // 0x50
			unk2 = ReadIntLowEndian(); // 0x54
			port = ReadShort();        // 0x58
			unk3 = ReadShort();        // 0x5A
			language = ReadString(8);  // 0x5C+
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x10_CharacterSelectRequest_190c(int capacity) : base(capacity)
		{
		}
	}
}