using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA3, -1, ePacketDirection.ClientToServer, "Ping reply")]
	public class CtoS_0xA3_PingReply : Packet
	{
		protected ushort unk1;
		protected ushort unk2;
		protected int timeStamp;
		protected uint unk3;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }
		public int TimeStamp { get { return timeStamp; } }
		public uint Unk3 { get { return unk3; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("timeStamp:0x{0:X8} unk1:0x{1:X4} unk2:0x{2:X4} unk3:0x{3:X8}", timeStamp, unk1, unk2, unk3);
			if (flagsDescription)
			{
				TimeSpan timeUp = new TimeSpan((long) timeStamp * 1000); // TimeSpan in 100-nanosecond
				text.Write(" upTime:");
				text.Write(timeUp);
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			unk2 = ReadShort();
			timeStamp = (int)ReadInt();
			unk3 = ReadInt();
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