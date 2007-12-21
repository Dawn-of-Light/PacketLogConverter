using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 188, ePacketDirection.ClientToServer, "Region list request v188")]
	public class CtoS_0x9D_RegionListRequest_188 : CtoS_0x9D_RegionListRequest_183
	{
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
				if (genderRace > 150)
				{
					race = (byte)(genderRace - 150);
					gender = 1;
				}
				else
				{
					race = (byte)(genderRace - 100);
					gender = 0;
				}
			}
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_188(int capacity) : base(capacity)
		{
		}
	}
}