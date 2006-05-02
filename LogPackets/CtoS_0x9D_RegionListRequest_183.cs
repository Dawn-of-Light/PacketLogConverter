using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 183, ePacketDirection.ClientToServer, "Region list request v183")]
	public class CtoS_0x9D_RegionListRequest_183 : CtoS_0x9D_RegionListRequest_180
	{
		protected byte unkB;

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("dBslot:{0,-2} flagOption:{1}", slot, flag);
			if (flag > 0)
			{
				str.AppendFormat(" resolutions:0x{0:X4} options:0x{1:X4} figureVersion:0x{2:X8}{3:X2} classId:{10,-2} unk:0x{4:X8} 0x{5:X8} 0x{6:X2} 0x{7:X8} 0x{8:X8} 0x{9:X2}",
					resolution, options, unk2, unk3 >> 24, unk1, unk3 & 0xFFFFFF, unkS & 0xFF, unk4, unk5, unkB, unkS >> 8);
				if (flagsDescription)
				{
					string description = string.Format("\n\t{0}*{1}", (resolution >> 8) * 10, (resolution & 0xFF) * 10);
					if ((options & 0x800) == 0x800)
						description += " WindowMode";
					else
						description += " FullScreen";
					if ((options & 0x100) == 0x100)
						description += ", Use Atlantis Trees";
					if ((options & 0x200) != 0x200)
						description += ", Use Atlantis Terrain";
					if ((options & 0x8000) != 0x8000)
						description += ", Dynamic Shadow";
					if ((options & 0x6400) == 0x4400)
						description += ", Classic Water";
					else if ((options & 0x6400) == 0x2000)
						description += ", Shrouded Isles Water";
					else if ((options & 0x6400) == 0x4000)
						description += ", Reflective Water";
					if ((options & 0x20) == 0x20)
						description += ", Use Classic Name Font";
					if ((options & 0x40) == 0x40)
						description += ", Font Size: Normal";
					else
						description += ", Font Size: Small";
					byte skin = (byte)((unk3 >> 16) & 7);
					if (skin == 0)
						description += ", Skin: Shrouded Isles";
					else if (skin == 3)
						description += ", Skin: Atlantis";
					else if (skin == 7)
						description += ", Skin: Custom";
					str.Append(description);
				}
			}

			return str.ToString();
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
				unk1 = ReadInt();
				unk2 = ReadInt();
				unk3 = ReadInt();
				unkS = ReadShort();
				unk4 = ReadInt();
				unk5 = ReadInt();
				unkB = ReadByte();
				zero = ReadByte();
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