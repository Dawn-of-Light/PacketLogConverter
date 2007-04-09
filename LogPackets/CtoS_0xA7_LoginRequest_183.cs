using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 183, ePacketDirection.ClientToServer, "Login request v183")]
	public class CtoS_0xA7_LoginRequest_183 : CtoS_0xA7_LoginRequest_178
	{
		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("|{0}{1}{2}|{3}|{4}",
				clientVersionMajor, clientVersionMinor, clientVersionBuild, clientAccountName, clientAccountPassword, cryptKeyRequests);
			if (AunkI.Length > 0)
			{
				str.Append("|AunkI");
				for (byte i = 0; i < AunkI.Length; i++)
					str.AppendFormat("|0x{0:X8}", AunkI[i]);
			}
			str.AppendFormat("|unkB1|0x{0:X2}|unkS1|0x{1:X4}", unkB1, unkS1);
			str.AppendFormat("|Aunk1|0x{0:X8}|0x{1:X8}|0x{2:X8}|0x{3:X8}", Aunk1, Aunk2, Aunk3, Aunk4);
			str.AppendFormat("|unk1|0x{0:X8}|unk2|0x{1:X8}|unk3|0x{2:X8}", unk1, unk2, unk3);
			str.AppendFormat("|EDI|0x{0:X8}|stack?|0x{1:X8}|", edi, AunkB);

			return str.ToString();
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