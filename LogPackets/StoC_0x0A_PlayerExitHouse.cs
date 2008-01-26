using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x0A, -1, ePacketDirection.ServerToClient, "Player exit house")]
	public class StoC_0x0A_ExitHouse: Packet, IHouseIdPacket
	{
		protected ushort houseOid;
		protected ushort unk1;

		#region public access properties

		public ushort HouseId { get { return houseOid; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("houseOid:0x{0:X4} unk1:0x{1:X4}", houseOid, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x0A_ExitHouse(int capacity) : base(capacity)
		{
		}
	}
}