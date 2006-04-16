using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x6C, -1, ePacketDirection.ServerToClient, "Keep/Tower component overview")]
	public class StoC_0x6C_KeepComponentOverview : Packet, IOidPacket
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

		public int Oid1 { get { return uid; } } // oid?
		public int Oid2 { get { return int.MinValue; } }

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

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} componentId:{1,-3} unk1:0x{2:X4} uid:0x{3:X4} wallSkinId:{4,-3} x:{5,-3} y:{6,-3} rotate:{7} height:{8} health:{9,3}% status:0x{10:X2} flag:0x{11:X2}",
				keepId, componentId, unk1, uid, skin, (sbyte)x, (sbyte)y, heading, height, health, status, flag);
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
