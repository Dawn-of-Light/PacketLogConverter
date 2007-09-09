using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB1, -1, ePacketDirection.ServerToClient, "Start Arena ")]
	public class StoC_0xB1_StartArena: Packet
	{
		protected byte regionIndex;
		protected byte region;
		protected string portFrom;
		protected string portTo;
		protected string serverIp;
		protected string zoneInfo; // 184+ part string (zone + " " + XOffset)

		#region public access properties

		public byte RegionIndex { get { return regionIndex; } }
		public byte Region { get { return region; } }
		public string PortFrom { get { return portFrom; } }
		public string PortTo { get { return portTo; } }
		public string ServerIP { get { return serverIp; } }
		public string ZoneInfo{ get { return zoneInfo; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{

			StringBuilder str = new StringBuilder();

			str.AppendFormat("regionIndex:0x{0:X2} region:{1,-3} serverIp:\"{2}\" portFrom:{3} portTo:{4}",
				 regionIndex, region, serverIp, portFrom, portTo);
			if (flagsDescription)
				str.AppendFormat(" zoneInfo:\"{0}\"", zoneInfo);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			regionIndex = ReadByte();
			region = ReadByte();
			Skip(20); // unknown, always = 0
			portFrom = ReadString(5);
			portTo = ReadString(5);
			long curPosition = Position;
			serverIp = ReadString(16);
			Position = curPosition + serverIp.Length + 1;
			zoneInfo = ReadString(20 - serverIp.Length);
//			zoneInfo = ReadString(4);
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