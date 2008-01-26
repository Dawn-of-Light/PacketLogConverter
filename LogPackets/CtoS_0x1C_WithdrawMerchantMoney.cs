using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1C, -1, ePacketDirection.ClientToServer, "Withdraw consignment merchant money")]
	public class CtoS_0x1C_WithdrawMerchantMoney: Packet
	{
		protected ushort unk1;
		protected ushort unk2; // same as CtoS_0xE8 unk2

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:0x{0:X4} unk2:0x{1:X4}", unk1, unk2);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x1C_WithdrawMerchantMoney(int capacity) : base(capacity)
		{
		}
	}
}