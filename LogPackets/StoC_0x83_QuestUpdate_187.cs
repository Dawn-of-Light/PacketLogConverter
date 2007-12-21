#define SKIP_CR_IN_DESCRIPTION
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 187, ePacketDirection.ServerToClient, "Quest update v187")]
	public class StoC_0x83_QuestUpdate_187 : StoC_0x83_QuestUpdate_186
	{
		protected override void InitQuestUpdate()
		{
			subData = new QuestUpdate_187();
			subData.Init(this);
		}

		public class QuestUpdate_187 : QuestUpdate_186
		{
			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("index:{0,-2} NameLen:{1,-3} descLen:{2,-3} rewards:{3} level:{4}", index, lenName, lenDesc, unk1, level);

				if (lenName == 0 && lenDesc == 0)
					return;
				str.AppendFormat("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);
			}
		}

		protected override void InitNewQuestUpdate()
		{
			subData = new NewQuestUpdate_187();
			subData.Init(this);
		}

		public class NewQuestUpdate_187 : NewQuestUpdate_186
		{
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				index = pak.ReadByte();
				lenName = pak.ReadByte();
				unk2 = pak.ReadShortLowEndian();
				goalsCount = pak.ReadByte();
				level = pak.ReadByte();
				name = pak.ReadString(lenName);
				lenDesc = pak.ReadByte();
				desc = pak.ReadString(lenDesc);
				goals = new string[goalsCount];
				goalInfo = new QuestGoalInfo[goalsCount];
				goalItems = new StoC_0x02_InventoryUpdate.Item[goalsCount];
				for (int i = 0; i < goalsCount; i++)
				{
					ushort goalDescLen = pak.ReadShortLowEndian();
#if SKIP_CR_IN_DESCRIPTION
					goals[i] = pak.ReadString(goalDescLen - 1); // 0x0A on end string
					pak.Skip(1);// skip 0x0A on end string
#else
					goals[i] = pak.ReadString(goalDescLen);
#endif
					QuestGoalInfo questGoalInfo = new QuestGoalInfo();
					questGoalInfo.zoneId2 = pak.ReadShortLowEndian();
					questGoalInfo.XOff2 = pak.ReadShortLowEndian();
					questGoalInfo.YOff2 = pak.ReadShortLowEndian();
					questGoalInfo.unk2 = pak.ReadShortLowEndian();
					questGoalInfo.type = pak.ReadShortLowEndian();
					questGoalInfo.unk1 = pak.ReadShortLowEndian();
					questGoalInfo.zoneId = pak.ReadShortLowEndian();
					questGoalInfo.XOff = pak.ReadShortLowEndian();
					questGoalInfo.YOff = pak.ReadShortLowEndian();
					goalInfo[i] = questGoalInfo;
					questGoalInfo.flagGoalFinished = pak.ReadByte();

					StoC_0x02_InventoryUpdate.Item item = new StoC_0x02_InventoryUpdate.Item();

					item.slot = pak.ReadByte();
					if (item.slot > 0)
					{
						item.level = pak.ReadByte();

						item.value1 = pak.ReadByte();
						item.value2 = pak.ReadByte();

						item.hand = pak.ReadByte();
						byte temp = pak.ReadByte(); //WriteByte((byte) ((item.Type_Damage*64) + item.Object_Type));
						item.damageType = (byte)(temp >> 6);
						item.objectType = (byte)(temp & 0x3F);
						item.weight = pak.ReadShort();
						item.condition = pak.ReadByte();
						item.durability = pak.ReadByte();
						item.quality = pak.ReadByte();
						item.bonus = pak.ReadByte();
						item.model = pak.ReadShort();
						item.extension = pak.ReadByte();
						item.color = pak.ReadShort();
						item.flag = pak.ReadByte();
						if ((item.flag & 0x08) == 0x08)
						{
							item.effectIcon = pak.ReadShort();
							item.effectName = pak.ReadPascalString();
						}
						if ((item.flag & 0x10) == 0x10)
						{
							item.effectIcon2 = pak.ReadShort();
							item.effectName2 = pak.ReadPascalString();
						}
						item.effect = pak.ReadByte();
						item.name = pak.ReadPascalString();
					}
					goalItems[i] = item;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("index:{0,-2} (NewQuest) NameLen:{1,-3} descLen:{2,-3} goals:{3} level:{4} unk2:{5}", index, lenName, lenDesc, goalsCount, level, unk2);

				if (lenName == 0 && lenDesc == 0)
					return;
				str.AppendFormat("\n\tQuestName: \"{0}\"\n\tQuestDesc: \"{1}\"", name, desc);
				for (int i = 0; i < goalsCount; i++)
				{
					str.AppendFormat("\n\t[{0}]: \"{1}\"", i, goals[i]);

					QuestGoalInfo questGoalInfo = goalInfo[i];
					str.AppendFormat("\n\tinfo: type:0x{0:X4} goalFinished:{1}",
						questGoalInfo.type, questGoalInfo.flagGoalFinished);
					str.AppendFormat("\n\tzoneId2:{0,-3} @X2:{1,-5} @Y2:{2,-5} radius?:{3}",
 						questGoalInfo.zoneId2, questGoalInfo.XOff2, questGoalInfo.YOff2, questGoalInfo.unk2);
					str.AppendFormat("\n\tzoneId1:{0,-3} @X1:{1,-5} @Y1:{2,-5} unk1:0x{3:X4}",
 						questGoalInfo.zoneId, questGoalInfo.XOff, questGoalInfo.YOff, questGoalInfo.unk1);
 					
					StoC_0x02_InventoryUpdate.Item item = goalItems[i];
					if (item.slot > 0)
					{
						str.AppendFormat("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} \"{17}\"",
							item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name);
						if (flagsDescription && item.name != null && item.name != "")
							str.AppendFormat(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
						if ((item.flag & 0x08) == 0x08)
							str.AppendFormat("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
							item.effectIcon, item.effectName);
						if ((item.flag & 0x10) == 0x10)
							str.AppendFormat("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
							item.effectIcon2, item.effectName2);
					}
				}
			}
		}
		/// <summary>
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x83_QuestUpdate_187(int capacity) : base(capacity)
		{
		}
	}
}