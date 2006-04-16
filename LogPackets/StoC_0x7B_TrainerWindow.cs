using System.Collections;
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

		public enum eSubType: byte
		{
			Skills = 0x00,
			RealmAbilities = 0x01,
			ChampionAbilities = 0x02,
		}

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("count:{0} points:{1} type:{2}({4}) unk1:{3}", count, points, subCode, unk1, ((eSubType)subCode).ToString());
			if (subData == null)
				str.AppendFormat(" UNKNOWN SUBCODE");
			else
				subData.MakeString(str);

			return str.ToString();
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
			abstract public void MakeString(StringBuilder str);
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

			public override void MakeString(StringBuilder str)
			{
				for (int i = 0; i < m_skills.Length; i++)
				{
					Skill skill = (Skill)m_skills[i];
					str.AppendFormat("\n\tindex:{0,-3} level:{1,-2} cost:{2,-2} \"{3}\"",
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
					pak.Position -= 2;
					for (int index = 0; index < skill.countSpells; index++)
					{
						ChampionSpell spell = new ChampionSpell();
						spell.indexSkill = pak.ReadByte();
						spell.countSpells = pak.ReadByte();
						spell.index = pak.ReadByte();
						spell.cost = pak.ReadByte();
						spell.icon = pak.ReadShortLowEndian();
						spell.name = pak.ReadPascalString();

						skill.m_spells[index] = spell;
					}
					skill.unk1 = pak.ReadShort();
					m_skills[i] = skill;
				}
			}

			public override void MakeString(StringBuilder str)
			{
				for (int i = 0; i < countRows; i++)
				{
					ChampionSkill skill = (ChampionSkill)m_skills[i];
					str.AppendFormat("\n\tskillIndex:{0,-3} countSpells:{1,-2} unk1:0x{2:X4}",
						skill.index, skill.countSpells, skill.unk1);
					for (int j = 0; j < skill.countSpells; j++)
					{
						ChampionSpell spell = (ChampionSpell)skill.m_spells[j];
						str.AppendFormat("\n\tindex:{0,-3} cost:{1,-2} icon:0x{2:X4} \"{3}\"",
							spell.index, spell.cost, spell.icon, spell.name);
					}
				}
			}
		}

		public struct ChampionSkill
		{
			public byte index;
			public byte countSpells;
			public ChampionSpell[] m_spells;
			public ushort unk1;
		}

		public struct ChampionSpell
		{
			public byte indexSkill;
			public byte countSpells;
			public byte index;
			public byte cost;
			public ushort icon;
			public string name;
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