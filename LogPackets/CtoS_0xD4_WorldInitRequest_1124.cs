using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xD4, 1124, ePacketDirection.ClientToServer, "World init request v1124")]
    public class CtoS_0xD4_WorldInitRequest_1124 : CtoS_0xD4_WorldInitRequest_172
    {
        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;           
            unk4 = ReadByte();
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public CtoS_0xD4_WorldInitRequest_1124(int capacity) : base(capacity)
        {
        }
    }
}
