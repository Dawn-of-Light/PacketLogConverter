using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD8, 1112, ePacketDirection.ClientToServer, "Detail display request v1112")]
	public class CtoS_0xD8_DetailDisplayRequest_1112 : CtoS_0xD8_DetailDisplayRequest_186
	{
		protected uint unk1_1112;

		#region public access properties

		public uint Unk1_1112 { get { return unk1_1112; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" unk1_1112:0x{0:X8}", unk1_1112);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectType = ReadShort();
			unk1 = ReadInt();
			objectId = ReadShort();
			unk1_1112 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD8_DetailDisplayRequest_1112(int capacity) : base(capacity)
		{
		}
	}
}