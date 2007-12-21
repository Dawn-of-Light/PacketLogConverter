using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDD, -1, ePacketDirection.ClientToServer, "Player move item request")]
	public class CtoS_0xDD_PlayerMoveItemRequest : Packet, IObjectIdPacket, ISessionIdPacket
	{
		protected ushort sessionId;
		protected ushort toSlot;
		protected ushort fromSlot;
		protected ushort itemCount;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get
			{
				ushort fromOID = 0;
				ushort toOID = 0;
				if (ToSlot > 1000)
					toOID = (ushort)(ToSlot - 1000);
				if (FromSlot > 1000)
					fromOID = (ushort)(FromSlot - 1000);
				if ((toOID > 0) && (fromOID > 0))
				{
					return new ushort[] { toOID, fromOID };
				}
				else if (toOID > 0)
				{
					return new ushort[] { toOID };
				}
				else if (fromOID > 0)
				{
					return new ushort[] { fromOID };
				}
				return new ushort[] {};
			}
		}

		#region public access properties

		public ushort SessionId { get { return sessionId; } } // property + interface for SID
		public ushort ToSlot { get { return toSlot; } }
		public ushort FromSlot { get { return fromSlot; } }
		public ushort ItemCount { get { return itemCount; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} toSlot:{1,-4} fromSlot:{2,-4} itemCount:{3}", sessionId, toSlot, fromSlot, itemCount);
			if (flagsDescription)
			{
				if (toSlot > 1000)
					str.AppendFormat(" (toOid:0x{0:X4}", toSlot - 1000);
				if (fromSlot > 1000)
					str.AppendFormat(" (fromOid:0x{0:X4}", fromSlot - 1000);
			}
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