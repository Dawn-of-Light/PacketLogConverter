using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x08, 189, ePacketDirection.ServerToClient, "House inside update v189")]
	public class StoC_0x08_HouseInsideUpdate_189 : StoC_0x08_HouseInsideUpdate
	{
		protected uint unk5;

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.Append(base.GetPacketDataString(flagsDescription));
			str.AppendFormat(" unk5:0x{0:X8}", unk5);

			return str.ToString();
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
			unk5 = ReadInt();
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