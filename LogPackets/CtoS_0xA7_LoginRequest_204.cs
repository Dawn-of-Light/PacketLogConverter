using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 204, ePacketDirection.ClientToServer, "Login request v204")]
	public class CtoS_0xA7_LoginRequest_204 : CtoS_0xA7_LoginRequest_178
	{
		protected uint unk1_204;

		#region public access properties

		public uint Unk1_204 { get { return unk1_204; } }

		#endregion
		

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" unk1_204:0x{0:X8}", unk1_204);
		}

		public override void Init()
		{
			base.Init();
			unk1_204 = ReadInt();

		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_204(int capacity) : base(capacity)
		{
		}
	}
}