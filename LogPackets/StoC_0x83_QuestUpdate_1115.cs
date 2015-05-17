/*
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
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 1115, ePacketDirection.ServerToClient, "Quest update v1115")]
	public class StoC_0x83_QuestUpdate_1115 : StoC_0x83_QuestUpdate_187
	{
		protected override void InitQuestUpdate()
		{
			subData = new QuestUpdate_1115();
			subData.Init(this);
		}

		public class QuestUpdate_1115 : QuestUpdate_187
		{
			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("index:{0,-2} NameLen:{1,-3} descLen:{2,-3} rewards:{3} level:{4}", index, lenName, lenDesc, unk1, level);

				if (lenName == 0 && lenDesc == 0)
					return;
				text.Write("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);
			}
		}

		protected override void InitNewQuestUpdate()
		{
			subData = new NewQuestUpdate_1115();
			subData.Init(this);
		}

		public class NewQuestUpdate_1115 : NewQuestUpdate_187
		{
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				index = pak.ReadByte();          // 0x00
				lenName = pak.ReadByte();        // 0x01
				unk2 = pak.ReadShortLowEndian(); // 0x02
				goalsCount = pak.ReadByte();     // 0x04
				level = pak.ReadByte();          // 0x05
				name = pak.ReadString(lenName);  // 0x06
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
						item.unk1_1115 = pak.ReadShort();
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
						item.unk2_1112 = pak.ReadByte();
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

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("index:{0,-2} (NewQuest) NameLen:{1,-3} descLen:{2,-3} goals:{3} level:{4} unk2:{5}", index, lenName, lenDesc, goalsCount, level, unk2);

				if (lenName == 0 && lenDesc == 0)
					return;
				text.Write("\n\tQuestName: \"{0}\"\n\tQuestDesc: \"{1}\"", name, desc);
				for (int i = 0; i < goalsCount; i++)
				{
					text.Write("\n\t[{0}]: \"{1}\"", i, goals[i]);

					QuestGoalInfo questGoalInfo = goalInfo[i];
					text.Write("\n\tinfo: type:0x{0:X4} goalFinished:{1}",
						questGoalInfo.type, questGoalInfo.flagGoalFinished);
					text.Write("\n\tzoneId2:{0,-3} @X2:{1,-5} @Y2:{2,-5} radius?:{3}",
 						questGoalInfo.zoneId2, questGoalInfo.XOff2, questGoalInfo.YOff2, questGoalInfo.unk2);
					text.Write("\n\tzoneId1:{0,-3} @X1:{1,-5} @Y1:{2,-5} unk1:0x{3:X4}",
 						questGoalInfo.zoneId, questGoalInfo.XOff, questGoalInfo.YOff, questGoalInfo.unk1);

					StoC_0x02_InventoryUpdate.Item item = goalItems[i];
					if (item.slot > 0)
					{
						text.Write("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} unk_1115:{18} \"{17}\"",
							item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name, item.unk1_1115);
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
		}
		/// <summary>
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x83_QuestUpdate_1115(int capacity) : base(capacity)
		{
		}
	}
}