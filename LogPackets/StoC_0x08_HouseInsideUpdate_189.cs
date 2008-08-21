using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x08, 189.1f, ePacketDirection.ServerToClient, "House inside update v189")] // not sure in what subversion it changed
	public class StoC_0x08_HouseInsideUpdate_189 : StoC_0x08_HouseInsideUpdate
	{
		protected ushort scheduled;
		protected ushort unk5;
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" unk5:0x{0:X4} scheduledHours:{1}", unk5, scheduled);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();         // 0x00
			z = ReadShort();                // 0x02
			x = ReadInt();                  // 0x04
			y = ReadInt();                  // 0x08
			heading = ReadShort();          // 0x0C
			bannerShield = ReadShort();     // 0x0E
			emblem = ReadShort();           // 0x10
			unk1 = ReadShort();             // 0x12
			modelHouse = ReadByte();        // 0x14
			unk2 = ReadShort();             // 0x15
			unk3 = ReadByte();              // 0x17
			firstCarpet = ReadByte();       // 0x18
			secondCarpet = ReadByte();      // 0x19
			thirdCarpet = ReadByte();       // 0x1A
			fourthCarpet = ReadByte();      // 0x1B
			scheduled = ReadShortLowEndian();//0x1C
			unk5 = ReadShort();             // 0x1E
			unk4 = ReadByte();              // 0x20
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x08_HouseInsideUpdate_189(int capacity) : base(capacity)
		{
		}
	}
}