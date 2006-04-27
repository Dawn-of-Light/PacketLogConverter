using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x66, -1, ePacketDirection.ClientToServer, "Warmap bonuses request")]
	public class CtoS_0x66_WarmapBonuses: Packet
	{
		protected byte unk1;
		protected uint unk2;

		#region public access properties

		public byte Unk1 { get { return unk1 ; } }
		public uint Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X2} unk2:0x{1:X8}", unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadByte();
			unk2 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x66_WarmapBonuses(int capacity) : base(capacity)
		{
		}
	}
}