using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB7, -1, ePacketDirection.ServerToClient, "Region change")]
	public class StoC_0xB7_RegionChange: Packet
	{
		protected ushort regionId;
		protected ushort zoneId;
		protected byte unk1;

		#region public access properties

		public ushort RegionId { get { return regionId; } }
		public ushort ZoneId { get { return zoneId; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("regionId:{0,-3} zoneId:{1,-3} unk1:0x{2:X2}", regionId, zoneId, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			regionId = ReadShort();
			zoneId = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xB7_RegionChange(int capacity) : base(capacity)
		{
		}
	}
}