using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7D, -1, ePacketDirection.ClientToServer, "Use spell list")]
	public class CtoS_0x7D_UseSpellList : Packet
	{
		protected ushort flagSpeedData;
		protected ushort heading;
		protected byte spellLevel;
		protected byte spellLineIndex;
		protected ushort unk1;

		#region public access properties

		public ushort FlagSpeedData { get { return flagSpeedData; } }
		public ushort Heading { get { return heading; } }
		public byte SpellLevel { get { return spellLevel; } }
		public byte SpellLineIndex { get { return spellLineIndex; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("flagSpeedData:0x{0:X4} heading:0x{1:X4} spellLevel:{2,-2} spellLineIndex:{3} unk1:0x{4:X4}", flagSpeedData, heading, spellLevel, spellLineIndex, unk1);

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
			spellLevel = ReadByte();
			spellLineIndex = ReadByte();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x7D_UseSpellList(int capacity) : base(capacity)
		{
		}
	}
}