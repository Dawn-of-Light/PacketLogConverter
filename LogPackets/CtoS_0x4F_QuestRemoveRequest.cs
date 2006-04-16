using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4F, -1, ePacketDirection.ClientToServer, "Quest remove request")]
	public class CtoS_0x4F_QuestRemoveRequest: Packet
	{
		protected ushort questIndex;
		protected ushort unk1;
		protected ushort unk2;
		protected ushort unk3;

		#region public access properties

		public ushort QuestIndex { get { return questIndex; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("questIndex:{0} unk1:0x{1:X4} unk2:{2:X4} unk3:0x{3:X4}",
				questIndex, unk1, unk2, unk3);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			questIndex = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();
			unk3 = ReadShort();

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x4F_QuestRemoveRequest(int capacity) : base(capacity)
		{
		}
	}
}