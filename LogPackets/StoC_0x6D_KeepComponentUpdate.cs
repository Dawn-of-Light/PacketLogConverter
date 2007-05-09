using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x6D, -1, ePacketDirection.ServerToClient, "Keep/Tower component update")]
	public class StoC_0x6D_KeepComponentUpdate : Packet, IObjectIdPacket
	{
		private ushort keepId;
		private ushort componentId;
		private byte height;
		private byte health;
		private byte status;
		private byte flags;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort ComponentId { get { return componentId; } }
		public byte Height { get { return height; } }
		public byte Health { get { return health; } }
		public byte Status { get { return status; } }
		public byte Flags { get { return flags; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} componentId:{1,-3} height:{2,-3} health:{3,3}% status:0x{4:X2} flags:0x{5:X2}",
				keepId, componentId, height, health, status, flags);
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
			height = ReadByte();
			health = ReadByte();
			status = ReadByte();
			flags = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x6D_KeepComponentUpdate(int capacity) : base(capacity)
		{
		}
	}
}