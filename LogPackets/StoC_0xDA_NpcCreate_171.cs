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

		public override string GetPacketDataString(bool flagsDescription)
		{
			Position = 0;

			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} speed:{1,-4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} speedZ:{6, -4} model:0x{7:X4} size:{8,-3} level:{9,-3} flags:0x{10:X2} maxStick:{11,-3} flag2:0x{12:X2} unk1_171:0x{13:X2} unk2_171:0x{14:X4} name:\"{15}\" guild:\"{16}\" unk1:{17}",
			                 oid, speed, heading, x, y, z, speedZ, model, size, level & 0x7F, flags, maxStick, flag2, unk1_171, unk2_171, name, guildName, unk1);
			if (flagsDescription)
			{
				string flag = string.Format("realm:{0}",(flags >> 6) & 3);
				if ((flags & 0x01) == 0x01)
					flag += ",Ghost";
				if ((flags & 0x02) == 0x02)
					flag += ",Inventory";
				if ((flags & 0x04) == 0x04)
					flag += ",UNK_0x04";
				if ((flags & 0x08) == 0x08)
					flag += ",LongRangeVisible"; // ~5550
				if ((flags & 0x10) == 0x10)
					flag += ",Peace";
				if ((flags & 0x20) == 0x20)
					flag += ",Fly";
				if ((model & 0x8000) == 0x8000)
					flag += ",Underwater";
				if ((flag2 & 0x01) == 0x01)
					flag += ",-DOR";
				if ((flag2 & 0x02) == 0x02)
					flag += ",-NON";
				if ((flag2 & 0x04) == 0x04)
					flag += ",Stealth";
				if ((flag2 & 0x08) == 0x08)
					flag += ",Quest";
				if ((flag2 & 0x10) == 0x10)
					flag += ",F2_UNK_0x10";//waiting Finish new Quest ?
				if ((flag2 & 0x20) == 0x20)
					flag += ",F2_UNK_0x20";//mb see Underwater creature from water outside ?
				if ((flag2 & 0x40) == 0x40)
					flag += ",F2_UNK_0x40";
				if ((flag2 & 0x80) == 0x80)
					flag += ",HaveOwner";
				if ((level & 0x80) == 0x80)
					flag += ",Statue"; // can't breath. Not in debug mode can't target and not see name. in debug mode see name, can name, see -DOR
				str.AppendFormat(" ({0})", flag);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			speed = (short)ReadShort();
			heading = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			speedZ = (short)ReadShort();
			model = ReadShort();
			size = ReadByte();
			level = ReadByte();
			flags = ReadByte();
			maxStick = ReadByte();
			flag2 = ReadByte();
			unk1_171 = ReadByte();
			unk2_171 = ReadShort();
			name = ReadPascalString();
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