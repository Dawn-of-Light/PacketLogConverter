using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, -1, ePacketDirection.ServerToClient, "Various update")]
	public class StoC_0x16_VariousUpdate : Packet, IObjectIdPacket, IHouseIdPacket
	{
		protected byte subCode;
		protected byte subCount;
		protected byte subType;
		protected byte startIndex;
		protected ASubData subData;
		// subtype 0 struct
		// byte
		// pascalString
		//
		// subtype 1 struct
		// byte
		// byte
		// byte
		// byte
		// byte
		// ushort
		// pascalString
		// pascalString
		//
		// subtype 2 struct
		// byte
		// ushort
		// pascalString
		//
		// subtype 3 struct
		// ushort
		// byte
		// uint
		// pascalString


		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get
			{
				if (subCode == 6)
					return InPlayerGroupUpdate.m_oids;
				return new ushort[] {};
			}
		}

		public ushort HouseId
		{
			get
			{
				if (subCode == 3)
					return InPlayerUpdate.personalHouse;
				return 0;
			}
		}

		#region public access properties

		public byte SubCode { get { return subCode; } }
		public byte SubCount{ get { return subCount; } }
		public byte SubType { get { return subType; } }
		public byte StartIndex { get { return startIndex; } }
		public ASubData SubData { get { return subData; } }

		#endregion
		#region Filter Helpers

		public SkillsUpdate         InSkillsUpdate         { get { return subData as SkillsUpdate; } }
		public SpellsListUpdate     InSpellsListUpdate     { get { return subData as SpellsListUpdate; } }
		public PlayerUpdate         InPlayerUpdate         { get { return subData as PlayerUpdate; } }
		public PlayerStateUpdate    InPlayerStateUpdate    { get { return subData as PlayerStateUpdate; } }
		public PlayerGroupUpdate    InPlayerGroupUpdate    { get { return subData as PlayerGroupUpdate; } }
		public CraftingSkillsUpdate InCraftingSkillsUpdate { get { return subData as CraftingSkillsUpdate; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("subcode:{0} count:{1} subType:{2} startIndex:{3}", subCode, subCount, subType, startIndex);
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

			subCode = ReadByte();
			subCount = ReadByte();
			subType = ReadByte();
			startIndex = ReadByte();
			InitSubcode(subCode);
		}

		public void InitSubcode(byte code)
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
			abstract public void MakeString(TextWriter text, bool flagsDescription);
		}

		#region subcode 1 - Skills Update

		public enum eSkillPage : byte
		{
			Specialization = 0x00,
			Abilities = 0x01,
			Styles = 0x02,
			Spells = 0x03,
			Songs = 0x04,
			AbilitiesSpell = 0x05,
			RealmAbilities = 0x06,
		}

		protected virtual void InitSkillsUpdate()
		{
			subData = new SkillsUpdate();
			subData.Init(this);
		}

		public class SkillsUpdate : ASubData
		{
			public Skill[] data;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				data = new Skill[pak.subCount];

				for (int i = 0; i < pak.subCount; i++)
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

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\nSKILLS UPDATE:");
				int index = -1;
				foreach (Skill skill in data)
				{
					text.Write("\n");
					text.Write("\t");
					if (flagsDescription)
					{
//						if (skill.page == eSkillPage.Styles && skill.stlOpen >= 0x6400) // 0x6400 = 100 << 8
//						{
//							text.Write("*({0,-2})", (skill.stlOpen >> 8) - 100);
//						}
					 	if((int)skill.page > 0)
							index++;
						text.Write("[{0,-2}] ", index);
					}
					text.Write("level:{0,-2} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-3} icon:0x{5:X4} name:\"{6}\"",
						skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name);
				}
			}
		}

		public struct Skill
		{
			public byte level;
			public ushort index;
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
			public Spell[] list;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				list = new Spell[pak.subCount];

				// level 0 spell is LineName
				for (int i = 0; i < pak.subCount; i++)
				{
					Spell spell = new Spell();

					spell.level = pak.ReadByte();
					spell.icon = pak.ReadShort();
					spell.name = pak.ReadPascalString();

					list[i] = spell;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\nSPELLS LIST UPDATE:");
				foreach (Spell spell in list)
				{
					text.Write("\n\tlevel:{0,-2} icon:0x{1:X4} name:\"{2}\"",
						spell.level, spell.icon, spell.name);
				}
			}
		}

		public struct Spell
		{
			public ushort level;
			public ushort index;
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
			public byte playerLevel;
			public string playerName;
			public byte maxHealthHigh;
			public string className;
			public byte maxHealthLow;
			public string profession;
			public byte unk1;
			public string title;
			public byte realmLevel;
			public string realmTitle;
			public byte realmSpecPoints;
			public string classBaseName;
			public byte personalHouseHight;
			public string guildName;
			public byte personalHouseLow;
			public string lastName;
			public byte mlLevel;
			public string raceName;
			public byte unk2;
			public string guildRank;
			public byte unk3;
			public string crafterGuild;
			public byte unk4;
			public string crafterTitle;
			public byte unk5;
			public string mlTitle;
			public ushort health; // calculated value
			public ushort personalHouse; // calculated value

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				playerLevel = pak.ReadByte();
				playerName = pak.ReadPascalString();

				maxHealthHigh = pak.ReadByte();
				className = pak.ReadPascalString();
				maxHealthLow = pak.ReadByte();
				health = (ushort)(((maxHealthHigh & 0xFF) << 8) | (maxHealthLow & 0xFF));
				profession = pak.ReadPascalString();
				unk1 = pak.ReadByte();
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
				unk2 = pak.ReadByte();
				guildRank = pak.ReadPascalString();
				unk3 = pak.ReadByte();
				crafterGuild = pak.ReadPascalString();
				unk4 = pak.ReadByte();
				crafterTitle = pak.ReadPascalString();
				unk5 = pak.ReadByte();
				mlTitle = pak.ReadPascalString();
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\nPLAYER UPDATE:level:{0} name:\"{1}\" health:{2} className:\"{3}\" profession:\"{4}\" title:\"{5}\" realmLevel:{6} realmTitle:\"{7}\" realmSpecPoints:{8} classBaseName:\"{9}\" guildName:\"{10}\" lastName:\"{11}\" raceName:\"{12}\" guildRank:\"{13}\" crafterGuild:\"{14}\" crafterTitle:\"{15}\" ML:\"{16}\"({17})",
					playerLevel, playerName, health, className, profession, title, realmLevel, realmTitle, realmSpecPoints, classBaseName, guildName, lastName, raceName, guildRank, crafterGuild, crafterTitle, mlTitle, mlLevel);
				text.Write("\n\tpersonalHouse:{0} unk1:{1} unk2:{2} unk3:{3} unk4:{4} unk5:{5}",
					personalHouse, unk1, unk2, unk3, unk4, unk5);
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
			public byte weaponDamageHigh;
			public byte weaponDamageLow;
			public byte weaponSkillHigh;
			public byte weaponSkillLow;
			public byte effectiveAFHigh;
			public byte effectiveAFLow;
			public PlayerStateProperty[] properties;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
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

				properties = new PlayerStateProperty[pak.SubCount - 6];
				for (byte i = 0; i < pak.SubCount - 6; i++)
				{
					PlayerStateProperty prop = new PlayerStateProperty();

					prop.index = i;
					prop.value = pak.ReadByte();
					prop.name = pak.ReadPascalString();

					properties[i] = prop;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\nPLAYER STATE UPDATE:");
				text.Write("\n\tweapDam:{0,2}.{1,-3} weapSkill:{2,-4} effectiveAF:{3}",
					weaponDamageHigh, weaponDamageLow, (weaponSkillHigh << 8) + weaponSkillLow, (effectiveAFHigh << 8) + effectiveAFLow);

				foreach (PlayerStateProperty prop in properties)
				{
					text.Write("\n\t[{0}] {1} \"{2}\"", prop.index, prop.value, prop.name);
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
			public GroupMember[] groupMembers;
			public ushort[] m_oids; // oids list

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				groupMembers = new GroupMember[pak.SubCount];
				m_oids = new ushort[pak.SubCount];
				for (int i = 0; i < pak.SubCount; i++)
				{
					GroupMember member = new GroupMember();

					member.level = pak.ReadByte();
					member.health = pak.ReadByte();
					member.mana = pak.ReadByte();
					member.status = pak.ReadByte();
					member.oid = pak.ReadShort();
					member.name = pak.ReadPascalString();
					member.classname = pak.ReadPascalString();
					m_oids[i] = member.oid;
					groupMembers[i] = member;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\nPLAYER GROUP UPDATE:");

				foreach (GroupMember member in groupMembers)
				{
					text.Write("\n\tlevel:{0,-2} health:{1,3}% mana:{2,3}% status:0x{3:X2}", member.level, member.health, member.mana, member.status);
					text.Write(" oid:0x{0:X4} class:\"{2}\"\t name:\"{1}\" ", member.oid, member.name, member.classname);
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
			public CraftingSkill[] skills;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				skills = new CraftingSkill[pak.SubCount];

				for (int i = 0; i < pak.SubCount; ++i)
				{
					CraftingSkill skill = new CraftingSkill();

					skill.points = pak.ReadShort();
					skill.icon = pak.ReadByte();
					skill.unk2 = pak.ReadInt();
					skill.name = pak.ReadPascalString();

					skills[i] = skill;
				}
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\nCRAFTING SKILLS UPDATE:");

				foreach (CraftingSkill skill in skills)
				{
					text.Write("\n\tpoints:{0,-4} icon:0x{1:X2} unk2:{2} name:\"{3}\"", skill.points, skill.icon, skill.unk2, skill.name);
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