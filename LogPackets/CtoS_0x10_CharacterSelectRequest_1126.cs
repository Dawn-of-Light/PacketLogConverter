using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, 1126, ePacketDirection.ClientToServer, "Character select request 1126")]
	public class CtoS_0x10_CharacterSelectRequest_1126 : Packet
    {
        byte unk1;
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
            text.Write(unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
            unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x10_CharacterSelectRequest_1126(int capacity) : base(capacity)
		{
		}
	}
}