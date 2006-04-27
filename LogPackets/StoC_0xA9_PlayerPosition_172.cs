using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 172, ePacketDirection.ServerToClient, "Player position update v172")]
	public class StoC_0xA9_PlayerPosition_172 : StoC_0xA9_PlayerPosition
	{

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
		public StoC_0xA9_PlayerPosition_172(int capacity) : base(capacity)
		{
		}
	}
}