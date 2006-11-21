using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDA, 171, ePacketDirection.ServerToClient, "Npc create v171")]
	public class StoC_0xDA_NpcCreate_171 : StoC_0xDA_NpcCreate
	{
		protected byte flag2;
		protected ushort unk1_171;
		protected byte instance;

		#region public access properties
		public byte Flag2 {get {return flag2; } }
		public ushort Unk1_171 { get { return unk1_171; } }
		public byte Instance {get {return instance; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			Position = 0;

			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} speed:{1,-4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} speedZ:{6, -4} model:0x{7:X4} size:{8,-3} level:{9,-3} flags:0x{10:X2} maxStick:{11,-3} flag2:0x{12:X2} unk1_171:0x{13:X4} instance:0x{14:X2} name:\"{15}\" guild:\"{16}\" unk1:{17}",
			                 oid, speed, heading, x, y, z, speedZ, model, size, level, flags, maxStick, flag2, unk1_171, instance, name, guildName, unk1);
			if (flagsDescription)
			{
				string flag = string.Format("realm:{0}",(flags >> 6) & 3);
				if ((flags & 0x01) == 0x01)
					flag += ",Ghost";
				if ((flags & 0x02) == 0x02)
					flag += ",Inventory";
				if ((flags & 0x04) == 0x04)
					flag += ",UNKx04";
				if ((flags & 0x08) == 0x08)
					flag += ",UNKx08";
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
			unk1_171 = ReadShort();
			instance = ReadByte();
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