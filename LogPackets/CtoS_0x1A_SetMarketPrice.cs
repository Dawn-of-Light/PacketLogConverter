using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1A, -1, ePacketDirection.ClientToServer, "Set market price")]
	public class CtoS_0x1A_SetMarketPrice: Packet
	{
		protected byte slot;
		protected byte unk1;
		protected ushort unk2;
		protected uint price;

		#region public access properties

		public byte Slot { get { return slot; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }
		public uint Price { get { return price; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("slot:{0,2} price:{1,-7} unk1:0x{2:X2} unk2:0x{3:X4}", slot, price, unk1, unk2);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			slot = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
			price = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x1A_SetMarketPrice(int capacity) : base(capacity)
		{
		}
	}
}