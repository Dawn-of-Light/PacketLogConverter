using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 1104, ePacketDirection.ClientToServer, "Login request v1104")]
	public class CtoS_0xA7_LoginRequest_1104 : CtoS_0xA7_LoginRequest_183
	{
		protected uint unk1_1104;

		#region public access properties

		public uint Unk1_1104 { get { return unk1_1104; } }

		#endregion
		

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" unk1_1104:0x{0:X8}", Unk1_1104);
		}

		public override void Init()
		{
			base.Init();
			unk1_1104 = ReadInt();

		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_1104(int capacity) : base(capacity)
		{
		}
	}
}