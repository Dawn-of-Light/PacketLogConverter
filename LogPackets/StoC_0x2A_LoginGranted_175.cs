using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2A, 175, ePacketDirection.ServerToClient, "Login granted v175")]
	public class StoC_0x2A_LoginGranted_175 : StoC_0x2A_LoginGranted
	{
		protected byte serverExpantion;

		#region public access properties

		public byte ServerExpantion { get { return serverExpantion; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" serverExpantion:0x{0:X2}", serverExpantion);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			base.Init();
			serverExpantion = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x2A_LoginGranted_175(int capacity) : base(capacity)
		{
		}
	}
}