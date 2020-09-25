using System.IO;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xF4, 1125, ePacketDirection.ClientToServer, "Crypt key request v1125")]
    public class CtoS_0xF4_CryptKeyRequest_1125 : CtoS_0xF4_CryptKeyRequest_1124
    {
        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;
                        
            clientTypeAndAddons = ReadByte();
            clientVersionMajor = ReadByte();
            clientVersionMinor = ReadByte();
            clientVersionBuild = ReadByte();
            clientBuildRevision = ReadString(1);
            if (Length > 7)
            {
                Skip(2);
                keyLength = (ushort)ReadIntLowEndian();
                Skip(keyLength);
            }
            else
            {
                Skip(2);
            }
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xF4_CryptKeyRequest_1125(int capacity) : base(capacity)
        {
        }
    }
}
