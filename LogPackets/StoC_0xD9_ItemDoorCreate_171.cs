using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD9, 171, ePacketDirection.ServerToClient, "Item/door create v171")]
	public class StoC_0xD9_ItemDoorCreate_171 : StoC_0xD9_ItemDoorCreate
	{
		protected byte staticFlag; // 1 byte +
		protected uint unk1_171; // 3 bytes

		#region public access properties

		public uint Unk1_171 { get { return unk1_171; } }
		public uint StaticFlag { get { return staticFlag; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} emblem:0x{1:X4} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} model:0x{6:X4} health:{7,3}% flags:0x{8:X2}(realm:{13}) extraBytes:{9} staticFlag:{10} unk1_171:0x{11:X6} name:\"{12}\"",
				oid, emblem, heading, x, y, z, model, hp, flags, extraBytes, staticFlag, unk1_171, name, (flags & 0x30)>>4);
			if (flagsDescription)
			{
				string flag = "";
				if ((flags & 0x01) == 0x01)
					flag += ",Underwater";// not let drop on ground ?
				if ((flags & 0x02) == 0x02)
					flag += ",UNK_0x02";
				if ((flags & 0x04) == 0x04)
					flag += ",Loot";
				if ((flags & 0x08) == 0x08)
					flag += ",StaticItem";//or Longrange ?
				// flag 0x10, 0x20 hold realm
				if ((flags & 0x40) == 0x40)// x = moving object oid, y = hookpoint
					flag += ",OnShipHookPoint";
				if ((flags & 0x80) == 0x80)
					flag += ",UNK_0x80";
				if ((staticFlag & 0x01) == 0x01)
					flag += ",-DOR";
				if ((staticFlag & 0x02) == 0x02)
					flag += ",Guild176Emblem";
				if ((staticFlag & 0xFC) > 0)
					flag += ",UNKNOWN_171_STATICFLAG";
				if (flag != "")
					text.Write(" ({0})", flag);
			}
			if (extraBytes == 4)
			{
				text.Write(" doorId:0x{0:X8}", internalId);
				if (flagsDescription)
				{
					uint doorType = internalId / 100000000;
					if (doorType == 7)
					{
						uint keepId = (internalId - 700000000) / 100000;
						uint keepPiece = (internalId - 700000000 - keepId * 100000) / 10000;
						uint componentId = (internalId - 700000000 - keepId * 100000 - keepPiece * 10000) / 100;
						int doorIndex = (int)(internalId - 700000000 - keepId * 100000 - keepPiece * 10000 - componentId * 100);
						text.Write(" (keepID:{0} componentId:{1} doorIndex:{2})", keepId + keepPiece * 256, componentId, doorIndex);
					}
					else if(doorType == 9)
					{
						doorType = internalId / 10000000;
						uint doorIndex = internalId - doorType * 10000000;
						text.Write(" (doorType:{0} houseDoorId:{1})", doorType, doorIndex);
					}
					else
					{
						int zoneDoor = (int)(internalId / 1000000);
						int fixture = (int)(internalId - zoneDoor * 1000000);
						int fixturePiece = fixture;
						fixture /= 100;
						fixturePiece = fixturePiece - fixture * 100;
						text.Write(" (zone:{0} fixture:{1} fixturePeace:{2})", zoneDoor, fixture, fixturePiece);
					}
				}
			}
			if (flagsDescription)
			{
//				uint guildLogo = (((unk1_171 & 0x2000000) >> 25) << 7) + (uint)(emblem >> 9);
				uint guildLogo = ((((uint)(staticFlag & 0x02) >> 1)) << 7) + (uint)(emblem >> 9);
				if (guildLogo != 0)
					text.Write(" guildLogo:{0,-3} pattern:{1} primaryColor:{2,-2} secondaryColor:{3}", guildLogo, (emblem >> 7) & 2, (emblem >> 3) & 0x0F, emblem & 7);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();          // 0x00
			emblem = ReadShort();       // 0x02
			heading = ReadShort();      // 0x04
			z = ReadShort();            // 0x06
			x = ReadInt();              // 0x08
			y = ReadInt();              // 0x0C
			model = ReadShort();        // 0x10
			hp = ReadByte();            // 0x12
			flags = ReadByte();         // 0x13
			uint tunk_171 = ReadInt();  // 0x14
			unk1_171 = tunk_171 & 0xFFFFFF;
			staticFlag = (byte)(tunk_171 >> 24); // 0x14
			name = ReadPascalString();  // 0x18
			extraBytes = ReadByte();    // ?
			if ((flags & 0x40) == 0x40)// x = moving object oid, y = hookpoint
				flagOnShipHookPoint = true;
			if (extraBytes == 4)
				internalId = ReadInt();
			else if (extraBytes != 0)
				throw new Exception("unknown extra bytes count.");
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD9_ItemDoorCreate_171(int capacity) : base(capacity)
		{
		}
	}
}