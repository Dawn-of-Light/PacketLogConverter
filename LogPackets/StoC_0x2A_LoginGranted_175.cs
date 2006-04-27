using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2A, 175, ePacketDirection.ServerToClient, "Login granted v175")]
	public class StoC_0x2A_LoginGranted_175 : StoC_0x2A_LoginGranted
	{
		protected byte unk1_175;

		#region public access properties

		public byte Unk1_175 { get { return unk1_175; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return base.GetPacketDataString(flagsDescription) + " cluster?:" + unk1_175;
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			base.Init();
			unk1_175 = ReadByte();
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