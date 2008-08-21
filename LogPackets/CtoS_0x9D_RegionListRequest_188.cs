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
				resolution = ReadShort();            // 0x00
				options = ReadShort();               // 0x02
				memory = ReadByte();                 // 0x04
				unk1 = ReadShort();                  // 0x05
				unk2 = ReadByte();                   // 0x07
				figureVersion = ReadInt();           // 0x08
				figureVersion1 = ReadByte();         // 0x0C
				skin = ReadByte();                   // 0x0D
				genderRace = ReadByte();             // 0x0E
				regionExpantions = ReadByte();       // 0x0F
				classId = ReadByte();                // 0x10
				expantions = ReadByte();             // 0x11
				VedioVendorId1 = ReadIntLowEndian(); // 0x12
				VedioVendorId2 = ReadIntLowEndian(); // 0x16
				osType = ReadByte();                 // 0x1A
				zero = ReadByte();                   // 0x1B
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