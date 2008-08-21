using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 183, ePacketDirection.ClientToServer, "Region list request v183")]
	public class CtoS_0x9D_RegionListRequest_183 : CtoS_0x9D_RegionListRequest_180
	{
		protected byte osType;

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			if (flag > 0)
			{
				text.Write(" OS:");
				text.Write(osType.ToString("D"));
				if(flagsDescription)
					text.Write("(" + (CtoS_0x37_ClientCrash.eOSType)osType + ")");
			}
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
				memory = ReadByte();
				unk1 = ReadShort();
				unk2 = ReadByte(); // unused
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

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_183(int capacity) : base(capacity)
		{
		}
	}
}