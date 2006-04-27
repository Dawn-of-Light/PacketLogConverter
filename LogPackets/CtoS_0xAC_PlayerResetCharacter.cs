using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAC, -1, ePacketDirection.ClientToServer, "Reset Character")]
	public class CtoS_0xAC_PlayerResetCharacter : Packet
	{
		protected ushort sessionId;
		protected ushort unk1;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4} unk1:0x{1:X4}", sessionId, unk1);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			sessionId = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xAC_PlayerResetCharacter(int capacity) : base(capacity)
		{
		}
	}
}