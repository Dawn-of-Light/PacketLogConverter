using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 170, ePacketDirection.ServerToClient, "Quest update v170")]
	public class StoC_0x83_QuestUpdate_170 : StoC_0x83_QuestUpdate
	{
		protected override void InitQuestUpdate()
		{
			subData = new QuestUpdate_170();
			subData.Init(this);
		}

		public class QuestUpdate_170 : QuestUpdate
		{
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				lenName = pak.ReadByte();
				lenDesc = pak.ReadShort();
				if (lenName == 0 && lenDesc == 0)
				{
					name = "";
					desc = "";
				}
				else
				{
					name = pak.ReadString(lenName);
					desc = pak.ReadString(lenDesc);
				}
			}
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x83_QuestUpdate_170(int capacity) : base(capacity)
		{
		}
	}
}