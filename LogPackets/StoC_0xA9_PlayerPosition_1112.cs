using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA9, 1112, ePacketDirection.ServerToClient, "Player position update v1112")]
	public class StoC_0xA9_PlayerPosition_1112 : StoC_0xA9_PlayerPosition_190f
	{

		protected ushort unk1_1112;
		public ushort Unk1_1112 { get { return unk1_1112; } }

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" unk1_1112:0x{0:X4}", unk1_1112);
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
			manaPercent = ReadByte();
			endurancePercent = ReadByte();
			unk1_1112 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xA9_PlayerPosition_1112(int capacity) : base(capacity)
		{
		}
	}
}