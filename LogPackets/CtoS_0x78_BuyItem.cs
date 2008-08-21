using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x78, -1, ePacketDirection.ClientToServer, "Buy from merchant")]
	public class CtoS_0x78_BuyItem: Packet, ISessionIdPacket
	{
		protected uint playerX;
		protected uint playerY;
		protected ushort sessionId;
		protected ushort itemId;
		protected byte quantity;
		protected byte windowType; // currency ? =0(gold), 2(DF/seals, catacombs/aurulite)
#if !SKIPUNUSEDINPACKET
		protected ushort unk1;
#endif

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public ushort SessionId { get { return sessionId; } }
		public ushort ItemId { get { return itemId; } }
		public byte Quantity { get { return quantity; } }
		public byte WindowType { get { return windowType; } }
#if !SKIPUNUSEDINPACKET
		public ushort Unk1 { get { return unk1; } }
#endif

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			int page = itemId / 30;
			int slot = itemId - page * 30;
			text.Write("sessionId:0x{0:X4} playerX:{1,-6} playerY:{2,-6} itemId:0x{3:X4} (page:{4} slot:{5,2}) quantity:{6,-3}",
				sessionId, playerX, playerY, itemId, page, slot, quantity);
#if !SKIPUNUSEDINPACKET
			if (flagsDescription)
				text.Write(" unk1:0x{0:X4}", unk1);
#endif
			text.Write(" windowType:{0, -2}", windowType);
			if (flagsDescription)
				text.Write("({0})", (StoC_0x17_MerchantWindow.eMerchantWindowType)windowType);
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
			quantity = ReadByte();
			windowType = ReadByte();
#if !SKIPUNUSEDINPACKET
			unk1 = ReadShort();
#else
			Skip(2);
#endif
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x78_BuyItem(int capacity) : base(capacity)
		{
		}
	}
}