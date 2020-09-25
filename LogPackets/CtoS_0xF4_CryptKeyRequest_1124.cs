using System.IO;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xF4, 1124, ePacketDirection.ClientToServer, "Crypt key request v1124")]
    public class CtoS_0xF4_CryptKeyRequest_1124 : CtoS_0xF4_CryptKeyRequest_1115
    {
        protected string clientBuildRevision;
        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            if (Length < 8)
            {
                text.Write("First message ");
            }
            else
            {
                text.Write("keyLength:");
                text.Write(keyLength.ToString("D"));
                text.Write(" ");
            }

            text.Write("clientTypeAndAddons:0x{0:X2} clientVersion: {1}.{2}.{3}{4}",
                clientTypeAndAddons, clientVersionMajor, clientVersionMinor, clientVersionBuild, clientBuildRevision);
            if (flagsDescription)
            {
                text.Write("\n\tclient:{0}", (eClientType)(clientTypeAndAddons & 0x0F));
                text.Write(" expantions:");
                if ((clientTypeAndAddons & 0x80) == 0x80)
                    text.Write(", NewFrontiers");
                if ((clientTypeAndAddons & 0x40) == 0x40)
                    text.Write(", Foundations(Housing)");
            }           
        }
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
                keyLength = ReadShortLowEndian();
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
        public CtoS_0xF4_CryptKeyRequest_1124(int capacity) : base(capacity)
        {
        }
    }
}
