using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x79, -1, ePacketDirection.ClientToServer, "Sell item")]
	public class CtoS_0x79_SellItem: Packet, IOidPacket
	{
		protected uint playerX;
		protected uint playerY;
		protected ushort sessionId;
		protected ushort itemId;

		public int Oid1 { get { return sessionId; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public ushort SessionId { get { return sessionId; } }
		public ushort ItemId { get { return itemId; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4} playerX:{1,-6} playerY:{2,-6} itemId:0x{3:X4}",
				sessionId, playerX, playerY, itemId);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerX = ReadInt();
			playerY = ReadInt();
			sessionId = ReadShort();
			itemId = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x79_SellItem(int capacity) : base(capacity)
		{
		}
	}
}