using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB1, -1, ePacketDirection.ServerToClient, "Start Arena ")]
	public class StoC_0xB1_StartArena: Packet
	{
		protected byte unk1;
		protected byte region;
		protected string portFrom;
		protected string portTo;
		protected string serverIp;
		protected ushort unk3; // cluster ?
		protected ushort unk4; // cluster ?

		#region public access properties

		public byte Unk1 { get { return unk1; } }
		public byte Region { get { return region; } }
		public string PortFrom { get { return portFrom; } }
		public string PortTo { get { return portTo; } }
		public string ServerIP { get { return serverIp; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort Unk4 { get { return unk4; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{

			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X2} region:{1,-3} serverIp:\"{2}\" portFrom:{3} portTo:{4} unk3:0x{5:X4} un4:0x{6:X4}",
				unk1, region, serverIp, portFrom, portTo, unk3, unk4);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadByte();
			region = ReadByte();
			Skip(20); // unknown, always = 0
			portFrom = ReadString(5);
			portTo = ReadString(5);
			serverIp = ReadString(16);
			unk3 = ReadShort();
			unk4 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xB1_StartArena(int capacity) : base(capacity)
		{
		}
	}
}