using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xA9, 1124, ePacketDirection.ClientToServer, "Player position update v1124")]
    public class CtoS_0xA9_PlayerPosition_1124 : CtoS_0xA9_PlayerPosition_1112
    {

        protected float playerX;
        protected float playerY;
        protected float playerZ;
        protected float currentSpeed;
        protected float currentZSpeed;        
        protected ushort flags1;
        protected ushort fallingDMG;        
        protected byte flags2;
        protected byte unknown1;
        protected byte unknown2;        
        protected byte mana;
        protected byte endurance;
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("sessionId:0x{0:X4} currentZone({1,-3}) Player X:{2,-6} Y:{3,-6} Z:{4,-5}) heading:{5,-4} speed:{6,-3} ZAxisSpeed:{7,-3} Flags1:0x{8:X4} Flags2:0x{9:X2} health:{10,3}%, mana:{11,3}% Endo:{12,3}%unknown:0x{13:X2},0x{14:X2}",
                sessionId, currentZoneId, (uint)playerX, (uint)playerY, (uint)playerZ, heading & 0xFFF, currentSpeed, currentZSpeed, flags1, flags2, health & 0x7F, mana & 0x7F, endurance & 0x7F, unknown1, unknown2);

            
            if (flags1 != 0 || flags2 != 0 || fallingDMG != 0)
            {
                AddDescription(text);
            }
        }


        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;
                        
            playerX = ReadFloatLowEndian();            
            playerY = ReadFloatLowEndian();
            playerZ = ReadFloatLowEndian();
            currentSpeed = ReadFloatLowEndian();
            currentZSpeed = ReadFloatLowEndian();
            sessionId = ReadShort();
            currentZoneId = ReadShort();
            flags1 = ReadShort();
            fallingDMG = ReadShort();                        
            heading = ReadShort();
            flags2 = ReadByte();
            unknown1 = ReadByte();
            unknown2 = ReadByte();            
            health = ReadByte();
            mana = ReadByte(); // both of these dont seem to be sent, only in server -> client packet
            endurance = ReadByte();
            heading = (ushort)(heading & 0xFFF);
        }

        public override void AddDescription(TextWriter text)
        {
            
            string flags = "Player flags: ";
            
            if ((flags1 & 0x0400) == 0x0400)
                flags += ",Swimming";
            if ((flags1 & 0x1800) == 0x1800)
                flags += ",Riding";
            else if ((flags1 & 0x0800) == 0x0800)
                        flags += ",Jumping";
            else if ((flags1 & 0x1000) == 0x1000)
                        flags += ",Sitting";

            if ((flags1 & 0x1400) == 0x1400)
                flags += ",Dead";            
            if ((flags1 & 0xA000) == 0xA000)
                flags += ",StrafeRight";
            if ((flags1 & 0x6000) == 0x6000)
                flags += ",StrafeLeft";
            if ((flags1 & 0x8000) == 0x8000)
                flags += ",Stop StrafeRight";
            if ((flags1 & 0x4000) == 0x4000)
                flags += ",Stop StrafeLeft";          
            if ((flags1 & 0x1C00) == 0x1C00)
                flags += ",Climbing";         
            if ((flags2 & 0x02) == 0x02)
                flags += ",Underwater";
            if ((flags2 & 0x04) == 0x04)
                flags += ",PetInView";
            if ((flags2 & 0x08) == 0x08)
                flags += ",GT Targeting";          
            if ((flags2 & 0x30) == 0x30)
                flags += ",Target in View";
            if ((flags2 & 0x40) == 0x40)
                flags += ",MoveTo";
            if ((flags2 & 0x80) == 0x80)
                flags += ",Torch On";            
            if ((flags2 & 0x01) == 0x01)
                flags += ",UNKx01";         
            if ((fallingDMG & 0x8000) == 0x8000)
                flags += ",FallDown";
            if ((speed & 0x4000) == 0x4000)
                flags += ",speed_UNK_0x4000";
            if ((speed & 0x2000) == 0x2000)
                flags += ",speed_UNK_0x2000";
            if (flags.Length > 0)
                text.Write(" (" + flags + ")");
        }
        
        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xA9_PlayerPosition_1124(int capacity) : base(capacity)
        {
        }
    }
}