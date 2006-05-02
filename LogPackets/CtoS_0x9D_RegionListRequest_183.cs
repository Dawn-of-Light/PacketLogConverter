using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 183, ePacketDirection.ClientToServer, "Region list request v183")]
	public class CtoS_0x9D_RegionListRequest_183 : CtoS_0x9D_RegionListRequest_180
	{
		protected byte unkB;

		public override string GetPacketDataString(bool flagsDescription)
		{
			string str = base.GetPacketDataString(flagsDescription);
			if (flag > 0)
				str += " 0x" + unkB.ToString("X2");
			return str;
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
				unkB = ReadByte();
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_183(int capacity) : base(capacity)
		{
		}
	}
}