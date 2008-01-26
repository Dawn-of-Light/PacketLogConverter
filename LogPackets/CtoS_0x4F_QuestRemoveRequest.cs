using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4F, -1, ePacketDirection.ClientToServer, "Quest remove request")]
	public class CtoS_0x4F_QuestRemoveRequest: Packet
	{
		protected ushort responce;
		protected ushort questSlot;// quest index in 0x83 as -1 (becouse first quest slot = 1 (0 slot = task))
		protected ushort unk1;
		protected ushort unk2;

		#region public access properties

		public ushort Responce { get { return responce; } }
		public ushort QuestSlot { get { return questSlot; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("responce?:{0} questSlot:{1,-2} unk1:0x{2:X4} unk2:0x{3:X4}",
				responce ,questSlot, unk1, unk2);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			responce = ReadShort();
			questSlot = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x4F_QuestRemoveRequest(int capacity) : base(capacity)
		{
		}
	}
}