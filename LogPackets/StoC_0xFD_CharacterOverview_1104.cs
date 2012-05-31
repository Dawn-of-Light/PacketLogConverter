using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFD, 1104, ePacketDirection.ServerToClient, "Character Overview v1104")]
	public class StoC_0xFD_CharacterOverview_1104 : StoC_0xFD_CharacterOverview_199
	{
		protected uint unk1_1104;

		#region public access properties

		public uint Unk1_1104 { get { return unk1_1104; } }

		#endregion
		
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(8192);

			text.Write("account name: \"{0}\" unk1_1104:0x{1:X8}\n", accountName, unk1_1104);
			
			GetPacketCharacters(text, flagsDescription);

		}

		public override void Init()
		{
			Position = 0;

			accountName = ReadString(24);                  // 0x00
			unk1_1104 = ReadInt();
			ReadCharacters();
			ReadUnused(90);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFD_CharacterOverview_1104(int capacity) : base(capacity)
		{
		}
	}
}