using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x41, -1, ePacketDirection.ClientToServer, "Use NewQuest item")]
	public class CtoS_0x41_UseNewQuestItem: Packet
	{
		protected byte questIndex;
		protected byte goalIndex;

		#region public access properties

		public byte QuestIndex { get { return questIndex; } }
		public byte GoalIndex { get { return goalIndex; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("questIndex:{0,-2} goal:{1}", questIndex, goalIndex);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			questIndex = ReadByte();
			goalIndex = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x41_UseNewQuestItem(int capacity) : base(capacity)
		{
		}
	}
}