using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA3, -1, ePacketDirection.ClientToServer, "Ping reply")]
	public class CtoS_0xA3_PingReply : Packet
	{
		protected uint unk1;
		protected int timeStamp;
		protected uint unk2;

		#region public access properties

		public uint Unk1 { get { return unk1; } }
		public int TimeStamp { get { return timeStamp; } }
		public uint Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("timeStamp:0x{0:X8} unk1:0x{1:X8} unk2:0x{2:X8}", timeStamp, unk1, unk2);
			if (flagsDescription)
			{
				TimeSpan timeUp = new TimeSpan((long) timeStamp * 1000); // TimeSpan in 100-nanosecond
				str.AppendFormat(" upTime:{0}", timeUp);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadInt();
			timeStamp = (int)ReadInt();
			unk2 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA3_PingReply(int capacity) : base(capacity)
		{
		}
	}
}