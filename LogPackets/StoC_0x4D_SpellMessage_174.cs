using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4D, -1, ePacketDirection.ServerToClient, "Spell message")]
	public class StoC_0x4D_SpellMessage_174: Packet, ISessionIdPacket
	{
		protected ushort unk1;
		protected ushort sessionId;
		protected byte font;
		protected ushort unk2;
		protected ushort unk3;
		protected ushort unk4;
		protected byte type;
		protected byte flag;
		protected string message;
		protected ushort unk5;
		protected byte unk6;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort SessionId { get { return sessionId; } }
		public byte Font { get { return font ; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort Unk4 { get { return unk4; } }
		public byte Type { get { return type; } }
		public byte Flag { get { return flag; } }
		public string Message { get { return message; } }
		public ushort Unk5 { get { return unk5; } }
		public byte Unk6 { get { return unk6; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("sessionId:0x{0:X4} font:0x{1:X2} type:{2,-2} flag:{3} msg:\"{4}\"",
				sessionId, font, type, flag, message);
			if (flagsDescription)
			{
				text.Write("\n\tunk1:0x{0:X4} unk2:0x{1:X4} unk3:0x{2:X4} unk4:0x{3:X4} unk5:0x{4:X4} unk6:0x{5:X2}",
				unk1, unk2, unk3, unk4, unk5, unk6);
			}
			string pattern="UNKNOWN";
			switch (type)
			{
				case 1: pattern="{0} casts a spell";break;
				case 2: pattern="You are already casting a spell! You prepare this spell as a followup!";break;
				case 3: pattern="You can't see your target from here!";break;
				case 4: pattern="That target is too far away!";break;
				case 5: pattern="You begin casting a {0} spell!";break;
				case 6: pattern="You regain {0} hit points from a magical font!";break;
				case 7: pattern="You regain {0} power from a magical font!";break;
				case 8: pattern="{0} is fully healed.";break;
				case 9: pattern="You don't have enough power to cast that!";break;
				case 10: pattern="You must wait {0} seconds to cast a spell!";break;
				case 11: pattern="You have too many pets in the area!";break;
				case 12: pattern="You gain an additional {0} for adventuring in this area!";break;
				// 194+
				case 13: pattern="Congratulations on reaching level 10. As a Trial account user, you will not be able to progress further in the game until you have upgraded to a full account. Please visit http://www.camelotherald.c";break;
				case 14: pattern="You feel very well rested.";break;
				case 15: pattern="You feel well rested.";break;
				case 16: pattern="You feel rested.";break;
				case 17: pattern="You are a bit tired.";break;
				case 18: pattern="You gain {0} extra experience for being rested";break;
				case 19: pattern="You earn {0} bonus experience for fighting close to an outpost!";break;
				case 20: pattern="You earn {0} bonus experience from realm outpost ownership!";break;
				case 21: pattern="You earn {0} bonus experience from your guild merit bonus!";break;
				case 22: pattern="You earn {0} bonus experience from your artifact!\n";break;
				case 23: pattern="You gain {0} bonus experience.";break;
				case 24: pattern="You earn {0} bonus experience for having a level 50 on your account.";break;
				case 25: pattern="You earn {0} bonus champion experience.";break;
				case 26: pattern="You earn {0} champion experience.";break;
				case 27: pattern="You have earned enough champion experience for champion level {0}!";break;
				case 28: pattern="You have earned as much Champion Level Experience as you can for this Champion Level. Please speak with your realm's King.";break;
				case 29: pattern="Your AFK flag is now off.";break;
				case 30: pattern="Your AFK flag is now on.";break;
				case 31: pattern="You get {0} bounty points!";break;
				case 32: pattern="You feel more rested.";break;
				case 33: pattern="You feel less rested.";break;
				case 34: pattern="<str>";break;
				case 35: pattern="You gain {0} bonus experience for adventuring in this area";break;
				default:break;
			}
			if (flag!=0) pattern=string.Format(pattern,message);
			text.Write("\n\tmessage:\"{0}\"", pattern);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			message = "";
			unk1 = ReadShort();      // 0x00
			sessionId = ReadShort(); // 0x02
			font = ReadByte();       // 0x04
			unk2 = ReadShort();      // 0x05
			unk3 = ReadShort();      // 0x07
			unk4 = ReadShort();      // 0x09
			type = ReadByte();       // 0x0B
			flag = ReadByte();       // 0x0C
			if (flag!=0) message = ReadPascalString(); // 0x0D+
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