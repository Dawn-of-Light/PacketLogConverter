using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7B, 1112, ePacketDirection.ServerToClient, "Trainer window v1112")]
	public class StoC_0x7B_TrainerWindow_1112: StoC_0x7B_TrainerWindow
	{
		protected override void InitRealmAbilitiesUpdate()
		{
			subData = new RealmAbilitiesUpdate_1112();
			subData.Init(this);
		}

		public class RealmAbilitiesUpdate_1112: RealmAbilitiesUpdate
		{
			public override void Init(StoC_0x7B_TrainerWindow pak)
			{
				m_skills = new RealmAbility[pak.Count];

				for (int i = 0; i < pak.Count; i++)
				{
					RealmAbility skill = new RealmAbility();
					skill.level = pak.ReadByte();
					skill.minLevel = pak.ReadByte();
					skill.type = pak.ReadByte();
					skill.cost = pak.ReadByte();
					if (skill.type == 9)
					{
						skill.unk1_1112 = pak.ReadByte();
						skill.unk2_1112 = pak.ReadByte();
						skill.unk3_1112 = pak.ReadShort();
						skill.unk2 = pak.ReadShort();
					}
					if ((skill.type == (byte)StoC_0x16_VariousUpdate.eSkillPage.AbilitiesSpell))
					{
						skill.unk2 = pak.ReadShort();
					}
					skill.unk3 = pak.ReadShort();
					skill.name = pak.ReadPascalString();
					m_skills[i] = skill;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				for (int i = 0; i < m_skills.Length; i++)
				{
					RealmAbility skill = (RealmAbility)m_skills[i];
					text.Write("\n\tlevel:{0} minLevel:{1, 2} type:{2}({6}) cost:{3} unk2:0x{4:X4} unk3:0x{5:X4}",
							skill.level, skill.minLevel, skill.type, skill.cost, skill.unk2, skill.unk3, (StoC_0x16_VariousUpdate.eSkillPage)skill.type);
					if (skill.type == 9)
					{
						text.Write(" unk1_1112:0x{0:X2} unk2_1112:0x{1:X2} unk3_1112:0x{2:X4}",
								skill.unk1_1112, skill.unk2_1112, skill.unk3_1112);
					}
					text.Write(" {0}", skill.name);
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7B_TrainerWindow_1112(int capacity) : base(capacity)
		{
		}
	}
}