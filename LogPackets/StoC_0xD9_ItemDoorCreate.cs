using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD9, -1, ePacketDirection.ServerToClient, "Item/door create")]
	public class StoC_0xD9_ItemDoorCreate : Packet, IObjectIdPacket
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
		protected byte flagOnShipHookPoint;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get
			{
				if (flagOnShipHookPoint == 1)
					return new ushort[] { oid, (ushort)x };// x = moving object oid, y = hookpoint
				else
					return new ushort[] { oid };
			}
		}

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
		public byte FlagOnShipHookPoint { get { return flagOnShipHookPoint; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} emblem:0x{1:X4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} model:0x{6:X4} health:{7,3}% flags:0x{8:X2}(realm:{11}) extraBytes:{9} name:\"{10}\"",
				oid, emblem, heading, x, y, z, model, hp, flags, extraBytes, name, (flags & 0x30)>>4);
			if (flagsDescription)
			{
				string flag = "";
				if ((flags & 0x01) == 0x01)
					flag += ",Underwater";// not let drop on ground ?
				if ((flags & 0x02) == 0x02)
					flag += ",UNK_0x02";
				if ((flags & 0x04) == 0x04)
					flag += ",Loot";
				if ((flags & 0x08) == 0x08)
					flag += ",StaticItem";//or Longrange ?
				// flag 0x10, 0x20 hold realm
				if ((flags & 0x40) == 0x40)
					flag += ",OnShipHookPoint";// x = moving object oid, y = hookpoint
				if ((flags & 0x80) == 0x80)
					flag += ",UNK_0x80";
				if(flag != "")
					text.Write(" ({0})", flag);
			}
			if (extraBytes == 4)
			{
				text.Write(" doorId:0x{0:X4}", internalId);
				if (flagsDescription)
				{
					uint doorType = internalId / 100000000;
					if (doorType == 7)
					{
						uint keepId = (internalId - 700000000) / 100000;
						uint keepPiece = (internalId - 700000000 - keepId * 100000) / 10000;
						uint componentId = (internalId - 700000000 - keepId * 100000 - keepPiece * 10000) / 100;
						int doorIndex = (int)(internalId - 700000000 - keepId * 100000 - keepPiece * 10000 - componentId * 100);
						text.Write(" (keepID:{0} componentId:{1} doorIndex:{2})", keepId + keepPiece * 256, componentId, doorIndex);
					}
					else if(doorType == 9)
					{
						doorType = internalId / 10000000;
						uint doorIndex = internalId - doorType * 10000000;
						text.Write(" (doorType:{0} houseDoorId:{1})", doorType, doorIndex);
					}
					else
					{
						int zoneDoor = (int)(internalId / 1000000);
						int fixture = (int)(internalId - zoneDoor * 1000000);
						int fixturePiece = fixture;
						fixture /= 100;
						fixturePiece = fixturePiece - fixture * 100;
						text.Write(" (zone:{0} fixture:{1} fixturePeace:{2})", zoneDoor, fixture, fixturePiece);
					}
				}
			}
			if (flagsDescription)
			{
				int guildLogo = emblem >> 9;
				if (guildLogo != 0)
					text.Write(" guildLogo:{0,-3} pattern:{1} primaryColor:{2,-2} secondaryColor:{3}", guildLogo, (emblem >> 7) & 2, (emblem >> 3) & 0x0F, emblem & 7);
			}

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
			if ((flags & 0x40) == 0x40)
				flagOnShipHookPoint = 1;

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