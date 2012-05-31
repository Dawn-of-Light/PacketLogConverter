using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, 1104, ePacketDirection.ClientToServer, "Character select request v1104")]
	public class CtoS_0x10_CharacterSelectRequest_1104 : CtoS_0x10_CharacterSelectRequest_190c
	{
		protected uint unk1_1104;

		#region public access properties

		public uint Unk1_1104 { get { return unk1_1104; } }

		#endregion
		

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" unk1_1104:0x{0:X8}", unk1_1104);
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
			unk1_1104 = ReadInt();
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
		public CtoS_0x10_CharacterSelectRequest_1104(int capacity) : base(capacity)
		{
		}
	}
}