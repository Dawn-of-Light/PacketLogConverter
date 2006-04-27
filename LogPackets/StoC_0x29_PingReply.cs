using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x29, -1, ePacketDirection.ServerToClient, "Ping reply")]
	public class StoC_0x29_PingReply : Packet
	{
		protected uint timeStamp;
		protected uint unk1;
		protected ushort sequence;
		protected uint unk2;
		protected ushort unk3;

		#region public access properties

		public uint TimeStamp { get { return timeStamp; } }
		public uint Unk1 { get { return unk1; } }
		public ushort Sequence { get { return sequence; } }
		public uint Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("timeStamp:0x{0:X8} sequence:0x{1:X4} unk1:0x{2:X8} unk2:0x{3:X8} unk4:0x{4:X4}",
				timeStamp, sequence, unk1, unk2, unk3);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			timeStamp = ReadInt();
			unk1 = ReadInt();
			sequence = ReadShort();
			unk2 = ReadInt();
			unk3 = ReadShort();
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