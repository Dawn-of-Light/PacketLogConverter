using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0x4B, 1124, ePacketDirection.ServerToClient, "Player create v1124")]
    public class StoC_0x4B_PlayerCreate_1124 : StoC_0x4B_PlayerCreate_180
    {
        int clientX = 0;
        int clientY = 0;
        int clientZ = 0;
        byte unk2 = 0;
        byte unk3 = 0;

        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("Player X:{0,-6} Y:{1,-6} Z:{2,-5}) sessionID:{3,4} ObjectId:{4,4} heading:{5,-4}  model:0x{6:X4} level:{7,-2} flags:0x{8:X2} eyeSize:0x{9:X2} lipSize:0x{10:X2} moodType:0x{11:X2} eyeColor:0x{12:X2} hairColor:0x{13:X2} faceType:0x{14:X2} hairStyle:0x{15:X2} unk1:{16} unk2:{17} unk3:{18} name:\"{19}\" guild:\"{20}\" lastName:\"{21}\" prefixName:\"{22}\" newTitle:\"{23}\"",
               clientX, clientY, clientZ, sessionId, oid, heading & 0xFFF, model, level, flags, eyeSize, lipSize, moodType, eyeColor, hairColor, faceType,
               hairStyle, unk1_174, unk2, unk3, guildName, lastName, prefixName, realmMissionTitle, horseId);
            text.Write(" horseId:{0}", horseId);
            if (horseId != 0)
                text.Write(" horseBoot:{0,-2} BootColor:0x{1:X4} horseSaddle:{2,-2} SaddleColor:0x{3:X2}",
                    horseBoot, horseBootColor, horseSaddle, horseSaddleColor);
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
            sessionId = ReadShort(); 
            oid = ReadShort();
            heading = ReadShort();            
            model = ReadShort();
            level = ReadByte();
            flags = ReadByte();

            eyeSize = ReadByte();
            lipSize = ReadByte();
            moodType = ReadByte();
            eyeColor = ReadByte();            
            hairColor = ReadByte();
            faceType = ReadByte();
            hairStyle = ReadByte();
            
            unk1_174 = ReadByte();
            unk2 = ReadByte();
            unk3 = ReadByte();
            name = ReadPascalString();
            guildName = ReadPascalString();
            lastName = ReadPascalString();
            prefixName = ReadPascalString();
            realmMissionTitle = ReadPascalString();
            horseId = ReadByte();
            if (horseId != 0)
            {
                horseBoot = ReadByte();
                horseBootColor = ReadShort();
                horseSaddle = ReadByte();
                horseSaddleColor = ReadByte();
            }
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0x4B_PlayerCreate_1124(int capacity) : base(capacity)
        {
        }
    }
}
