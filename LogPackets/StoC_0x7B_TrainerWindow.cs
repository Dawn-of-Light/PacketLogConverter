using System.Collections;
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
		
		public TrainerSkillsUpdate	InTrainerSkills { get { return subData as TrainerSkillsUpdate; } }
		public ChampionSkillsUpdate	InChampionAbilities	{ get { return subData as ChampionSkillsUpdate; } }
		
		#endregion

		public enum eSubType: byte
		{
			Skills = 0x00,
			RealmAbilities = 0x01,
			ChampionAbilities = 0x02,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("{5}:{0} points:{1} type:{2}({4}) unk1:{3}", count, points, subCode, unk1, ((eSubType)subCode).ToString(), subCode == 2 ? "IdLine" : "count");
			if (subData == null)
				text.Write(" UNKNOWN SUBCODE");
			else
				subData.MakeString(text);

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
			abstract public void MakeString(TextWriter text);
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

			public override void MakeString(TextWriter text)
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

			public override void MakeString(TextWriter text)
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

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7B_TrainerWindow(int capacity) : base(capacity)
		{
		}
	}
}