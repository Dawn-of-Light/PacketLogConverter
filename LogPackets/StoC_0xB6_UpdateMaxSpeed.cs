using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB6, -1, ePacketDirection.ServerToClient, "Max speed")]
	public class StoC_0xB6_UpdateMaxSpeed : Packet
	{
		protected ushort maxSpeedPercent;
		protected byte turningEnabled;
		protected byte waterMaxSpeedPercent;

		#region public access properties

		public ushort MaxSpeedPercent { get { return maxSpeedPercent; } }
		public byte TurningEnabled { get { return turningEnabled; } }
		public byte WaterMaxSpeedPercent { get { return waterMaxSpeedPercent; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("{0,3}% turning:{1} waterSpeed:{2}", maxSpeedPercent, turningEnabled, waterMaxSpeedPercent);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			maxSpeedPercent = ReadShort();    // 0x00
			turningEnabled = ReadByte();      // 0x02
			waterMaxSpeedPercent = ReadByte();// 0x03
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xB6_UpdateMaxSpeed(int capacity) : base(capacity)
		{
		}
	}
}