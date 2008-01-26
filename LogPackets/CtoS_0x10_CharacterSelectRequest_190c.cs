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
//			((Packet) this).GetPacketDataString(text, flagsDescription); // this crash program
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

			sessionId = ReadShort();
			regionIndex = ReadByte();
			unk1 = ReadByte();
			serverId = ReadByte();
			charName = ReadString(24);
			unks0 = ReadShort();
			unks1 = ReadByte();
			unk0 = ReadIntLowEndian();
			loginName = ReadString(20);
			u1 = ReadIntLowEndian();
			u2 = ReadIntLowEndian();
			u3 = ReadIntLowEndian();
			u4 = ReadIntLowEndian();
			u5 = ReadIntLowEndian();
			u6 = ReadIntLowEndian();
			u7 = ReadIntLowEndian();
			unk2 = ReadIntLowEndian();
			port = ReadShort();
			unk3 = ReadShort();
			language = ReadString(8);
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