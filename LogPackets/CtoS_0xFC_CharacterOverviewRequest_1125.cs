using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFC, 1125, ePacketDirection.ClientToServer, "Character overview request 1125")]
	public class CtoS_0xFC_CharacterOverviewRequest_1125 : CtoS_0xFC_CharacterOverviewRequest
    {
        protected byte realm;
        protected string realmName = "";

        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
            switch (realm)
            {
                case 1: realmName = "Albion"; break;
                case 2: realmName = "Midgard"; break;
                case 3: realmName = "Hibernia"; break;
                default: realmName = "None"; break;
            }
            text.Write("Realm selected: {0}", realmName);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			realm = ReadByte();			
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFC_CharacterOverviewRequest_1125(int capacity) : base(capacity)
		{
		}
	}
}