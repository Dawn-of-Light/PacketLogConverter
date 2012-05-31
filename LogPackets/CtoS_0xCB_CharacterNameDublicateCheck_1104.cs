using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xCB, 1104, ePacketDirection.ClientToServer, "Character name dublicate check v1104")]
	public class CtoS_0xCB_CharacterNameDublicateCheck_1104: CtoS_0xCB_CharacterNameDublicateCheck
	{
		protected uint unk1_1104;

		#region public access properties

		public uint Unk1_1104 { get { return unk1_1104; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			if (flagsDescription)
				text.Write(" unk1_1104:0x{0:X8}", unk1_1104);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			charName = ReadString(30);
			loginName = ReadString(24);
			unk1_1104 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xCB_CharacterNameDublicateCheck_1104(int capacity) : base(capacity)
		{
		}
	}
}