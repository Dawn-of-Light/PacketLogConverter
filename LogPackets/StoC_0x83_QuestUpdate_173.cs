using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 173, ePacketDirection.ServerToClient, "Quest update v173")]
	public class StoC_0x83_QuestUpdate_173 : StoC_0x83_QuestUpdate_170
	{
		protected override void InitQuestUpdate()
		{
			subData = new QuestUpdate_173();
			subData.Init(this);
		}

		public class QuestUpdate_173 : QuestUpdate_170
		{
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				index = pak.ReadByte();
				if (index == 0)
				{
					lenName = pak.ReadShortLowEndian();
					lenDesc = pak.ReadByte();
				}
				else
				{
					lenName = pak.ReadByte();
					lenDesc = pak.ReadShortLowEndian();
				}
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
		public StoC_0x83_QuestUpdate_173(int capacity) : base(capacity)
		{
		}
	}
}