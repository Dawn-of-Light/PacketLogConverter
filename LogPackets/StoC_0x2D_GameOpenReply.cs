using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2D, -1, ePacketDirection.ServerToClient, "Game open reply")]
	public class StoC_0x2D_GameOpenReply : Packet
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "unk1:" + unk1;
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
		public StoC_0x2D_GameOpenReply(int capacity) : base(capacity)
		{
		}
	}
}