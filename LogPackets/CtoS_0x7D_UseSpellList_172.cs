using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7D, 172, ePacketDirection.ClientToServer, "Use spell list v172")]
	public class CtoS_0x7D_UseSpellList_172 : CtoS_0x7D_UseSpellList
	{
		protected ushort xOffsetInZone;
		protected ushort yOffsetInZone;
		protected ushort currentZoneId;
		protected ushort realZ;

		#region public access properties

		public ushort XOffsetInZone { get { return xOffsetInZone; } }
		public ushort YOffsetInZone { get { return yOffsetInZone; } }
		public ushort CurrentZoneId { get { return currentZoneId; } }
		public ushort RealZ { get { return realZ; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("flagSpeedData:0x{0:X4} heading:0x{1:X4} spellLevel:{2,-2} spellLineIndex:{3} playerZonePos({4,3}):({5,-6} {6,-6} {7,-5}) unk1:0x{8:X4})",
				flagSpeedData, heading, spellLevel, spellLineIndex, currentZoneId, xOffsetInZone, yOffsetInZone, realZ, unk1);
			if (flagsDescription)
			{
				str.AppendFormat(" (speed:{0}{1}", (flagSpeedData & 0x200) == 0x200 ? "-" : "", flagSpeedData & 0x1FF);
				if ((flagSpeedData & 0x400) == 0x400)
					str.Append(",UNKx0400");
				if ((flagSpeedData & 0x800) == 0x800)
					str.Append(",PetInView");
				if ((flagSpeedData & 0x1000) == 0x1000)
					str.Append(",GTinView");
				if ((flagSpeedData & 0x2000) == 0x2000)
					str.Append(",CheckTargetInView");
				if ((flagSpeedData & 0x4000) == 0x4000)
					str.Append(",Strafe");// Swim under water
				if ((flagSpeedData & 0x8000) == 0x8000)
					str.Append(",TargetInView");
				str.AppendFormat(")");
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flagSpeedData = ReadShort();
			heading = ReadShort();
			xOffsetInZone = ReadShort();
			yOffsetInZone = ReadShort();
			currentZoneId = ReadShort();
			realZ = ReadShort();
			spellLevel = ReadByte();
			spellLineIndex = ReadByte();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x7D_UseSpellList_172(int capacity) : base(capacity)
		{
		}
	}
}