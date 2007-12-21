using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x79, -1, ePacketDirection.ClientToServer, "Sell item")]
	public class CtoS_0x79_SellItem: Packet, ISessionIdPacket
	{
		protected uint playerX;
		protected uint playerY;
		protected ushort sessionId;
		protected ushort slot;

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public ushort SessionId { get { return sessionId; } }
		public ushort Slot { get { return slot; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4} playerX:{1,-6} playerY:{2,-6} slot:{3}",
				sessionId, playerX, playerY, slot);

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
			slot = ReadShort();
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