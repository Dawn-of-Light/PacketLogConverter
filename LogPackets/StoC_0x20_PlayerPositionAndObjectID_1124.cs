using System.IO;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0x20, 1124, ePacketDirection.ServerToClient, "Set player position and OID v1124")]
    public class StoC_0x20_PlayerPositionAndObjectID_1124 : StoC_0x20_PlayerPositionAndObjectID_174
    {
        private int objectX;
        private int objectY;
        private int objectZ;
        private byte unkflag1;
        private byte unkflag2;
        private byte unkflag3;
        private byte unkflag4;
        private byte housing;
        
        private string servername = "";
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("x:{0,-6} y:{1,-6} z:{2,-5} oid:0x{3:X4} heading:0x{4:X4} zoneXOffset:{5,-2} zoneYOffset:{6,-2} region:{7,-3} flags:0x{8:X2} servername:\"{9}\" unk1:0x{10:X2} unk2:0x{11:X2} unk3:0x{12:X2}, unk4:0x{13:X2}",
                objectX, objectY, objectZ, playerOid, heading, zoneXOffset, zoneYOffset, region, flags, servername, unkflag1, unkflag2, unkflag3, unkflag4);
            if (flagsDescription && (flags != 0))
            {
                string str = "";
                if ((flags & 0x80) == 0x80)
                    str += "DivingEnable";
                if ((flags & 0x01) == 0x01)
                    str += ",Underwater";
                text.Write("({0})", str);
            }
        }

        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;
            
            objectX = (int)ReadFloatLowEndian();
            objectY = (int)ReadFloatLowEndian();
            objectZ = (int)ReadFloatLowEndian();
            playerOid = ReadShort();
            heading = ReadShort();                       
            zoneXOffset = ReadShort();
            zoneYOffset = ReadShort();
            region = ReadShort();
            flags = ReadByte();
            try
            {
                servername = ReadPascalString();
            }
            catch
            {
                housing = ReadByte(); // ?? cant remember what i'm doing here
            }                       
            unkflag1 = ReadByte();
            unkflag2 = ReadByte();
            unkflag3 = ReadByte();
            unkflag4 = ReadByte();
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0x20_PlayerPositionAndObjectID_1124(int capacity) : base(capacity)
        {
        }
    }
}