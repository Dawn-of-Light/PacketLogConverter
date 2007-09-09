using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x6C, -1, ePacketDirection.ServerToClient, "Keep/Tower component overview")]
	public class StoC_0x6C_KeepComponentOverview : Packet, IObjectIdPacket
	{
		protected ushort keepId;
		protected ushort componentId;
		protected ushort unk1;
		protected ushort uid;
		protected byte skin;
		protected byte x;
		protected byte y;
		protected byte heading;
		protected byte height;
		protected byte health;
		protected byte status;
		protected byte flag;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { uid, keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort ComponentId { get { return componentId; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Uid { get { return uid; } }
		public byte Skin { get { return skin; } }
		public byte X { get { return x; } }
		public byte Y { get { return y; } }
		public byte Heading { get { return heading; } }
		public byte Height { get { return height; } }
		public byte Health { get { return health; } }
		public byte Status { get { return status; } }
		public byte Flag { get { return flag; } }

		#endregion

		public enum eKeepComponentType: byte
		{
			Gate = 0,
			WallInclined = 1,
			WallInclined2 = 2,
			WallAngle2 = 3,
			TowerAngle = 4,
			WallAngle = 5,
			WallAngleInternal = 6,
			TowerHalf = 7,
			WallHalfAngle = 8,
			Wall = 9,
			Keep = 10,
			Tower = 11,
			WallWithDoorLow = 12,
			WallWithDoorHigh = 13,
			BridgeHigh = 14,
			WallInclinedLow = 15,
			BridgeLow = 16,
			BridgeHightSolid = 17,
			BridgeHighWithHook = 18,
			GateFree = 19,
			BridgeHightWithHook2 = 20,
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} componentId:{1,-3} unk1:0x{2:X4} uid:0x{3:X4} wallSkinId:{4,-3} x:{5,-3} y:{6,-3} rotate:{7} height:{8} health:{9,3}% status:0x{10:X2} flag:0x{11:X2}",
				keepId, componentId, unk1, uid, skin, (sbyte)x, (sbyte)y, heading, height, health, status, flag);
			if (flagsDescription)
			{
				byte componentType = skin;
				if (componentType > 20)
					componentType -= 20;
				str.AppendFormat(" ({1}{0})", (eKeepComponentType)componentType, skin > 20 ? "New" : "");
			}
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort();
			componentId = ReadShort();
			unk1 = ReadShort();
			uid = ReadShort();
			skin = ReadByte();
			x = ReadByte();
			y = ReadByte();
			heading = ReadByte();
			height = ReadByte();
			health = ReadByte();
			status = ReadByte();
			flag = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x6C_KeepComponentOverview(int capacity) : base(capacity)
		{
		}
	}
}
