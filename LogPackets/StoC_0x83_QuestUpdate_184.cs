using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 184, ePacketDirection.ServerToClient, "Quest update v184")]
	public class StoC_0x83_QuestUpdate_184 : StoC_0x83_QuestUpdate_183
	{
		protected override void InitQuestUpdate()
		{
			subData = new QuestUpdate_184();
			subData.Init(this);
		}

		public class QuestUpdate_184 : QuestUpdate_183
		{
			public byte level;
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				index = pak.ReadByte();
				if (index == 0)
				{
					lenName = pak.ReadShortLowEndian();
					lenDesc = pak.ReadByte();
					unk1 = pak.ReadByte();
				}
				else
				{
					lenName = pak.ReadByte();
					lenDesc = pak.ReadShortLowEndian();
					unk1 = pak.ReadByte();
				}
				level = pak.ReadByte();
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

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("index:{0,-2} NameLen:{1,-3} descLen:{2,-3} unk1:{3} zone?:{4}", index, lenName, lenDesc, unk1, level);

				if (lenName == 0 && lenDesc == 0)
					return;
				str.AppendFormat("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);
			}
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x83_QuestUpdate_184(int capacity) : base(capacity)
		{
		}
	}
}