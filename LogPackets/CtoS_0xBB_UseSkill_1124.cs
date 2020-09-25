using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xBB, 1124, ePacketDirection.ClientToServer, "Use skill 1124")]
    public class CtoS_0xBB_UseSkill_1124 : CtoS_0xBB_UseSkill
    {
        int clientX = 0;
        int clientY = 0;
        int clientZ = 0;
        int clientCurrentSpeed = 0;
        ushort clientHeading = 0;
        ushort flags = 0;        
        ushort unk1 = 0;
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("Player X:{0,-6} Y:{1,-6} Z:{2,-5}) speed:{3,-3} heading:{4,-4} flags:0x{5,X4} spellLevel:{6,1} spellIndex:{7,1} unk1:{8,1}",
               clientX, clientY, clientZ, clientCurrentSpeed, clientHeading & 0xFFF, flags, index, type, unk1);
            if (flagsDescription)
            {
                string speed = (flagSpeedData & 0x1FF).ToString();
                if ((flagSpeedData & 0x200) == 0x200)
                    speed = "-" + speed;
                if ((flagSpeedData & 0x400) == 0x400)
                    speed += ",UNKx0400";
                if ((flagSpeedData & 0x800) == 0x800)
                    speed += ",PetInView";
                if ((flagSpeedData & 0x1000) == 0x1000)
                    speed += ",GTinView";
                if ((flagSpeedData & 0x2000) == 0x2000)
                    speed += ",CheckTargetInView";
                if ((flagSpeedData & 0x4000) == 0x4000)
                    speed += ",Strafe";// Swim under water
                if ((flagSpeedData & 0x8000) == 0x8000)
                    speed += ",TargetInView";
                text.Write(" (speed:{0})", speed);
            }

        }

        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;
            clientX = (int)ReadFloatLowEndian();
            clientY = (int)ReadFloatLowEndian();
            clientZ = (int)ReadFloatLowEndian();
            clientCurrentSpeed = (int)ReadFloatLowEndian();
            clientHeading = ReadShort();
            flags = ReadShort(); // target visible ? 0xA000 : 0x0000
            index = ReadByte();
            type = ReadByte();
            unk1 = ReadShort();
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xBB_UseSkill_1124(int capacity) : base(capacity)
        {
        }
    }
}
