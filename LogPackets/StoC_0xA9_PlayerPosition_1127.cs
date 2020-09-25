using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xA9, 1127, ePacketDirection.ServerToClient, "Player position update v1127")]
    public class StoC_0xA9_PlayerPosition_1127 : StoC_0xA9_PlayerPosition_1124
    {
        protected ushort targetId;
        protected ushort unknown1127;
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("sessionId:0x{0:X4} currentZone({1,-3}): ({2,-6} {3,-6} {4,-5}) heading:0x{5:X4} speed:{6,-3} ZAxisSpeed:{7,-3} PlayerState:0x{8:X2} Flags2:0x{9:X2} RolePlayFlag:0x{10:X2} health:{11,3}%, mana:{12,3}%, endurance:{13,3}%, steedSeatPosition:0x{14:X2} TargetId:{15:X4}, unknown4:0x{16:X2}, unknown1127:{17:X4}",
                sessionId, currentZoneId, playerX, playerY, playerZ, heading & 0xFFF, currentSpeed, currentZSpeed, playerState, flags2, rpFlag, health & 0x7F, manaPercent & 0x7F, endurancePercent & 0x7F, steedSeatPosition, targetId, unknown4, unknown1127);
                        
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
            targetId = ReadShort();
            currentZoneId = ReadShort();
            playerState = ReadShort();
            steedSeatPosition = ReadShort();
            heading = ReadShort();
            flags2 = ReadByte();
            rpFlag = ReadByte();
            unknown4 = ReadByte();
            health = ReadByte();
            manaPercent = ReadByte();
            endurancePercent = ReadByte();
            unknown1127 = ReadShort();
            heading = (ushort)(heading & 0xFFF);
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0xA9_PlayerPosition_1127(int capacity) : base(capacity)
        {
        }
    }
}