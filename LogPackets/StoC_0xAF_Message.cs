using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAF, -1, ePacketDirection.ServerToClient, "msg")]
	public class StoC_0xAF_Message : Packet, ISessionIdPacket
	{
		protected ushort unk1;
		protected ushort sessionId;
		protected byte type;
		protected byte unk2;
		protected ushort unk3;
		protected string text;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort SessionId { get { return sessionId; } }
		public byte Type { get { return type; } }
		public byte Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public string Text { get { return text; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("sid:0x{0:X4} 0x{1:X2}", sessionId, type);

			if (flagsDescription)
				text.Write(" unk1:0x{0:X4} unk2:0x{1:X2} unk3:0x{2:X4}", unk1, unk2, unk3);
			string s = Text;
			if (s.StartsWith("@@"))
			{
				text.Write(", chat");
				s = s.Substring(2);
			}
			else if (s.StartsWith("##"))
			{
				text.Write(",popup");
				s = s.Substring(2);
			}

			text.Write(": \"{0}\"", s);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			sessionId = ReadShort();
			type = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadShort();
			text = ReadString(2048);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xAF_Message(int capacity) : base(capacity)
		{
		}
	}
}