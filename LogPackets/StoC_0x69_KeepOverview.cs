using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x69, -1, ePacketDirection.ServerToClient, "Keep/Tower overview")]
	public class StoC_0x69_KeepOverview : Packet, IObjectIdPacket
	{
		protected ushort keepId;
		protected ushort unk1;
		protected uint keepX;
		protected uint keepY;
		protected ushort heading;
		protected byte realm;
		protected byte level;
		protected ushort unk2;
		protected byte model;
		protected byte unk3;

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
		public ushort Unk1 { get { return unk1; } }
		public uint KeepX { get { return keepX; } }
		public uint KeepY { get { return keepY; } }
		public ushort Heading { get { return heading; } }
		public byte Realm { get { return realm; } }
		public byte Level { get { return level; } }
		public ushort Unk2 { get { return unk2; } }
		public byte Model { get { return model; } }
		public byte Unk3 { get { return unk3; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} x:{1,-6} y:{2,-6} heading:0x{3:X4} realm:{4} level:{5,-2} model:0x{6:X2} unk1:{7} unk2:{8} unk3:0x{9:X2}",
				keepId, keepX, keepY, heading, realm, level, model, unk1, unk2, unk3);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort();
			unk1 = ReadShort();
			keepX = ReadInt();
			keepY = ReadInt();
			heading = ReadShort();
			realm = ReadByte();
			level = ReadByte();
			unk2 = ReadShort();
			model = ReadByte();
			unk3 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x69_KeepOverview(int capacity) : base(capacity)
		{
		}
	}
}