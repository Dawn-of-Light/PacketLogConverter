using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x12, -1, ePacketDirection.ServerToClient, "Create moving object")]
	public class StoC_0x12_CreateMovingObject : Packet, IObjectIdPacket
	{
		protected ushort objectOid;
		protected ushort unk1;
		protected ushort heading;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort model;
		protected ushort flags;
		protected ushort emblem;
		protected ushort unk2;
		protected ushort unk3;
		protected ushort unk4;
		protected string name;
		protected byte unk5; // Trailing 0 ?

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { objectOid }; }
		}

		#region public access properties

		public ushort ObjectOid { get { return objectOid; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Heading { get { return heading; } }
		public ushort Z { get { return z; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public ushort Model { get { return model; } }
		public ushort Flags { get { return flags; } }
		public ushort Emblem { get { return emblem; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort Unk4 { get { return unk4; } }
		public string Name { get { return name; } }
		public byte Unk5 { get { return unk5; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat(" oid:0x{0:X4} unk1:0x{1:X4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} model:0x{6:X4} flags:0x{7:X4} emblem:0x{8:X4} unk2-4:0x{9:X4}{10:X4}{11:X4} name:\"{12}\" unk5:{13}",
				objectOid, unk1, heading, x, y, z, model, flags, emblem, unk2, unk3, unk4, name, unk5/*, (flags >> 10) & 7, (flags >> 4) & 7, flags & 0xE38F*/);
			if (flagsDescription)
			{
				str.AppendFormat(" (level:{0,2} realm:{1})", flags >> 9, ((flags & 0x40) == 0x40 ? 3 : (flags >> 4) & 3));
				if (emblem != 0)
				  str.AppendFormat(" logo:{0,-3} pattern:{1} primaryColor:{2,-2} secondaryColor:{3}", ((unk4 & 1) << 7) | (emblem >> 9), (emblem >> 7) & 2, (emblem >> 3) & 0x0F, emblem & 7);
			}
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectOid = ReadShort();
			unk1 = ReadShort();
			heading = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			model = ReadShort();
			flags = ReadShort();
			emblem = ReadShort();
			unk2 = ReadShort();
			unk3 = ReadShort();
			unk4 = ReadShort();
			name = ReadPascalString();
			unk5 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x12_CreateMovingObject(int capacity) : base(capacity)
		{
		}
	}
}