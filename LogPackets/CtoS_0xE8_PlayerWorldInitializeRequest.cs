using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE8, -1, ePacketDirection.ClientToServer, "Player Wolrd initialize request")]
	public class CtoS_0xE8_PlayerWorldInializeRequest: Packet
	{
		protected ushort unk1;
		protected ushort unk2; // releated with CtoS_0xD4.Unk5 ?

		#region public access properties

		public ushort Unk1 { get { return unk1 ; } }
		public ushort Unk2 { get { return unk2 ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("unk1:0x{0:X4} unk2:0x{1:X4}", unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			unk2 = ReadShortLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xE8_PlayerWorldInializeRequest(int capacity) : base(capacity)
		{
		}
	}
}