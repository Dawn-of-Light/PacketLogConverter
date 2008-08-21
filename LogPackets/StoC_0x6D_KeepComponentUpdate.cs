using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x6D, -1, ePacketDirection.ServerToClient, "Keep/Tower component update")]
	public class StoC_0x6D_KeepComponentUpdate : Packet, IKeepIdPacket
	{
		private ushort keepId;
		private ushort componentId;
		private byte height;
		private byte health;
		private byte status;
		private byte flags;

		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keepId:0x{0:X4} componentId:{1,-2} height:{2,-3} health:{3,3}% status:0x{4:X2} flags:0x{5:X2}",
				keepId, componentId, height, health, status, flags);
			if (flagsDescription)
			{
				if (status > 0)
					text.Write(" ({0})", (StoC_0x6C_KeepComponentOverview.eKeepComponentStatus)status);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort();     // 0x00
			componentId = ReadShort();// 0x02
			height = ReadByte();      // 0x04
			health = ReadByte();      // 0x05
			status = ReadByte();      // 0x06
			flags = ReadByte();       // 0x07
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