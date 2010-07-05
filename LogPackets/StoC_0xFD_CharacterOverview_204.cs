using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFD, 204, ePacketDirection.ServerToClient, "Character Overview v204")]
	public class StoC_0xFD_CharacterOverview_204 : StoC_0xFD_CharacterOverview_199
	{
		protected uint unk1_204;

		#region public access properties

		public uint Unk1_204 { get { return unk1_204; } }

		#endregion
		
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			StringBuilder str = new StringBuilder(8192);

			text.Write("account name: \"{0}\" unk1_204:0x{1:X8}\n", accountName, unk1_204);
			
			GetPacketCharacters(text, flagsDescription);

		}

		public override void Init()
		{
			Position = 0;

			accountName = ReadString(24);                  // 0x00
			unk1_204 = ReadInt();
			ReadCharacters();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFD_CharacterOverview_204(int capacity) : base(capacity)
		{
		}
	}
}