using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDA, 171, ePacketDirection.ServerToClient, "Npc create v171")]
	public class StoC_0xDA_NpcCreate_171 : StoC_0xDA_NpcCreate
	{
		protected byte flag2;
		protected byte unk1_171;
		protected ushort unk2_171;

		#region public access properties
		public byte Flag2 {get {return flag2; } }
		public byte Unk1_171 { get { return unk1_171; } }
		public ushort Unk2_171 { get { return unk2_171; } }

		public byte Statue {get {return (byte)((level & 0x80) == 0x80 ? 1 : 0); } }
		public byte Flag0x04 {get {return (byte)((flags & 0x04) == 0x04 ? 1 : 0); } }
		public byte LongRangeVisible {get {return (byte)((flags & 0x08) == 0x08 ? 1 : 0); } }
		public byte HaveOwner {get {return (byte)((flag2 & 0x80) == 0x80 ? 1 : 0); } }// 1.87+

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			Position = 0;
			text.Write("oid:0x{0:X4} speed:{1,-4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} speedZ:{6, -4} model:0x{7:X4} size:{8,-3}({18,-3}) level:{9,-3} flags:0x{10:X2} maxStick:{11,-3} flag2:0x{12:X2} unk1_171:0x{13:X2} unk2_171:0x{14:X4} name:\"{15}\" guild:\"{16}\" unk1:{17}",
			                 oid, speed, heading, x, y, z, speedZ, model, size, level & 0x7F, flags, maxStick, flag2, unk1_171, unk2_171, name, guildName, unk1, size * 2);
			if (flagsDescription)
			{
				text.Write(" (realm:{0}", (flags >> 6) & 3);
				if ((flags & 0x01) == 0x01)
					text.Write(",Ghost");
				if ((flags & 0x02) == 0x02)
					text.Write(",Inventory");
				if ((flags & 0x04) == 0x04)
					text.Write(",UNK_0x04");
				if ((flags & 0x08) == 0x08)
					text.Write(",LongRangeVisible"); // 4000, 5500, 8000
				if ((flags & 0x10) == 0x10)
					text.Write(",Peace");
				if ((flags & 0x20) == 0x20)
					text.Write(",Fly");
				if ((model & 0x8000) == 0x8000)
					text.Write(",Underwater");
				if ((flag2 & 0x01) == 0x01)
					text.Write(",-DOR");
				if ((flag2 & 0x02) == 0x02)
					text.Write(",-NON");
				if ((flag2 & 0x04) == 0x04)
					text.Write(",Stealth");
				if ((flag2 & 0x08) == 0x08)
					text.Write(",Quest");
				if ((flag2 & 0x10) == 0x10)
					text.Write(",FinishQuest");//waiting Finish new Quest ?
				if ((flag2 & 0x20) == 0x20)
					text.Write(",WaterMob");//mb see Underwater creature from water outside ?
				if ((flag2 & 0x40) == 0x40)
					text.Write(",F2_UNK_0x40");
				if ((flag2 & 0x80) == 0x80)
					text.Write(",HaveOwner");
				if ((level & 0x80) == 0x80)
					text.Write(",Statue"); // can't breath. Not in debug mode can't target and not see name. in debug mode see name, can name, see -DOR
				text.Write(')');
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();           // 0x00
			speed = (short)ReadShort();  // 0x02
			heading = ReadShort();       // 0x04
			z = ReadShort();             // 0x06
			x = ReadInt();               // 0x08
			y = ReadInt();               // 0x0C
			speedZ = (short)ReadShort(); // 0x10
			model = ReadShort();         // 0x12
			size = ReadByte();           // 0x14
			level = ReadByte();          // 0x15
			flags = ReadByte();          // 0x16
			maxStick = ReadByte();       // 0x17
			flag2 = ReadByte();          // 0x18
			unk1_171 = ReadByte();       // 0x19
			unk2_171 = ReadShort();      // 0x1A
			name = ReadPascalString();   // 0x1C+
			guildName = ReadPascalString();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDA_NpcCreate_171(int capacity) : base(capacity)
		{
		}
	}
}