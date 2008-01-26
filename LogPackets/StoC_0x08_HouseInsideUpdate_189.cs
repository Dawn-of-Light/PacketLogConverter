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

			houseOid = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			heading = ReadShort();
			bannerShield = ReadShort();
			emblem = ReadShort();
			unk1 = ReadShort();
			modelHouse = ReadByte();
			unk2 = ReadShort();
			unk3 = ReadByte();
			firstCarpet = ReadByte();
			secondCarpet = ReadByte();
			thirdCarpet = ReadByte();
			fourthCarpet = ReadByte();
			scheduled = ReadShortLowEndian();
			unk5 = ReadShort();
			unk4 = ReadByte();
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