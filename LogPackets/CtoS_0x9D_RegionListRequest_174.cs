using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 174, ePacketDirection.ClientToServer, "Region list request v174")]
	public class CtoS_0x9D_RegionListRequest_174 : CtoS_0x9D_RegionListRequest_172
	{
		protected ushort unkS;

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("dBslot:{0,-2} flag:{1}", slot, flag);
			if (flag > 0)
			{
				str.AppendFormat(" resolutions:0x{0:X4} options:0x{1:X4} unk:0x{2:X8} 0x{3:X8} 0x{4:X8} 0x{5:X4}",
					resolution, options, unk1, unk2, unk3, unkS);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			slot = ReadByte();
			flag = ReadByte();
			if (flag > 0)
			{
				resolution = ReadShort();
				options = ReadShort();
				unk1 = ReadInt();
				unk2 = ReadInt();
				unk3 = ReadInt();
				unkS = ReadShort();
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_174(int capacity) : base(capacity)
		{
		}
	}
}