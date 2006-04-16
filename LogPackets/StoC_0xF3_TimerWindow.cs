using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF3, -1, ePacketDirection.ServerToClient, "Timer window")]
	public class StoC_0xF3_TimerWindow : Packet
	{
		protected ushort timer;
		protected byte messageLength;
		protected byte flag;
		protected string message;
		protected byte zero;

		#region public access properties

		public ushort Timer { get { return timer; } }
		public byte MessageLength { get { return messageLength; } }
		public byte Flag { get { return flag; } }
		public string Message { get { return message; } }
		public byte Zero { get { return zero; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			
			str.AppendFormat("timer:0x{0:X4} length:{1} flag:{2}{3}",
				timer, messageLength, flag, flag != 0 ? " title:\""+message+"\"" : message);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			timer = ReadShort();
			messageLength = ReadByte();
			flag = ReadByte();
			message = "";
			if (flag != 0)
			{
				message = ReadString(messageLength);
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF3_TimerWindow(int capacity) : base(capacity)
		{
		}
	}
}