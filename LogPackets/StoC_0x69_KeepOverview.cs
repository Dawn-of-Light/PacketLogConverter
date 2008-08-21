using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x69, -1, ePacketDirection.ServerToClient, "Keep/Tower overview")]
	public class StoC_0x69_KeepOverview : Packet, IKeepIdPacket
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
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keepId:0x{0:X4} x:{1,-6} y:{2,-6} heading:0x{3:X4} realm:{4} level:{5,-2} model:0x{6:X2} unk1:{7} unk2:{8} unk3:0x{9:X2}",
				keepId, keepX, keepY, heading, realm, level, model, unk1, unk2, unk3);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort(); // 0x00
			unk1 = ReadShort();   // 0x02
			keepX = ReadInt();    // 0x04
			keepY = ReadInt();    // 0x08
			heading = ReadShort();// 0x0C
			realm = ReadByte();   // 0x0E
			level = ReadByte();   // 0x0F
			unk2 = ReadShort();   // 0x10
			model = ReadByte();   // 0x12
			unk3 = ReadByte();    // 0x13
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