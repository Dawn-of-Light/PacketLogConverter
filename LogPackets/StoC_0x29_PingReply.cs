using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x29, -1, ePacketDirection.ServerToClient, "Ping reply")]
	public class StoC_0x29_PingReply : Packet
	{
		protected uint timeStamp;
		protected uint unk1;
		protected ushort sequence;
		protected ushort unk2;
		protected uint unk3;

		#region public access properties

		public uint TimeStamp { get { return timeStamp; } }
		public uint Unk1 { get { return unk1; } }
		public ushort Sequence { get { return sequence; } }
		public ushort Unk2 { get { return unk2; } }
		public uint Unk3 { get { return unk3; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("timeStamp:0x{0:X8} sequence:0x{1:X4} unk1:0x{2:X8} unk2:0x{3:X4} unk3:0x{4:X8}",
				timeStamp, sequence, unk1, unk2, unk3);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			timeStamp = ReadInt(); // 0x00
			unk1 = ReadInt();      // 0x04
			sequence = ReadShort();// 0x08
			unk2 = ReadShort();    // 0x0A
			unk3 = ReadInt();      // 0x0C
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x29_PingReply(int capacity) : base(capacity)
		{
		}
	}
}