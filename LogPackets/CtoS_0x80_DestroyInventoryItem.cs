using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x80, -1, ePacketDirection.ClientToServer, "Destroy inventory item")]
	public class CtoS_0x80_DestroyInventoryItem : Packet
	{
		protected ushort sessionId;
		protected ushort unk1;
		protected ushort slot;
		protected ushort unk2;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Slot { get { return slot; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} unk1:0x{1:X4} slot:{2,-3} unk2:0x{3:X4}",
				sessionId, unk1, slot, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			unk1 = ReadShort();
			slot = ReadShort();
			unk2 = ReadShort();

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x80_DestroyInventoryItem(int capacity) : base(capacity)
		{
		}
	}
}