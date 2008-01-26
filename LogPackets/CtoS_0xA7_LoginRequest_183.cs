using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 183, ePacketDirection.ClientToServer, "Login request v183")]
	public class CtoS_0xA7_LoginRequest_183 : CtoS_0xA7_LoginRequest_178
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("|{0}{1}{2}|{3}|{4}",
				clientVersionMajor, clientVersionMinor, clientVersionBuild, clientAccountName, clientAccountPassword, cryptKeyRequests);
			if (AunkI.Length > 0)
			{
				text.Write("|AunkI");
				for (byte i = 0; i < AunkI.Length; i++)
					text.Write("|0x{0:X8}", AunkI[i]);
			}
			text.Write("|unkB1|0x{0:X2}|unkS1|0x{1:X4}", unkB1, unkS1);
			text.Write("|Aunk1|0x{0:X8}|0x{1:X8}|0x{2:X8}|0x{3:X8}", aunk1, aunk2, aunk3, aunk4);
			text.Write("|unk1|0x{0:X8}|unk2|0x{1:X8}|unk3|0x{2:X8}", unk1, unk2, unk3);
			text.Write("|EDI|0x{0:X8}|stack?|0x{1:X8}|", edi, AunkB);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_183(int capacity) : base(capacity)
		{
		}
	}
}