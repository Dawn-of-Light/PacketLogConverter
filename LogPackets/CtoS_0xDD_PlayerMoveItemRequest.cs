using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDD, -1, ePacketDirection.ClientToServer, "Player move item request")]
	public class CtoS_0xDD_PlayerMoveItemRequest : Packet
	{
		protected ushort sessionId;
		protected ushort toSlot;
		protected ushort fromSlot;
		protected ushort itemCount;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public ushort ToSlot { get { return toSlot; } }
		public ushort FromSlot { get { return fromSlot; } }
		public ushort ItemCount { get { return itemCount; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} toSlot:{1,-3} fromSlot:{2,-3} itemCount:{3}", sessionId, toSlot, fromSlot, itemCount);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			toSlot = ReadShort();
			fromSlot = ReadShort();
			itemCount = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xDD_PlayerMoveItemRequest(int capacity) : base(capacity)
		{
		}
	}
}