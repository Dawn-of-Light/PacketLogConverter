using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1E, -1, ePacketDirection.ServerToClient, "Consignment merchant money")]
	public class StoC_0x1E_ConsignmentMerchantMoney: Packet
	{
		protected byte copper;
		protected byte silver;
		protected ushort gold;
		protected ushort mithril;
		protected ushort platinum;

		#region public access properties

		public byte Copper { get { return copper; } }
		public byte Silver { get { return silver; } }
		public ushort Gold { get { return gold; } }
		public ushort Mithril { get { return mithril; } }
		public ushort Platinum { get { return platinum; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("copper:{0,-2} silver:{1,-2} gold:{2,-3} mithril:{3,-3} platinum:{4}",
				copper, silver, gold, mithril, platinum);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			copper = ReadByte();    // 0x00
			silver = ReadByte();    // 0x01
			gold = ReadShort();     // 0x02
			mithril = ReadShort();  // 0x04
			platinum = ReadShort(); // 0x06
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x1E_ConsignmentMerchantMoney(int capacity) : base(capacity)
		{
		}
	}
}