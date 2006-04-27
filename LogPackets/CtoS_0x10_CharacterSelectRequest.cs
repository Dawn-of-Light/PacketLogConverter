using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, -1, ePacketDirection.ClientToServer, "Character select request")]
	public class CtoS_0x10_CharacterSelectRequest : Packet
	{
		protected ushort sessionId;
		protected ushort unk1;
		protected string charName;
		protected string loginName;
		protected uint unk2;
		protected ushort port; // socket ?
		protected ushort unk3;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Unk1 { get { return unk1; } }
		public string CharName { get { return charName; } }
		public string LoginName { get { return loginName; } }
		public uint Unk2 { get { return unk2; } }
		public ushort Port { get { return port; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("charName:\"{0}\" sessionId:0x{1:X4} unk1:0x{2:X4} socket:{3} login:\"{4}\" unk2:0x{5:X8} unk3:0x{6:X4}",
				charName, sessionId, unk1, port, loginName, unk2, unk3);

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
			charName = ReadString(28);
			loginName = ReadString(48); // skip unknown
			unk2 = ReadInt();
			port = ReadShort();
			unk3 = ReadShort();

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x10_CharacterSelectRequest(int capacity) : base(capacity)
		{
		}
	}
}