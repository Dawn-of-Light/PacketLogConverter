using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4D, -1, ePacketDirection.ServerToClient, "Spell message")]
	public class StoC_0x4D_SpellMessage_174: Packet, IOidPacket
	{
		protected ushort unk1;
		protected ushort sessionId;
		protected byte font;
		protected ushort unk2;
		protected ushort unk3;
		protected ushort unk4;
		protected ushort type;
		protected ushort flag;
		protected string message;
		protected ushort unk5;
		protected byte unk6;

		public int Oid1 { get { return sessionId; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort SessionId { get { return sessionId; } }
		public byte Font { get { return font ; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort Unk4 { get { return unk4; } }
		public ushort Type { get { return type; } }
		public ushort Flag { get { return flag; } }
		public string Message { get { return message; } }
		public ushort Unk5 { get { return unk5; } }
		public byte Unk6 { get { return unk6; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X4} sessionId:0x{1:X4} font:0x{2:X2} unk2:0x{3:X4} unk3:0x{4:X4} unk4:0x{5:X4} type:{6} flag:{7} msg:\"{8}\" unk5:0x{9:X4} unk6:0x{10:X2}",
			unk1, sessionId, font, unk2, unk3, unk4, type, flag, message, unk5, unk6);
			string pattern="UNKNOWN";
			switch (type)
			{
				case 1: pattern="{0} cast spell";break;
				case 2: pattern="You are already casting a spell! Your prepare this spell as followup!";break;
				case 3: pattern="You can't see your target from here!";break;
				case 4: pattern="That target is too far away!";break;
				case 5: pattern="You begin casting a {0} spell!";break;
				case 6: pattern="You regain {0} hit points from a magical font!";break;
				case 7: pattern="You regain {0} power from a magical font!";break;
				case 8: pattern="{0} is fully healed.";break;
				case 9: pattern="You don't have enough power to cast that!";break;
				case 10: pattern="You must wait {0} seconds to cast a spell!";break;
				case 11: pattern="You have too many pets in the area!";break;
				default:break;
			}
			if (flag!=0) pattern=string.Format(pattern,message);
			str.AppendFormat("\n\tmessage:\"{0}\"", pattern);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			message = "";
			unk1 = ReadShort();
			sessionId = ReadShort();
			font = ReadByte();
			unk2 = ReadShort();
			unk3 = ReadShort();
			unk4 = ReadShort();
			type = ReadByte();
			flag = ReadByte();
			if (flag!=0) message = ReadPascalString();
			unk5 = ReadShort();
			unk6 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4D_SpellMessage_174(int capacity) : base(capacity)
		{
		}
	}
}