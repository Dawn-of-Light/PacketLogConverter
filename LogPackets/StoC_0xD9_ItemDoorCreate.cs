using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD9, -1, ePacketDirection.ServerToClient, "Item/door create")]
	public class StoC_0xD9_ItemDoorCreate : Packet, IOidPacket
	{
		protected ushort oid;
		protected ushort emblem;
		protected ushort heading;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort model;
		protected byte hp;
		protected byte flags;
		protected string name;
		protected byte extraBytes;
		protected uint internalId;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort Emblem { get { return emblem; } }
		public ushort Heading { get { return heading; } }
		public ushort Z { get { return z; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public ushort Model { get { return model; } }
		public byte HP { get { return hp; } }
		public byte Flags { get { return flags; } }
		public string Name { get { return name; } }
		public byte ExtraBytes { get { return extraBytes; } }
		public uint InternalId { get { return internalId; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} emblem:0x{1:X4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} model:0x{6:X4} health:{7,3}% flags:0x{8:X2}(realm:{11}) extraBytes:{9} name:\"{10}\"",
				oid, emblem, heading, x, y, z, model, hp, flags, extraBytes, name, (flags & 0x30)>>4);
			if (extraBytes == 4)
				str.AppendFormat(" internalId:0x{0:X4}", internalId);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			emblem = ReadShort();
			heading = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			model = ReadShort();
			hp = ReadByte();
			flags = ReadByte();
			name = ReadPascalString();
			extraBytes = ReadByte();

			if (extraBytes == 4)
				internalId = ReadInt();
			else if (extraBytes != 0)
				throw new Exception("unknown extra bytes count.");
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD9_ItemDoorCreate(int capacity) : base(capacity)
		{
		}
	}
}