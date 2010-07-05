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

		public enum eChatType : byte
		{
			CT_System = 0x00,
			CT_Say = 0x01,
			CT_Send = 0x02,
			CT_Group = 0x03,
			CT_Guild = 0x04,
			CT_Broadcast = 0x05,
			CT_Emote = 0x06,
			CT_Help = 0x07,
			CT_Friend = 0x08,
			CT_Advise = 0x09,
			CT_Officer = 0x0a,
			CT_Alliance = 0x0b,
			CT_BattleGroup = 0x0c,
			CT_BattleGroupLeader = 0x0d,
			CT_Staff = 0xf,

			CT_Spell = 0x10,
			CT_YouHit = 0x11,
			CT_YouWereHit = 0x12,
			CT_Skill = 0x13,
			CT_Merchant = 0x14,
			CT_YouDied = 0x15,
			CT_PlayerDied = 0x16,
			CT_OthersCombat = 0x17,
			CT_ResistChanged = 0x18,
			CT_SpellExpires = 0x19,
			CT_Loot = 0x1a,
			CT_SpellResisted = 0x1b,
			CT_Important = 0x1c,
			CT_Damaged = 0x1d,
			CT_Missed = 0x1e,
			CT_SpellPulse = 0x1f,
			CT_KilledByAlb = 0x20,
			CT_KilledByMid = 0x21,
			CT_KilledByHib = 0x22,
			CT_SocialInterface = 0x64,
			CT_ScreenCenter = 0xC8,
			CT_ScreenCenterSmaller = 0xC9,
		};
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("sid:0x{0:X4} 0x{1:X2}", sessionId, type);

			if (flagsDescription)
			{
				text.Write("({0})", (eChatType)type);
				text.Write(" unk1:0x{0:X4} unk2:0x{1:X2} unk3:0x{2:X4}", unk1, unk2, unk3);
			}
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