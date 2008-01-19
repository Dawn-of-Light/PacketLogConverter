using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4B, 175, ePacketDirection.ServerToClient, "Player create v175")]
	public class StoC_0x4B_PlayerCreate_175 : StoC_0x4B_PlayerCreate_174
	{
		protected byte unk1_175;

		#region public access properties

		public byte Unk1_175 { get { return unk1_175; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return base.GetPacketDataString(flagsDescription) + " unk1_175:" + unk1_175.ToString();
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
		public StoC_0x4B_PlayerCreate_175(int capacity) : base(capacity)
		{
		}
	}
}