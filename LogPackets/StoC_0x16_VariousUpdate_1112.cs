using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, 1112, ePacketDirection.ServerToClient, "Various update v1112")]
	public class StoC_0x16_VariousUpdate_1112 : StoC_0x16_VariousUpdate_179
	{
		protected override void InitSkillsUpdate()
		{
			subData = new SkillsUpdate_1112();
			subData.Init(this);
		}

		public class SkillsUpdate_1112 : SkillsUpdate
		{

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				data = new Skill[pak.SubCount];

				for (int i = 0; i < pak.SubCount; i++)
				{
					Skill sk = new Skill();

					sk.level = pak.ReadByte();
					sk.index = pak.ReadShort();
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
					text.Write("level:{0,-2} index:{7,-5} type:{1}({2,-14}) stlOpen:0x{3:X4} bonus:{4,-3} icon:0x{5:X4} name:\"{6}\"",
						skill.level, (int)skill.page, skill.page.ToString().ToLower(), skill.stlOpen, skill.bonus, skill.icon, skill.name, skill.index);
				}
			}
		}


		protected override void InitSpellsListUpdate()
		{
			subData = new SpellsListUpdate_1112();
			subData.Init(this);
		}

		public class SpellsListUpdate_1112 : SpellsListUpdate
		{

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				list = new Spell[pak.SubCount];

				// level 0 spell is LineName
				for (int i = 0; i < pak.SubCount; i++)
				{
					Spell spell = new Spell();

					spell.level = pak.ReadShortLowEndian(); // or (byte and unknown byte) ?
					spell.index = pak.ReadShort();
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
					text.Write("\n\tlevel:{0,-2} index:{3,-5} icon:0x{1:X4} name:\"{2}\"",
						spell.level, spell.icon, spell.name, spell.index);
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x16_VariousUpdate_1112(int capacity) : base(capacity)
		{
		}
	}
}