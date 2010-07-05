using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xCB, 204, ePacketDirection.ClientToServer, "Character name dublicate check v204")]
	public class CtoS_0xCB_CharacterNameDublicateCheck_204: CtoS_0xCB_CharacterNameDublicateCheck
	{
		protected uint unk1_204;

		#region public access properties

		public uint Unk1_204 { get { return unk1_204; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			if (flagsDescription)
				text.Write(" unk1_204:0x{0:X8}", unk1_204);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			charName = ReadString(30);
			loginName = ReadString(24);
			unk1_204 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xCB_CharacterNameDublicateCheck_204(int capacity) : base(capacity)
		{
		}
	}
}