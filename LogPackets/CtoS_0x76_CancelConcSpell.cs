using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x76, -1, ePacketDirection.ClientToServer, "Cancel concentration spell")]
	public class CtoS_0x76_CancelConcSpell: Packet
	{
		protected byte spellIndex; // maybe reverse int ?
		protected byte unk1;
		protected ushort unk2;

		#region public access properties

		public byte SpellIndex { get { return spellIndex; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("spellIndex:{0} unk1:{1} unk2:{2}", spellIndex, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			spellIndex = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x76_CancelConcSpell(int capacity) : base(capacity)
		{
		}
	}
}