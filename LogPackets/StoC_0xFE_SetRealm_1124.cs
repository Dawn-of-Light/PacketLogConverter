
namespace PacketLogConverter.LogPackets
{
    [LogPacket(0xFE, 1124, ePacketDirection.ServerToClient, "Set realm 1124")]
    public class StoC_0xFE_SetRealm1124 : StoC_0xFE_SetRealm
    {
        
        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        public override void Init()
        {
            Position = 0;

            realm = ReadByte();
            if (Length > 1)
            {
                Skip(12);
            }                      
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0xFE_SetRealm1124(int capacity) : base(capacity)
        {
        }
    }
}