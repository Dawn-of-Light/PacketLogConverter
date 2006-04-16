using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 172, ePacketDirection.ClientToServer, "Player position update v172")]
	public class CtoS_0xA9_PlayerPosition_172 : CtoS_0xA9_PlayerPosition
	{

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} status:0x{1:X2} speed:{2,-3} heading:0x{3:X4}(0x{13:X2}) currentZone({4,-3}): ({5,-6} {6,-6} {7,-4}) flyFlags:0x{8:X2} speed2:{9,-4} flags:0x{10:X2} health:{11,3}%{12}",
				sessionId, (status & 0x1FF ^ status) >> 8 ,status & 0x1FF, heading & 0xFFF, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, (speed & 0x7FF ^ speed) >> 8, speed & 0x7FF, flag, health & 0x7F, ((health>>7)==1)?" combat":"", (heading & 0xFFF ^ heading) >> 8);
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
			currentZoneId= ReadShort();
			heading = ReadShort();
			speed = ReadShort();
			flag = ReadByte();
			health = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA9_PlayerPosition_172(int capacity) : base(capacity)
		{
		}
	}
}