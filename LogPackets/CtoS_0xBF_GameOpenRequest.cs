using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBF, -1, ePacketDirection.ClientToServer, "Game open request")]
	public class CtoS_0xBF_GameOpenRequest : Packet
	{
		protected byte useUDP;

		#region public access properties

		public byte UseUDP { get { return useUDP; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return string.Format("UDP:{0}({1})", useUDP, (useUDP == 1) ? "Enable" : "Disable");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			useUDP = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xBF_GameOpenRequest(int capacity) : base(capacity)
		{
		}
	}
}