using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 172, ePacketDirection.ClientToServer, "Region list request v172")]
	public class CtoS_0x9D_RegionListRequest_172 : CtoS_0x9D_RegionListRequest
	{
		protected byte slot;
		protected ushort resolution;
		protected ushort options;
		protected uint unk1;
		protected uint figureVersion;
		protected byte figureVersion1;
		protected byte skin;
		protected byte race;
		protected byte regionExpantions;
		protected byte zero;

		#region public access properties

		public byte Slot { get { return slot; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("dBslot:{0,-2} flagOption:{1}", slot, flag);
			if (flag > 0)
			{
				str.AppendFormat(" resolutions:0x{0:X4} options:0x{1:X4} figureVersion:0x{2:X8}{3:X2} memory:{4,2}({9,-2}) unk1:0x{5:X6} skin:0x{6:X2} race:{7,-2} regionExpantions:0x{8:X2}",
					resolution, options, figureVersion, figureVersion1, unk1 >> 24, unk1 & 0xFFFFFF, skin, race > 18 ? 18 - race : race, regionExpantions, (unk1 >> 24) * 64);
				if (flagsDescription)
				{
					str.Append("\n\tExpantions:");
//					str.Append(", Shrouded Isles");
//					str.Append(", Trials of Atlantis");
//					str.Append(", Catacombs");
					if ((regionExpantions & 0x01) == 0x01)
						str.Append(", Foundations(Housing)");
					if ((regionExpantions & 0x02) == 0x02)
						str.Append(", NewFrontiers");
					string description = string.Format("\n\t{0}*{1}", (resolution >> 8) * 10, (resolution & 0xFF) * 10);
					if ((options & 0x800) == 0x800)
						description += " WindowMode";
					else
						description += " FullScreen";
					if ((options & 0x100) == 0x100)
						description += ", Use Atlantis Trees"; //TreeFlag
					if ((options & 0x200) != 0x200)
						description += ", Use Atlantis Terrain"; // OldTerrainFlag
					if ((options & 0x8000) != 0x8000)
						description += ", Dynamic Shadow"; // OldShadowFlag or Shadow ?
					if ((options & 0x6400) == 0x4400)
						description += ", Classic Water";  // OldRiverFlag
					else if ((options & 0x6400) == 0x2000)
						description += ", Shrouded Isles Water"; // Water1
					else if ((options & 0x6400) == 0x4000) // Water2
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
				race = ReadByte();
				regionExpantions = ReadByte();
				zero = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_172(int capacity) : base(capacity)
		{
		}
	}
}