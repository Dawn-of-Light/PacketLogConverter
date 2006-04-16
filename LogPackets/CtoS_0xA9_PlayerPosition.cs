using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, -1, ePacketDirection.ClientToServer, "Player position update")]
	public class CtoS_0xA9_PlayerPosition : Packet
	{
		protected ushort sessionId;
		protected ushort status;
		protected ushort currentZoneZ;
		protected ushort currentZoneX;
		protected ushort currentZoneY;
		protected ushort currentZoneId;
		protected byte unk1;
		protected ushort heading;
		protected ushort speed;
		protected byte flag;
		protected byte health;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Status { get { return status; } }
		public ushort CurrentZoneZ { get { return currentZoneZ; } }
		public ushort CurrentZoneX { get { return currentZoneX; } }
		public ushort CurrentZoneY { get { return currentZoneY; } }
		public ushort CurrentZoneId { get { return currentZoneId; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Heading { get { return heading; } }
		public ushort Speed { get { return speed; } }
		public byte Flag { get { return flag; } }
		public byte Health { get { return health; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} heading:0x{3:X4} currentZone({4,-3}): ({5,-6} {6,-6} {7,-4}) flyFlags:0x{8:X2} speed2:{9,-4} flags:0x{10:X2} health:{11,3}%{12}",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, heading, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, speed & 0x7FF, flag, health & 0x7F, ((health>>7)==1)?" combat":"");
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			status = ReadShort();
			currentZoneZ = ReadShort();
			currentZoneX = ReadShort();
			currentZoneY = ReadShort();
			currentZoneId= ReadByte();
			unk1 = ReadByte();
			heading = ReadShort();
			speed = ReadShort();
			flag = ReadByte();
			health = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA9_PlayerPosition(int capacity) : base(capacity)
		{
		}
	}
}