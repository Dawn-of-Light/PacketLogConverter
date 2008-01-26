using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xED, -1, ePacketDirection.ClientToServer, "Craft item")]
	public class CtoS_0xED_CraftItem : Packet
	{
		protected ushort receptId;
		protected ushort unk1;

		#region public access properties

		public ushort ReceptId { get { return receptId; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("receptId:{0} unk1:0x{1:X4}", receptId, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			receptId = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xED_CraftItem(int capacity) : base(capacity)
		{
		}
	}
}