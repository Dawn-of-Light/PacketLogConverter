using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xA9, 1127, ePacketDirection.ClientToServer, "Player position update v1127")]
    public class CtoS_0xA9_PlayerPosition_1127 : CtoS_0xA9_PlayerPosition_1124
    {
        protected ushort targetId;
        protected ushort unknown1127;

        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("sessionId:0x{0:X4} currentZone({1,-3}) Player X:{2,-6} Y:{3,-6} Z:{4,-5}) heading:{5,-4} speed:{6,-3} ZAxisSpeed:{7,-3} Flags1:0x{8:X4} Flags2:0x{9:X2} health:{10,3}%, mana:{11,3}% Endo:{12,3}% unknown:0x{13:X2},0x{14:X2} unknown11270x{15:X2}",
                sessionId, currentZoneId, (uint)playerX, (uint)playerY, (uint)playerZ, heading & 0xFFF, currentSpeed, currentZSpeed, flags1, flags2, health & 0x7F, mana & 0x7F, endurance & 0x7F, unknown1, unknown2, unknown1127);

            
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
            targetId = ReadShort();
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
            // two trailing bytes after these two
            unknown1127 = ReadShort();
            heading = (ushort)(heading & 0xFFF);
        }        

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xA9_PlayerPosition_1127(int capacity) : base(capacity)
        {
        }
    }
}