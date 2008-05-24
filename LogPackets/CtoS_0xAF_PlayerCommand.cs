using System.IO;
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
		protected short flag;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public string Command { get { return command; } }
		public short Flag { get { return flag; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("sid:0x{0:X4}", sessionId);
			if (flagsDescription)
				text.Write(" unk1:0x{0:X4} unk2:0x{1:X4} unk3:0x{2:X4}", unk1, unk2, unk3);
			text.Write(" \"{0}\"", command);
			if (flagsDescription && (flag >= 0))
				text.Write(" flag:0x{0:X2}", flag);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			flag = -1;

			sessionId = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();
			unk3 = ReadShort();
//			command = ReadString(255);
			command = ReadString();
			if (Position < Length)
				flag = ReadByte();
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