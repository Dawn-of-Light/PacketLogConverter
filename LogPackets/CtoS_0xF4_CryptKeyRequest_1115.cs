using System.IO;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xF4, 1115, ePacketDirection.ClientToServer, "Crypt key request v1115")]
    public class CtoS_0xF4_CryptKeyRequest_1115 : CtoS_0xF4_CryptKeyRequest_186
    {
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            text.Write("keyLength:");
            text.Write(keyLength.ToString("D"));
            text.Write(" ");
            base.GetPacketDataString(text, flagsDescription);
        }
        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;

            rc4Enabled = ReadByte();
            clientTypeAndAddons = ReadByte();
            clientVersionMajor = ReadByte();
            clientVersionMinor = ReadByte();
            clientVersionBuild = ReadByte();
            if (rc4Enabled == 1)
            {
                base.keyLength = ReadByte(); ;
                Skip(keyLength);
            }
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xF4_CryptKeyRequest_1115(int capacity) : base(capacity)
        {
        }
    }
}
