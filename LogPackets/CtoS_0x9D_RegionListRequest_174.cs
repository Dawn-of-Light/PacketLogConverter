using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 174, ePacketDirection.ClientToServer, "Region list request v174")]
	public class CtoS_0x9D_RegionListRequest_174 : CtoS_0x9D_RegionListRequest_172
	{
		protected byte classId;
		protected byte expantions;

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("dBslot:{0,-2} flagOption:{1}", slot, flag);
			if (flag > 0)
			{
				str.AppendFormat(" resolutions:0x{0:X4} options:0x{1:X4} figureVersion:0x{2:X8}{3:X2} memory:{4}({11,-2}) unk1:0x{5:X6} skin:0x{6:X2} unk2:0x{7:X2} regionExpantions:0x{8:X2} classId:{9,-2} expantions:0x{10:X2}",
					resolution, options, figureVersion, figureVersion1, unk1 >> 24, unk1 & 0xFFFFFF, skin, unk2, regionExpantions, classId, expantions, (unk1 >> 24) * 64);
				if (flagsDescription)
				{
					str.Append("\n\tExpantions:");
					if ((regionExpantions & 0x01) == 0x01)
						str.Append(", Foundations(Housing)");
					if ((regionExpantions & 0x02) == 0x02)
						str.Append(", NewFrontiers");
					if ((expantions & 0x04) == 0x04)
						str.Append(", Tutorial");
					if ((expantions & 0x10) == 0x10)
						str.Append(", NewTowns");
					if ((expantions & 0x20) == 0x20)
						str.Append(", Dark Rising?");
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
					if (skin == 0)
						description += ", Skin: Shrouded Isles";
					else if (skin == 3)
						description += ", Skin: Atlantis";
					else if (skin == 7)
						description += ", Skin: Custom";
					else
						description += ", Skin: ?" + skin.ToString();
					if ((options & 0x1000) == 0x1000)
						description += ", RuningSecondDaoc";
					// 0x8000 - Dynamic Shadow
					// 0x4000 - Reflective Water
					// 0x2000 - Shrouded Isles Water
					// 0x1000 - Flag running second copy DAOC ?
					// 0x0800 - WindowMode
					// 0x0400 - Classic Water
					// 0x0200 - Use Atlantis Terrain
					// 0x0100 - Use Atlantis Trees
					// 0x0080 - ?
					// 0x0040 - Font Size: Normal (else Small)
					// 0x0020 - Use Classic Name Font
					// 0x001F - ?
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
				figureVersion = ReadInt();
				figureVersion1 = ReadByte();
				skin = ReadByte();
				unk2 = ReadByte();
				regionExpantions = ReadByte();
				classId = ReadByte();
				expantions = ReadByte();
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_174(int capacity) : base(capacity)
		{
		}
	}
}