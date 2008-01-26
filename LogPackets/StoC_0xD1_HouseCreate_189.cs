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
			text.Write(" unk3:0x{0:X4} scheduledHours:{1}", unk3, scheduled);
			if (flagsDescription)
				if (scheduled > 0)
					text.Write(" ({0} days)", scheduled / 24);
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
			colorPorch = ReadShort();
			unk1 = ReadByte();
			modelPorch = ReadByte();
			emblem = ReadShort();
			scheduled = ReadShort();
			modelHouse= ReadByte();
			modelRoof = ReadByte();
			modelWall = ReadByte();
			modelDoor = ReadByte();
			modelTruss = ReadByte();
			materialPorch = ReadByte();
			materialShutter = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadShort();
			name = ReadPascalString();
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