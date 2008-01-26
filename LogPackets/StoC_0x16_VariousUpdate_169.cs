using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, 169, ePacketDirection.ServerToClient, "Various update v169")]
	public class StoC_0x16_VariousUpdate_169 : StoC_0x16_VariousUpdate
	{
		protected override void InitPlayerGroupUpdate()
		{
			subData = new PlayerGroupUpdate_169();
			subData.Init(this);
		}

		public class PlayerGroupUpdate_169 : PlayerGroupUpdate
		{
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
					member.endurance = pak.ReadByte(); // new in 1.69
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
					text.Write("\n\tlevel:{0,-2} health:{1,3}% mana:{2,3}% endurance:{3,3}% status:0x{4:X2}",
						member.level, member.health, member.mana, member.endurance, member.status);
					text.Write(" pid:0x{0:X4} class:\"{2}\"\t name:\"{1}\"", member.oid, member.name, member.classname);
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x16_VariousUpdate_169(int capacity) : base(capacity)
		{
		}
	}
}