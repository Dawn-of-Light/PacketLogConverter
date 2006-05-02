using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 180, ePacketDirection.ClientToServer, "Region list request v180")]
	public class CtoS_0x9D_RegionListRequest_180 : CtoS_0x9D_RegionListRequest_174
	{
		protected uint unk4;
		protected uint unk5;

		public override string GetPacketDataString(bool flagsDescription)
		{
			return base.GetPacketDataString(flagsDescription) + "\n\tnew in 1.80 0x" + unk4.ToString("X8") + " 0x" + unk5.ToString("X8");
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
				unk4 = ReadInt();
				unk5 = ReadInt();
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_180(int capacity) : base(capacity)
		{
		}
	}
}