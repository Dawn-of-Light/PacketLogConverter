using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAF, -1, ePacketDirection.ServerToClient, "msg")]
	public class StoC_0xAF_Message : Packet
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

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.Append("sid:0x").Append(sessionId.ToString("X4"));
			str.Append(" 0x").Append(type.ToString("X2"));

			string s = text;
			if (s.StartsWith("@@"))
			{
				str.Append(", chat");
				s = s.Substring(2);
			}
			else if (s.StartsWith("##"))
			{
				str.Append(",popup");
				s = s.Substring(2);
			}

			str.Append(": \"").Append(s).Append('"');
			if (flagsDescription)
				str.AppendFormat(" unk1:0x{0:X4} unk2:0x{1:X2} unk3:0x{2:X4}", unk1, unk2, unk3);

			return str.ToString();
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