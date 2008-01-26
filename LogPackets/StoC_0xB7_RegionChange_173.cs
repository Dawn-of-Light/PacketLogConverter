using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB7, 173, ePacketDirection.ServerToClient, "Region change v173")]
	public class StoC_0xB7_RegionChange_173: StoC_0xB7_RegionChange
	{
		protected ushort unk1_173;
		protected ushort unk2_173;

		#region public access properties

		public ushort Unk1_173 { get { return unk1_173; } }
		public ushort Unk2_173 { get { return unk2_173; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("regionId:{0,-3} zoneId:{1,-3} unk1_173:0x{2:X4} unk2_173:0x{3:X4}", regionId, zoneId, unk1_173, unk2_173);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			regionId = ReadShort();
			zoneId = ReadShort();
			unk1_173 = ReadShort();
			unk2_173 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xB7_RegionChange_173(int capacity) : base(capacity)
		{
		}
	}
}