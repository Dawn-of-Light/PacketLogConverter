using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD1, 189.1f, ePacketDirection.ServerToClient, "House create v189")] // not sure in what subversion it changed
	public class StoC_0xD1_HouseCreate_189 : StoC_0xD1_HouseCreate
	{
		protected ushort scheduled;
		protected ushort unk3;

		#region public access properties

		public ushort Scheduled { get { return scheduled; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			base.GetPacketDataString(text, flagsDescription);
			text.Write(" scheduledHours:{0}", scheduled);
			if (flagsDescription)
			{
				if (scheduled > 0)
					text.Write(" ({0} days)", scheduled / 24);
				text.Write(" unk3:0x{0:X4}", unk3);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			houseOid = ReadShort();      // 0x00
			z = ReadShort();             // 0x02
			x = ReadInt();               // 0x04
			y = ReadInt();               // 0x08
			heading = ReadShort();       // 0x0C
			colorPorch = ReadShort();    // 0x0E
			unk1 = ReadByte();           // 0x10
			modelPorch = ReadByte();     // 0x11
			emblem = ReadShort();        // 0x12
			scheduled = ReadShort();     // 0x14
			modelHouse= ReadByte();      // 0x16
			modelRoof = ReadByte();      // 0x17
			modelWall = ReadByte();      // 0x18
			modelDoor = ReadByte();      // 0x19
			modelTruss = ReadByte();     // 0x1A
			materialPorch = ReadByte();  // 0x1B
			materialShutter = ReadByte();// 0x1C
			unk2 = ReadByte();           // 0x1D
			unk3 = ReadShort();          // 0x1E
			name = ReadPascalString();   // 0x20
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD1_HouseCreate_189(int capacity) : base(capacity)
		{
		}
	}
}