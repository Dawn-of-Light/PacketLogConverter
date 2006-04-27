using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB6, -1, ePacketDirection.ServerToClient, "Max speed")]
	public class StoC_0xB6_UpdateMaxSpeed : Packet
	{
		protected ushort maxSpeedPercent;
		protected byte turningEnabled;
		protected byte unk1;

		#region public access properties

		public ushort MaxSpeedPercent { get { return maxSpeedPercent; } }
		public byte TurningEnabled { get { return turningEnabled; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("{0,3}% turning:{1} unk1:{2}", maxSpeedPercent, turningEnabled, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			maxSpeedPercent = ReadShort();
			turningEnabled = ReadByte();
			unk1 = ReadByte();
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