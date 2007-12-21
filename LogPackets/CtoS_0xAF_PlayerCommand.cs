using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAF, -1, ePacketDirection.ClientToServer, "cmd")]
	public class CtoS_0xAF_PlayerCommand : Packet, ISessionIdPacket
	{
		protected ushort sessionId;
		protected ushort unk1;
		protected ushort unk2;
		protected ushort unk3;
		protected string command;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public string Command { get { return command; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sid:0x{0:X4} \"{1}\" unk1:0x{2:X4} unk2:0x{3:X4} unk3:0x{4:X4}", sessionId, command, unk1, unk2, unk3);

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
			unk2 = ReadShort();
			unk3 = ReadShort();
			command = ReadString(255);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xAF_PlayerCommand(int capacity) : base(capacity)
		{
		}
	}
}