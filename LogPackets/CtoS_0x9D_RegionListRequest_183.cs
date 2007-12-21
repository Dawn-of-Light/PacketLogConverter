using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 183, ePacketDirection.ClientToServer, "Region list request v183")]
	public class CtoS_0x9D_RegionListRequest_183 : CtoS_0x9D_RegionListRequest_180
	{
		protected byte osType;

		public override string GetPacketDataString(bool flagsDescription)
		{
			string str = base.GetPacketDataString(flagsDescription);
			if (flag > 0)
			{
				str += " OS:" + osType.ToString("D");
				if(flagsDescription)
					str += "(" + (eOSType)osType + ")";
			}
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
				figureVersion = ReadInt();
				figureVersion1 = ReadByte();
				skin = ReadByte();
				genderRace = ReadByte();
				regionExpantions = ReadByte();
				classId = ReadByte();
				expantions = ReadByte();
				VedioVendorId1 = ReadIntLowEndian();
				VedioVendorId2 = ReadIntLowEndian();
				osType = ReadByte();
				zero = ReadByte();
				gender = 0;
				race = genderRace;
				if (genderRace > 18)
				{
					race = (byte)(genderRace - 18);
					gender = 1;
				}
			}
		}

		public enum eOSType : int
		{
			WIN95 = 1,
			WIN98 = 2,
			WindowsMe = 3,
			NT351 = 4,
			NT4 = 5,
			WIN2000 = 6,
			WINXP = 7,
			WIN2003 = 8,
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