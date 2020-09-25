using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, 1125, ePacketDirection.ServerToClient, "Various update v1125")]
	public class StoC_0x16_VariousUpdate_1125 : StoC_0x16_VariousUpdate_1112
	{
        /// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
        {
            Position = 0;

            subCode = ReadByte();
            subCount = ReadByte();
            if (subCode != 6)
            {
                subType = ReadByte();
                startIndex = ReadByte();
            }
            InitSubcode(subCode);
        }

        protected override void InitPlayerGroupUpdate()
        {
            subData = new PlayerGroupUpdate1125();
            subData.Init(this);
        }

        public class PlayerGroupUpdate1125 : PlayerGroupUpdate
        {            
            public override void Init(StoC_0x16_VariousUpdate pak)
            {
                groupMembers = new GroupMember[pak.SubCount];
                m_oids = new ushort[pak.SubCount];
                for (int i = 0; i < pak.SubCount; i++)
                {
                    GroupMember member = new GroupMember();                    
                    member.name = pak.ReadPascalString();
                    member.classname = pak.ReadPascalString();
                    member.oid = pak.ReadShort();
                    member.level = pak.ReadByte();
                    m_oids[i] = member.oid;
                    groupMembers[i] = member;
                }
            }

            public override void MakeString(TextWriter text, bool flagsDescription)
            {
                text.Write("\nPLAYER GROUP UPDATE:");

                foreach (GroupMember member in groupMembers)
                {                    
                    text.Write("\n\toid:0x{0:X4} class:\"{2}\"\t name:\"{1}\" level:{3,-2}", member.oid, member.name, member.classname, member.level);
                }
            }
        }


        protected override void InitPlayerStateUpdate()
        {
            subData = new PlayerStateUpdate1125();
            subData.Init(this);
        }

        public class PlayerStateUpdate1125 : PlayerStateUpdate
        {            
            public override void Init(StoC_0x16_VariousUpdate pak)
            {
                //03 00 0C 00 02 00 73 00 02 00 9E 00 
                weaponDamageHigh = pak.ReadByte();
                pak.Skip(1);
                weaponDamageLow = pak.ReadByte();
                pak.Skip(1);
                weaponSkillHigh = pak.ReadByte();
                pak.Skip(1);
                weaponSkillLow = pak.ReadByte();
                pak.Skip(1);
                effectiveAFHigh = pak.ReadByte();
                pak.Skip(1);
                effectiveAFLow = pak.ReadByte();
                pak.Skip(1);

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
                text.Write("\n\tweapDam:{0,-4} weapSkill:{1,-4} effectiveAF:{2}",
                    (weaponDamageHigh << 8) + weaponDamageLow, (weaponSkillHigh << 8) + weaponSkillLow, (effectiveAFHigh << 8) + effectiveAFLow);

                foreach (PlayerStateProperty prop in properties)
                {
                    text.Write("\n\t[{0}] {1} \"{2}\"", prop.index, prop.value, prop.name);
                }
            }
        }
        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0x16_VariousUpdate_1125(int capacity) : base(capacity)
		{
		}
	}
}