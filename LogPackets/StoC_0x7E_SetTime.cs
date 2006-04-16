using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7E, -1, ePacketDirection.ServerToClient, "Set time")]
	public class StoC_0x7E_SetTime : Packet
	{
		protected uint currentTime;
		protected uint dayIncrement;

		#region public access properties

		public uint CurrentTime { get { return currentTime; } }
		public uint DayIncrement { get { return dayIncrement; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("currentTime:0x{0:X8} dayIncrement:{1}", currentTime, dayIncrement);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			currentTime = ReadInt();
			dayIncrement = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7E_SetTime(int capacity) : base(capacity)
		{
		}
	}
}