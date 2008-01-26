using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x08, -1, ePacketDirection.ServerToClient, "House inside update")]
	public class StoC_0x08_HouseInsideUpdate : Packet, IHouseIdPacket
	{
		protected ushort houseOid;
		protected ushort z;
		protected uint x;
		protected uint y;
		protected ushort heading;
		protected ushort bannerShield;
		protected ushort emblem;
		protected ushort unk1;
		protected byte modelHouse;
		protected ushort unk2;
		protected byte unk3;
		protected byte firstCarpet;
		protected byte secondCarpet;
		protected byte thirdCarpet;
		protected byte fourthCarpet;
		protected byte unk4;

		#region public access properties

		public ushort HouseId { get { return houseOid; } }
		public ushort Z { get { return z; } }
		public uint X{ get { return x; } }
		public uint Y{ get { return y; } }
		public ushort Heading { get { return heading; } }
		public ushort BannerShield { get { return bannerShield; } }
		public ushort Emblem { get { return emblem; } }
		public ushort Unk1 { get { return unk1; } }
		public byte ModelHouse { get { return modelHouse; } }
		public byte FirstCarpet { get { return firstCarpet; } }
		public byte SecondCarpet { get { return secondCarpet; } }
		public byte ThirdCarpet { get { return thirdCarpet; } }
		public byte FourthCarpet { get { return fourthCarpet; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("houseOid:0x{0:X4} heading:0x{1:X4} x:{2,-6} y:{3,-6} z:{4,-5} banner&shield:0x{5:X2} emblem:0x{6:X4} unk1:0x{7:X4} modelHouse:{8} unk2:0x{9:X4} unk3:0x{10:X2} firstCarpet:{11,-3} secondCarpet:{12,-3} thirdCarpet:{13,-3} fourthCarpet:{14,-3} unk4:{15}",
				houseOid, heading, x, y, z, bannerShield, emblem, unk1, modelHouse, unk2, unk3, firstCarpet, secondCarpet, thirdCarpet, fourthCarpet, unk4);
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
			unk4 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x08_HouseInsideUpdate(int capacity) : base(capacity)
		{
		}
	}
}