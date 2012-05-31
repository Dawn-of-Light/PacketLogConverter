using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7B, -1, ePacketDirection.ServerToClient, "Trainer window")]
	public class StoC_0x7B_TrainerWindow: Packet
	{
		protected byte count;
		protected byte points;
		protected byte subCode;
		protected byte unk1;
		protected ASubData subData;

		#region public access properties

		public byte Count { get { return count; } }
		public byte Points { get { return points; } }
		public byte SubCode { get { return subCode; } }
		public byte Unk1 { get { return unk1; } }
		public ASubData SubData { get { return subData; } }

		#endregion

		#region Filter Helpers
		
		public TrainerSkillsUpdate	InTrainerSkills 	{ get { return subData as TrainerSkillsUpdate; } }
		public ChampionSkillsUpdate	InChampionAbilities	{ get { return subData as ChampionSkillsUpdate; } }
		public SkillNamesUpdate		InSkillNames		{ get { return subData as SkillNamesUpdate; } }
		
		#endregion

		public enum eSubType: byte
		{
			Skills = 0x00,
			RealmAbilities = 0x01,
			ChampionAbilities = 0x02,
			SpecSkillsUpdate = 0x03,     // v.205+
			SkillNamesUpdate = 0x04,     // v.205+
			RealmAbilitiesUpdate = 0x05, // v.205+
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("{5}:{0} points:{1} type:{2}({4}) unk1:{3}", count, points, subCode, unk1, ((eSubType)subCode).ToString(), subCode == 2 ? "IdLine" : "count");
			if (subData == null)
				text.Write(" UNKNOWN SUBCODE");
			else
				subData.MakeString(text, flagsDescription);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			count = ReadByte();
			points = ReadByte();
			subCode = ReadByte();
			unk1 = ReadByte();
			InitSubcode(subCode);
		}

		private void InitSubcode(byte code)
		{
			switch (code)
			{
				case 0:
				case 1: InitTrainerSkillsUpdate(); break;
				case 2: InitChampionSkillsUpdate(); break;
				case 3: InitSpecSkillsUpdate(); break;
				case 4: InitSkillNamesUpdate(); break;
				case 5: InitRealmAbilitiesUpdate(); break;
				default: subData = null; break;
			}
			return;
		}

		/// <summary>
		/// Base abstract class for all sub codes data
		/// </summary>
		public abstract class ASubData
		{
			abstract public void Init(StoC_0x7B_TrainerWindow pak);
			abstract public void MakeString(TextWriter text, bool flagsDescription);
		}

		#region subcode 1 - Trainer Skills Update

		protected virtual void InitTrainerSkillsUpdate()
		{
			subData = new TrainerSkillsUpdate();
			subData.Init(this);
		}

		public class TrainerSkillsUpdate : ASubData
		{
			public Skill[] m_skills;

			public override void Init(StoC_0x7B_TrainerWindow pak)
			{
				m_skills = new Skill[pak.count];

				for (int i = 0; i < pak.count; i++)
				{
					Skill skill = new Skill();

					skill.index = pak.ReadByte();
					skill.level = pak.ReadByte();
					skill.cost = pak.ReadByte();
					skill.name = pak.ReadPascalString();

					m_skills[i] = skill;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				for (int i = 0; i < m_skills.Length; i++)
				{
					Skill skill = (Skill)m_skills[i];
					text.Write("\n\tindex:{0,-3} level:{1,-2} cost:{2,-2} \"{3}\"",
					skill.index, skill.level, skill.cost, skill.name);
				}
			}
		}

		public struct Skill
		{
			public byte index;
			public byte level;
			public byte cost;
			public string name;
		}

		#endregion

		#region subcode 2 - Champion Skills Update
		
		protected virtual void InitChampionSkillsUpdate()
		{
			subData = new ChampionSkillsUpdate();
			subData.Init(this);
		}

		public class ChampionSkillsUpdate: ASubData
		{
			public byte countRows;
			public ChampionSkill[] m_skills;

			public override void Init(StoC_0x7B_TrainerWindow pak)
			{
				countRows = pak.ReadByte();
				m_skills = new ChampionSkill[countRows];

				for (int i = 0; i < countRows; i++)
				{
					ChampionSkill skill = new ChampionSkill();
					skill.index = pak.ReadByte();
					skill.countSpells = pak.ReadByte();
					skill.m_spells = new ChampionSpell[skill.countSpells];
					for (int index = 0; index < skill.countSpells; index++)
					{
						ChampionSpell spell = new ChampionSpell();
						spell.index = pak.ReadByte();
						spell.type = pak.ReadByte();
						spell.icon = pak.ReadShortLowEndian();
						spell.name = pak.ReadPascalString();
						spell.aviability = pak.ReadByte();
						spell.stickedSkillsCount = pak.ReadByte();
						if (spell.stickedSkillsCount > 0)
							spell.stickedSkills = new byte[spell.stickedSkillsCount];
						for (int k = 0; k < spell.stickedSkillsCount; k++)
						{
							spell.stickedSkills[k] = pak.ReadByte();
						}
						skill.m_spells[index] = spell;
					}
					m_skills[i] = skill;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				for (int i = 0; i < countRows; i++)
				{
					ChampionSkill skill = (ChampionSkill)m_skills[i];
					text.Write("\n\tskillIndex:{0,-3} countSpells:{1,-2}",
						skill.index, skill.countSpells);
					for (int j = 0; j < skill.countSpells; j++)
					{
						ChampionSpell spell = (ChampionSpell)skill.m_spells[j];
						text.Write("\n\tindex:{0,-3} type:{1,-2} icon:0x{2:X4} aviability:{4} stickedSkillsCount:{5} \"{3}\" ",
							spell.index, spell.type, spell.icon, spell.name, spell.aviability, spell.stickedSkillsCount);
						for (int k = 0; k < spell.stickedSkillsCount; k++)
						{
							text.Write(" [{0}]:0x{1:X2}", k, spell.stickedSkills[k]);
						}
					}
				}
			}
		}

		public struct ChampionSkill
		{
			public byte index;
			public byte countSpells;
			public ChampionSpell[] m_spells;
		}

		public struct ChampionSpell
		{
			public byte indexSkill;
			public byte countSpells;
			public byte index;
			public byte type;
			public ushort icon;
			public string name;
			public byte aviability;//1 - can purchase, 2 - purchased
			public byte stickedSkillsCount;
			public byte[] stickedSkills;
		}

		#endregion


		protected virtual void InitSpecSkillsUpdate()
		{
			subData = new SpecSkillsUpdate();
			subData.Init(this);
		}

		public class SpecSkillsUpdate: ASubData
		{
			public byte[] specPoints = new byte[50];
			public Dictionary<int, List<SpecSkill>> m_skills;

			public override void Init(StoC_0x7B_TrainerWindow pak)
			{
				for (int i = 0 ; i < 50; i++)
				{
					specPoints[i] = pak.ReadByte();
				}
				m_skills = new Dictionary<int, List<SpecSkill>>(pak.count);
				for (int j = 0; j < pak.count; j++)
				{
					int index = pak.ReadByte();
					int skillsCount = pak.ReadByte();
					pak.Skip(1); // TODO
					List<SpecSkill> specList = new List<SpecSkill>(skillsCount);
					for (int i = 0; i < skillsCount ; i++)
					{
						SpecSkill skill = new SpecSkill();
						skill.level = pak.ReadByte();
						skill.icon  = pak.ReadShort();
						skill.type  = pak.ReadByte();
						if (skill.type == (byte)StoC_0x16_VariousUpdate.eSkillPage.Styles)
						{
							skill.internalId = skill.icon;
							skill.unk1 = pak.ReadByte();
							skill.unk2 = pak.ReadByte();
							if (skill.unk1 >= 200) // on prev style
							{
								skill.previosId = pak.ReadShort();
							}
							skill.icon = pak.ReadShort();
						}
						else
						{
							skill.unk1 = pak.ReadByte();
							skill.unk2 = pak.ReadByte();
							skill.internalId = pak.ReadShort();
						}
						specList.Add(skill);
					}
					m_skills[index] = specList;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\n\t");
				for (int i = 0; i < specPoints.Length; i++)
				{
					text.Write(",{0}", specPoints[i]);
				}
				foreach (KeyValuePair<int, List<SpecSkill>> entry in m_skills)
				{
					text.Write("\n\tskillIndex:{0}", (int)entry.Key);
					foreach(SpecSkill skill in (List<SpecSkill>)entry.Value)
					{
						text.Write("\n\t\tlevel:{0,-2} icon:0x{1:X4} type:{2} internalId:0x{3:X4}",
							skill.level, skill.icon, skill.type, skill.internalId);
						if (skill.type == (byte)StoC_0x16_VariousUpdate.eSkillPage.Styles)
						{
							text.Write(" opening1:{0,-3} opening2:{1,-3}", skill.unk1, skill.unk2);
							if (skill.unk1 >= 200)
							{
								text.Write(" previosId:0x{0:X4}", skill.previosId);
							}
						}
						else
						{
							text.Write(" unk1:0x{0:X2} unk2:{1:X2}", skill.unk1, skill.unk2);
						}
					}
				}
			}
		}

		public struct SpecSkill
		{
			public byte level;
			public ushort icon;
			public byte type;
			public byte unk1;        // opening1 for type = 2 (style)
			public byte unk2;        // opening2 for type = 2 (style)
			public ushort previosId; // style only
			public ushort internalId;
		}

		protected virtual void InitSkillNamesUpdate()
		{
			subData = new SkillNamesUpdate();
			subData.Init(this);
		}

		public class SkillNamesUpdate: ASubData
		{
			public byte startIndex;
			public string[] names;

			public override void Init(StoC_0x7B_TrainerWindow pak)
			{
				startIndex = pak.ReadByte();
				names = new string[pak.count];

				for (int i = 0; i < pak.count; i++)
				{
					names[i] = pak.ReadPascalString();
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write(" startIndex:{0}", startIndex);
				for (int i = 0; i < names.Length; i++)
				{
					text.Write("\n\t");
					if (flagsDescription)
					{
						text.Write("[{0, 2}] ", startIndex + i);

					}
					text.Write("\"{0}\"", names[i]);
				}
			}
		}
		

		protected virtual void InitRealmAbilitiesUpdate()
		{
			subData = new RealmAbilitiesUpdate();
			subData.Init(this);
		}

		public class RealmAbilitiesUpdate: ASubData
		{
			public RealmAbility[] m_skills;

			public override void Init(StoC_0x7B_TrainerWindow pak)
			{
				m_skills = new RealmAbility[pak.count];

				for (int i = 0; i < pak.count; i++)
				{
					RealmAbility skill = new RealmAbility();
					skill.level = pak.ReadByte();
					skill.minLevel = pak.ReadByte();
					skill.type = pak.ReadByte();
					skill.cost = pak.ReadByte();
					if (skill.type == (byte)StoC_0x16_VariousUpdate.eSkillPage.AbilitiesSpell)
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
					text.Write("\n\tlevel:{0} minLevel:{1, 2} type:{2}({7}) cost:{3} unk2:0x{4:X4} unk3:0x{5:X4} {6}",
							skill.level, skill.minLevel, skill.type, skill.cost, skill.unk2, skill.unk3, skill.name, (StoC_0x16_VariousUpdate.eSkillPage)skill.type);
				}
			}
		}

		public struct RealmAbility
		{
			public byte level;
			public byte minLevel;
			public byte type;
			public byte cost;
			public ushort unk2;
			public ushort unk3;
			public string name;
			public byte   unk1_1112;
			public byte   unk2_1112;
			public ushort unk3_1112;
		}


		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7B_TrainerWindow(int capacity) : base(capacity)
		{
		}
	}
}