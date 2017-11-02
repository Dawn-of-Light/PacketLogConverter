﻿/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */

#define SKIP_CR_IN_DESCRIPTION
using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x81, 1115, ePacketDirection.ServerToClient, "Show dialog v1115")]
	public class StoC_0x81_ShowDialog_1115 : StoC_0x81_ShowDialog_1112
	{
		public new NewQuestUpdate_1115 InNewQuestUpdate		{ get { return subData as NewQuestUpdate_1115; } }
		protected override void InitNewQuestUpdate()
		{
			subData = new NewQuestUpdate_1115();
			subData.Init(this);
		}

		public class NewQuestUpdate_1115: NewQuestUpdate_1112
		{
			public override void Init(StoC_0x81_ShowDialog pak)
			{
				questName = pak.ReadPascalString(); // 0x0c
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
                
                moneyReward = pak.ReadInt();               
				rewardExp = pak.ReadByte();
				baseRewardsCount = pak.ReadByte();
				baseRewards = new StoC_0x02_InventoryUpdate.Item[baseRewardsCount];
				for (int i = 0; i < baseRewardsCount; i++)
				{
					StoC_0x02_InventoryUpdate.Item item = new StoC_0x02_InventoryUpdate.Item();
					item.itemID = pak.ReadShort();
					item.level = pak.ReadByte();

					item.value1 = pak.ReadByte();
					item.value2 = pak.ReadByte();

					item.hand = pak.ReadByte();
					byte temp = pak.ReadByte(); //WriteByte((byte) ((item.Type_Damage*64) + item.Object_Type));
					item.damageType = (byte)(temp >> 6);
					item.objectType = (byte)(temp & 0x3F);
					item.unk1_1112 = pak.ReadByte();
					item.weight = pak.ReadShort();
					item.condition = pak.ReadByte();
					item.durability = pak.ReadByte();
					item.quality = pak.ReadByte();
					item.bonus = pak.ReadByte();
					item.bonus_level = pak.ReadByte();
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
					baseRewards[i] = item;
				}
				optionalRewardsChoiceMax = pak.ReadByte();
				optionalRewardsCount = pak.ReadByte();
				optionalRewards = new StoC_0x02_InventoryUpdate.Item[optionalRewardsCount];
				for (int i = 0; i < optionalRewardsCount; i++)
				{
					StoC_0x02_InventoryUpdate.Item item = new StoC_0x02_InventoryUpdate.Item();
					item.itemID = pak.ReadShort();
					item.level = pak.ReadByte();

					item.value1 = pak.ReadByte();
					item.value2 = pak.ReadByte();

					item.hand = pak.ReadByte();
					byte temp = pak.ReadByte(); //WriteByte((byte) ((item.Type_Damage*64) + item.Object_Type));
					item.damageType = (byte)(temp >> 6);
					item.objectType = (byte)(temp & 0x3F);
					item.unk1_1112 = pak.ReadByte();
					item.weight = pak.ReadShort();
					item.condition = pak.ReadByte();
					item.durability = pak.ReadByte();
					item.quality = pak.ReadByte();
					item.bonus = pak.ReadByte();
					item.bonus_level = pak.ReadByte();
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
					optionalRewards[i] = item;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\n\tQuestName: \"{0}\"\n\tQuestDesc: \"{1}\"", questName, questDesc);
				text.Write("\n\tlen:{0} message:\"{1}\"", dialogLen, message);
				text.Write("\n\tquestID:0x{0:X4} goalsCount:{1}", questID, goalsCount);
				for (int i = 0; i < goalsCount; i++)
				{
					text.Write("\n\t[{0}]: \"{1}\"", i, goals[i]);
				}
                
                text.Write("\n\tMoney_Reward:{0} Exp:{1:00.0}% baseRewardsCount:{2}", MoneyRewardFormatted(moneyReward), rewardExp, baseRewardsCount);
                
				for (int i = 0; i < baseRewardsCount; i++)
				{

					StoC_0x02_InventoryUpdate.Item item = baseRewards[i];

					text.Write("\n\t[{0}]: level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} unk_1112:0x{18:X2} bonus level:{19:X2} itemID:{20} \"{17}\"",
						i, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name, item.unk1_1112, item.bonus_level, item.itemID);
					if (flagsDescription && item.name != null && item.name != "")
						text.Write(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
					if ((item.flag & 0x08) == 0x08)
						text.Write("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
						item.effectIcon, item.effectName);
					if ((item.flag & 0x10) == 0x10)
						text.Write("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
						item.effectIcon2, item.effectName2);
				}
				text.Write("\n\toptionalRewardsChoiceMax:{0} optionalRewardsCount:{1}", optionalRewardsChoiceMax, optionalRewardsCount);
				for (int i = 0; i < optionalRewardsCount; i++)
				{

					StoC_0x02_InventoryUpdate.Item item = optionalRewards[i];

					text.Write("\n\t[{0}]: level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} unk_1112:0x{18:X2} bonus level:{19:X2} itemID:{20} \"{17}\"",
						i, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name, item.unk1_1112, item.bonus_level, item.itemID);
					if (flagsDescription && item.name != null && item.name != "")
						text.Write(" ({0})", (StoC_0x02_InventoryUpdate.eObjectType)item.objectType);
					if ((item.flag & 0x08) == 0x08)
						text.Write("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
						item.effectIcon, item.effectName);
					if ((item.flag & 0x10) == 0x10)
						text.Write("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
						item.effectIcon2, item.effectName2);
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x81_ShowDialog_1115(int capacity) : base(capacity)
		{
		}
	}
}