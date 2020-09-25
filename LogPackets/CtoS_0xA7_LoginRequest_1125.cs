using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 1125, ePacketDirection.ClientToServer, "Login request v1125")]
	public class CtoS_0xA7_LoginRequest_1125 : CtoS_0xA7_LoginRequest_1115
	{		
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{			
			text.Write(" version:{0}.{1}{2}{3} : account name: {4} | password: {5}", clientVersionMajor, clientVersionMinor, clientVersionBuild, revision, clientAccountName, clientAccountPassword);
		}

		public override void Init()
		{
			clientType = ReadByte();
			clientVersionMajor = ReadByte();
			clientVersionMinor = ReadByte();
			clientVersionBuild = ReadByte();
			revision = ReadByte();
			build = ReadShort();
			clientAccountName = ReadString((int)ReadIntLowEndian());
			clientAccountPassword = ReadString((int)ReadIntLowEndian());			
		}
		
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_1125(int capacity) : base(capacity)
		{
		}
	}
}