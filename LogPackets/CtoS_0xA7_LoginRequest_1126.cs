using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 1126, ePacketDirection.ClientToServer, "Login request v1126")]
	public class CtoS_0xA7_LoginRequest_1126 : CtoS_0xA7_LoginRequest_1125
	{
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {           
            text.Write(" account name: {0} | password: {1}", clientAccountName, clientAccountPassword);
        }

        public override void Init()
		{			
			clientAccountName = ReadString((int)ReadIntLowEndian());
			clientAccountPassword = ReadString((int)ReadIntLowEndian());
			Skip(13);
		}
		
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_1126(int capacity) : base(capacity)
		{
		}
	}
}