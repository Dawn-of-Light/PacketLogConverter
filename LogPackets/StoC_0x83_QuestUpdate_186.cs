#define SKIP_CR_IN_DESCRIPTION
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 186, ePacketDirection.ServerToClient, "Quest update v186")]
	public class StoC_0x83_QuestUpdate_186 : StoC_0x83_QuestUpdate_184
	{

		public NewQuestUpdate_186 InNewQuestUpdate		{ get { return subData as NewQuestUpdate_186; } }
	
		public override void Init()
		{
			Position = 0;
			byte index = ReadByte();
			subCode = 0;
			if (index > 0)
			{
				Skip(4);
				byte level = ReadByte();
				if (level > 0)
					subCode = 1;
			}
			Position = 0;
			InitSubcode(subCode);
		}

		protected override void InitQuestUpdate()
		{
			subData = new QuestUpdate_186();
			subData.Init(this);
		}

		public class QuestUpdate_186 : QuestUpdate_184
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
			subData = new NewQuestUpdate_186();
			subData.Init(this);
		}

		public class NewQuestUpdate_186 : NewQuestUpdate
		{
			public byte index;
			public ushort lenName;
			public ushort lenDesc;
			public byte level = 0;
			public byte goalsCount = 0;
			public string name;
			public string desc;
			public string[] goals;
			public QuestGoalInfo[] goalInfo;
			public Item[] rewards;

			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				index = pak.ReadByte();
				lenName = pak.ReadByte();
				lenDesc = pak.ReadShortLowEndian();
				goalsCount = pak.ReadByte();
				level = pak.ReadByte();
				name = pak.ReadString(lenName);
				lenDesc = pak.ReadByte();
				desc = pak.ReadString(lenDesc);
				goals = new string[goalsCount];
				goalInfo = new QuestGoalInfo[goalsCount];
				rewards = new Item[goalsCount];
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

					Item item = new Item();

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
					rewards[i] = item;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("index:{0,-2} (NewQuest) NameLen:{1,-3} descLen:{2,-3} goals:{3} level:{4}", index, lenName, lenDesc, goalsCount, level);

				if (lenName == 0 && lenDesc == 0)
					return;
				str.AppendFormat("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);
				for (int i = 0; i < goalsCount; i++)
				{
					str.AppendFormat("\n\t[{0}]: \"{1}\"", i, goals[i]);

					QuestGoalInfo questGoalInfo = goalInfo[i];
					str.AppendFormat("\n\tinfo: type:0x{0:X4} unk1:0x{1:X4}",
						questGoalInfo.type, questGoalInfo.unk1);
					str.AppendFormat("\n\tzoneId2:{0,-3} @X2:{1,-5} @Y2:{2,-5} radius?:{3}",
 						questGoalInfo.zoneId2, questGoalInfo.XOff2, questGoalInfo.YOff2, questGoalInfo.unk2);
					str.AppendFormat("\n\tzoneId1:{0,-3} @X1:{1,-5} @Y1:{2}",
 						questGoalInfo.zoneId, questGoalInfo.XOff, questGoalInfo.YOff);

					Item item = rewards[i];

					str.AppendFormat("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} \"{17}\"",
						item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name);
					if ((item.flag & 0x08) == 0x08)
						str.AppendFormat("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
						item.effectIcon, item.effectName);
					if ((item.flag & 0x10) == 0x10)
						str.AppendFormat("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
						item.effectIcon2, item.effectName2);
				}
			}
		}

		public class Item
		{
			public byte slot;
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte temp;
			public byte damageType;
			public byte objectType;
			public ushort weight;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public ushort color;
			public byte effect;
			public byte flag;
			public string name;
			public byte extension; // new in 1.72
			public ushort effectIcon; // new 1.82
			public string effectName; // new 1.82
			public ushort effectIcon2; // new 1.82
			public string effectName2; // new 1.82
		}

		public class QuestGoalInfo
		{
			public ushort zoneId2;
			public ushort XOff2;
			public ushort YOff2;
			public ushort unk2;
			public ushort type;
			public ushort unk1;
			public ushort zoneId;
			public ushort XOff;
			public ushort YOff;
			public byte unk_187;//goals finished flag ?
		}

		/// <summary>
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x83_QuestUpdate_186(int capacity) : base(capacity)
		{
		}
	}
}