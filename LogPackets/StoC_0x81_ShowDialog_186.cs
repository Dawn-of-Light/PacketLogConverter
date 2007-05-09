#define SKIP_CR_IN_DESCRIPTION
using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x81, 186, ePacketDirection.ServerToClient, "Show dialog v186")]
	public class StoC_0x81_ShowDialog_186 : StoC_0x81_ShowDialog
	{
		public NewQuestUpdate_186 InNewQuestUpdate		{ get { return subData as NewQuestUpdate_186; } }
		protected override void InitNewQuestUpdate()
		{
			subData = new NewQuestUpdate_186();
			subData.Init(this);
		}

		public class NewQuestUpdate_186: NewQuestUpdate
		{
			public string questName;
			public string questDesc;
			public ushort dialogLen;
			public string message;
			public ushort questID;
			public byte goalsCount;
			public string[] goals;
			public byte questLevel;
			public byte rewardGold;
			public byte rewardExp;
			public ushort itemsRewardType;
			public byte rewardsCount;
			public Item[] rewards;

			public enum eItemsRewardType: byte
			{
				Basic = 0,
				Optional = 1,
			}

			public override void Init(StoC_0x81_ShowDialog pak)
			{
				questName = pak.ReadPascalString();
				questDesc = pak.ReadPascalString();
				dialogLen = pak.ReadShort();
				message = pak.ReadString(dialogLen);
				questID = pak.ReadShort();
				goalsCount = pak.ReadByte();
				goals = new string[goalsCount];
				for (int i = 0; i < goalsCount; i++)
				{
#if SKIP_CR_IN_DESCRIPTION
					questLevel = pak.ReadByte(); // temporary used
					goals[i] = pak.ReadString(questLevel - 1); // 0x0A on end string
					pak.Skip(1);// skip 0x0A on end string
#else
					goals[i] = pak.ReadPascalString();
#endif
				}
				questLevel = pak.ReadByte();
				rewardGold = pak.ReadByte();
				rewardExp = pak.ReadByte();
				itemsRewardType = pak.ReadShort();
				if (itemsRewardType > 1)
					pak.Position -=2;
				rewardsCount = pak.ReadByte();
				rewards = new Item[rewardsCount];
				for (int i = 0; i < rewardsCount; i++)
				{
					Item item = new Item();
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
					rewards[i] = item;
				}
				if (itemsRewardType > 1)
					itemsRewardType = pak.ReadShort();
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\n\tQuestName: \"{0}\"\n\tQuestDesc: \"{1}\"", questName, questDesc);
				str.AppendFormat("\n\tlen:{0} message:\"{1}\"", dialogLen, message);
				str.AppendFormat("\n\tquestID:0x{0:X4} goalsCount:{1}", questID, goalsCount);
				for (int i = 0; i < goalsCount; i++)
				{
					str.AppendFormat("\n\t[{0}]: \"{1}\"", i, goals[i]);
				}
				str.AppendFormat("\n\trewardLevel:{0} gold:{1}% Exp:{2:00.0}% itemsRewardType:0x{3:X4}({5}) itemsRewardCount:{4}", questLevel, rewardGold, rewardExp, itemsRewardType, rewardsCount, (eItemsRewardType)itemsRewardType);
				for (int i = 0; i < rewardsCount; i++)
				{

					Item item = rewards[i];

					str.AppendFormat("\n\t[{0}]: level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} \"{17}\"",
						i, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name);
					if ((item.flag & 0x08) == 0x08)
						str.AppendFormat("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
						item.effectIcon, item.effectName);
					if ((item.flag & 0x10) == 0x10)
						str.AppendFormat("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
						item.effectIcon2, item.effectName2);
				}
			}
		}

		public struct Item
		{
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
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x81_ShowDialog_186(int capacity) : base(capacity)
		{
		}
	}
}