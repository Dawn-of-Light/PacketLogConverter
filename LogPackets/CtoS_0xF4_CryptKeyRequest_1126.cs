using System.IO;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xF4, 1126, ePacketDirection.ClientToServer, "Crypt key request v1126")]
    public class CtoS_0xF4_CryptKeyRequest_1126 : CtoS_0xF4_CryptKeyRequest_1125
    {        
        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;
            if (Length > 7)
            {                
                keyLength = (ushort)ReadIntLowEndian();
                Skip(keyLength);
                return;
            }
            
            clientTypeAndAddons = ReadByte();
            clientVersionMajor = ReadByte();
            clientVersionMinor = ReadByte();
            clientVersionBuild = ReadByte();
            clientBuildRevision = ReadString(1);
            Skip(2);
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xF4_CryptKeyRequest_1126(int capacity) : base(capacity)
        {
        }
    }
}
