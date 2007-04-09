using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, -1, ePacketDirection.ServerToClient, "Various update")]
	public class StoC_0x16_VariousUpdate : Packet
	{
		protected byte subCode;
		protected ASubData subData;

		#region public access properties

		public byte SubCode { get { return subCode; } }
		public ASubData SubData { get { return subData; } }

		#endregion
		#region Filter Helpers
		
		public SkillsUpdate			InSkillsUpdate			{ get { return subData as SkillsUpdate; } }
		public SpellsListUpdate		InSpellsListUpdate		{ get { return subData as SpellsListUpdate; } }
		public PlayerUpdate			InPlayerUpdate			{ get { return subData as PlayerUpdate; } }
		public PlayerStateUpdate	InPlayerStateUpdate		{ get { return subData as PlayerStateUpdate; } }
		public PlayerGroupUpdate	InPlayerGroupUpdate		{ get { return subData as PlayerGroupUpdate; } }
		public CraftingSkillsUpdate	InCraftingSkillsUpdate	{ get { return subData as CraftingSkillsUpdate; } }
		
		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("subcode:0x{0:X2}", subCode);
			if (subData == null)
				str.AppendFormat(" UNKNOWN SUBCODE");
			else
				subData.MakeString(str, flagsDescription);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			subCode = ReadByte();
			InitSubcode(subCode);
		}

		private void InitSubcode(byte code)
		{
			switch (code)
			{
				case 1: InitSkillsUpdate(); break;
				case 2: InitSpellsListUpdate(); break;
				case 3: InitPlayerUpdate(); break;
				case 5: InitPlayerStateUpdate(); break;
				case 6: InitPlayerGroupUpdate(); break;
				case 8: InitCraftingSkillsUpdate(); break;
				default: subData = null; break;
			}
			return;
		}

		/// <summary>
		/// Base abstract class for all sub codes data
		/// </summary>
		public abstract class ASubData
		{
			abstract public void Init(StoC_0x16_VariousUpdate pak);
			abstract public void MakeString(StringBuilder str, bool flagsDescription);
		}

		#region subcode 1 - Skills Update

		public enum eSkillPage : byte
		{
			Specialization = 0x00,
			Abilities = 0x01,
			Styles = 0x02,
			Spells = 0x03,
			Songs = 0x04,
			SavageAbility = 0x05,
			RealmAbilities = 0x06,
		}

		protected virtual void InitSkillsUpdate()
		{
			subData = new SkillsUpdate();
			subData.Init(this);
		}

		public class SkillsUpdate : ASubData
		{
			public byte count;
			public byte subtype;
			public byte startIndex;
			public Skill[] data;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				count = pak.ReadByte();
				subtype = pak.ReadByte();
				startIndex = pak.ReadByte();
				data = new Skill[count];

				for (int i = 0; i < count; i++)
				{
					Skill sk = new Skill();

					sk.level = pak.ReadByte();
					sk.page = (eSkillPage)pak.ReadByte();
					sk.stlOpen = pak.ReadShort();
					sk.bonus = pak.ReadByte();
					sk.icon = pak.ReadShort();
					sk.name = pak.ReadPascalString();

					data[i] = sk;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\nSKILLS UPDATE:  count:{0,-2} subtype:{1} startIndex:{2}", count, subtype, startIndex);
				int index = -1;
				foreach (Skill skill in data)
				{
					str.Append("\n\t");
					if (flagsDescription)
					{
					 	if((int)skill.page > 0)
							index++;
						str.AppendFormat("[{0,-2}] ", index);
					}
					str.AppendFormat("level:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-2} icon:0x{5:X4} name:\"{6}\"",
						skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
				}
			}
		}

		public struct Skill
		{
			public byte level;
			public eSkillPage page;
			public ushort stlOpen;
			public byte bonus;
			public ushort icon;
			public string name;
		}

		#endregion

		#region subcode 2 - Spells List Update

		protected virtual void InitSpellsListUpdate()
		{
			subData = new SpellsListUpdate();
			subData.Init(this);
		}

		public class SpellsListUpdate : ASubData
		{
			public byte count;
			public byte subtype;
			public byte lineIndex;
			public Spell[] list;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				count = pak.ReadByte();
				subtype = pak.ReadByte();
				lineIndex = pak.ReadByte();
				list = new Spell[count];

				// level 0 spell is LineName
				for (int i = 0; i < count; i++)
				{
					Spell spell = new Spell();

					spell.level = pak.ReadByte();
					spell.icon = pak.ReadShort();
					spell.name = pak.ReadPascalString();

					list[i] = spell;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\nSPELLS LIST UPDATE:  count:{0,-2} subtype:{1} lineIndex:{2,-2}", count, subtype, lineIndex);
				foreach (Spell spell in list)
				{
					str.AppendFormat("\n\tlevel:{0,-2} icon:0x{1:X4} name:\"{2}\"",
						spell.level, spell.icon, spell.name);
				}
			}
		}

		public struct Spell
		{
			public byte level;
			public ushort icon;
			public string name;
		}

		#endregion

		#region subcode 3 - Player Update

		protected virtual void InitPlayerUpdate()
		{
			subData = new PlayerUpdate();
			subData.Init(this);
		}

		public class PlayerUpdate : ASubData
		{
			public byte count;
			public byte subtype;
			public byte unk1;
			public byte playerLevel;
			public string playerName;
			public byte maxHealthHigh;
			public string className;
			public byte maxHealthLow;
			public ushort health;
			public string profession;
			public byte unk2;
			public string title;
			public byte realmLevel;
			public string realmTitle;
			public byte realmSpecPoints;
			public string classBaseName;
			public byte personalHouseHight;
			public string guildName;
			public byte personalHouseLow;
			public ushort personalHouse;
			public string lastName;
			public byte mlLevel;
			public string raceName;
			public byte unk3;
			public string guildRank;
			public byte unk4;
			public string crafterGuild;
			public byte unk5;
			public string crafterTitle;
			public byte unk6;
			public string maTitle;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				count = pak.ReadByte();
				subtype = pak.ReadByte();
				unk1 = pak.ReadByte();

				playerLevel = pak.ReadByte();
				playerName = pak.ReadPascalString();

				maxHealthHigh = pak.ReadByte();
				className = pak.ReadPascalString();
				maxHealthLow = pak.ReadByte();
				health = (ushort)(((maxHealthHigh & 0xFF) << 8) | (maxHealthLow & 0xFF));
				profession = pak.ReadPascalString();
				unk2 = pak.ReadByte();
				title = pak.ReadPascalString();
				realmLevel = pak.ReadByte();
				realmTitle = pak.ReadPascalString();
				realmSpecPoints = pak.ReadByte();
				classBaseName = pak.ReadPascalString();
				personalHouseHight = pak.ReadByte();
				guildName = pak.ReadPascalString();
				personalHouseLow = pak.ReadByte();
				personalHouse = (ushort)(((personalHouseHight & 0xFF) << 8) | (personalHouseLow & 0xFF));
				lastName = pak.ReadPascalString();
				mlLevel= pak.ReadByte();
				raceName = pak.ReadPascalString();
				unk3 = pak.ReadByte();
				guildRank = pak.ReadPascalString();
				unk4 = pak.ReadByte();
				crafterGuild = pak.ReadPascalString();
				unk5 = pak.ReadByte();
				crafterTitle = pak.ReadPascalString();
				unk6 = pak.ReadByte();
				maTitle = pak.ReadPascalString();
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\nPLAYER UPDATE:  count:{0} subtype:{1} level:{2} name:\"{3}\" health:{4} className:\"{5}\" profession:\"{6}\" title:\"{7}\" realmLevel:{8} realmTitle:\"{9}\" realmSpecPoints:{10} classBaseName:\"{11}\" guildName:\"{12}\" lastName:\"{13}\" raceName:\"{14}\" guildRank:\"{15}\" crafterGuild:\"{16}\" crafterTitle:\"{17}\" ML:\"{18}\"({19})",
					count, subtype, playerLevel, playerName, health, className, profession, title, realmLevel, realmTitle, realmSpecPoints, classBaseName, guildName, lastName, raceName, guildRank, crafterGuild, crafterTitle, maTitle, mlLevel);
				str.AppendFormat("\n\tpersonalHouse:{0} unk1:{1} unk2:{2} unk3:{3} unk4:{4} unk5:{5} unk6:{6}",
					personalHouse, unk1, unk2, unk3, unk4, unk5, unk6);
			}
		}

		#endregion

		#region subcode 5 - Player State Update

		protected virtual void InitPlayerStateUpdate()
		{
			subData = new PlayerStateUpdate();
			subData.Init(this);
		}

		public class PlayerStateUpdate : ASubData
		{
			public byte count;
			public byte unk1;
			public byte unk2;
			public byte weaponDamageHigh;
			public byte weaponDamageLow;
			public byte weaponSkillHigh;
			public byte weaponSkillLow;
			public byte effectiveAFHigh;
			public byte effectiveAFLow;
			public PlayerStateProperty[] properties;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				count = pak.ReadByte();
				unk1 = pak.ReadByte();
				unk2 = pak.ReadByte();
				weaponDamageHigh = pak.ReadByte();
				if (pak.ReadPascalString() != " ") throw new Exception("expected \" \", got something else.");
				weaponDamageLow = pak.ReadByte();
				if (pak.ReadPascalString() != " ") throw new Exception("expected \" \", got something else.");
				weaponSkillHigh = pak.ReadByte();
				if (pak.ReadPascalString() != " ") throw new Exception("expected \" \", got something else.");
				weaponSkillLow = pak.ReadByte();
				if (pak.ReadPascalString() != " ") throw new Exception("expected \" \", got something else.");
				effectiveAFHigh = pak.ReadByte();
				if (pak.ReadPascalString() != " ") throw new Exception("expected \" \", got something else.");
				effectiveAFLow = pak.ReadByte();
				if (pak.ReadPascalString() != " ") throw new Exception("expected \" \", got something else.");

				properties = new PlayerStateProperty[count-6];
				for (byte i = 0; i < count-6; i++)
				{
					PlayerStateProperty prop = new PlayerStateProperty();

					prop.index = i;
					prop.value = pak.ReadByte();
					prop.name = pak.ReadPascalString();

					properties[i] = prop;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\nPLAYER STATE UPDATE:  count:{0,-2} unk1:{1,-2} unk2:{2,-2}", count, unk1, unk2);
				str.AppendFormat("\n\tweapDam:{0,2}.{1,-3} weapSkill:{2,-4} effectiveAF:{3}",
					weaponDamageHigh, weaponDamageLow, (weaponSkillHigh << 8) + weaponSkillLow, (effectiveAFHigh << 8) + effectiveAFLow);

				for (int i = 0; i < count - 6; i++)
				{
					PlayerStateProperty prop = properties[i];
					str.AppendFormat("\n\t[{0}] {1} \"{2}\"", prop.index, prop.value, prop.name);
				}
			}
		}

		public struct PlayerStateProperty
		{
			public byte index;
			public byte value;
			public string name;
		}

		#endregion

		#region subcode 6 - Player Group Update

		protected virtual void InitPlayerGroupUpdate()
		{
			subData = new PlayerGroupUpdate();
			subData.Init(this);
		}

		public class PlayerGroupUpdate : ASubData
		{
			public byte count;
			public byte unk1;
			public byte unk2;
			public GroupMember[] groupMembers;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				count = pak.ReadByte();
				unk1 = pak.ReadByte();
				unk2 = pak.ReadByte();
				groupMembers = new GroupMember[count];

				for (int i = 0; i < count; i++)
				{
					GroupMember member = new GroupMember();

					member.level = pak.ReadByte();
					member.health = pak.ReadByte();
					member.mana = pak.ReadByte();
					member.status = pak.ReadByte();
					member.oid = pak.ReadShort();
					member.name = pak.ReadPascalString();
					member.classname = pak.ReadPascalString();

					groupMembers[i] = member;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\nPLAYER GROUP UPDATE:  count:{0,-2} unk1:{1} unk2:{2}", count, unk1, unk2);

				for (int i = 0; i < count; i++)
				{
					GroupMember member = groupMembers[i];
					str.AppendFormat("\n\tlevel:{0,-2} health:{1,3}% mana:{2,3}% status:0x{3:X2}", member.level, member.health, member.mana, member.status);
					str.AppendFormat(" oid:0x{0:X4} name:\"{1}\" \tclass:\"{2}\"", member.oid, member.name, member.classname);
				}
			}
		}

		public struct GroupMember
		{
			public byte level;
			public byte health;
			public byte mana;
			public byte status;
			public ushort oid;
			public string name;
			public string classname;
			public byte endurance; // new in 1.69
		}

		#endregion

		#region subcode 8 - Crafting Skills Update

		protected virtual void InitCraftingSkillsUpdate()
		{
			subData = new CraftingSkillsUpdate();
			subData.Init(this);
		}

		public class CraftingSkillsUpdate : ASubData
		{
			public byte skillsCount;
			public byte subtype;
			public byte unk1;
			public CraftingSkill[] skills;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				skillsCount = pak.ReadByte();
				subtype = pak.ReadByte();
				unk1 = pak.ReadByte();
				skills = new CraftingSkill[skillsCount];

				for (int i = 0; i < skillsCount; ++i)
				{
					CraftingSkill skill = new CraftingSkill();

					skill.points = pak.ReadShort();
					skill.icon = pak.ReadByte();
					skill.unk2 = pak.ReadInt();
					skill.name = pak.ReadPascalString();

					skills[i] = skill;
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\nCRAFTING SKILLS UPDATE:  count:{0,-2} subtype:{1} unk1:{2}", skillsCount, subtype, unk1);

				for (int i = 0; i < skillsCount; i++)
				{
					CraftingSkill skill = skills[i];
					str.AppendFormat("\n\tpoints:{0,-4} icon:0x{1:X2} unk2:{2} name:\"{3}\"", skill.points, skill.icon, skill.unk2, skill.name);
				}
			}
		}

		public struct CraftingSkill
		{
			public ushort points;
			public byte icon;
			public uint unk2;
			public string name;
		}

		#endregion

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x16_VariousUpdate(int capacity) : base(capacity)
		{
		}
	}
}