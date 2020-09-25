using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
    [LogPacket(0x15, 1124, ePacketDirection.ServerToClient, "Equipment update v1124")]
    public class StoC_0x15_EquipmentUpdate_1124 : StoC_0x15_EquipmentUpdate_189
    {
        /// <summary>
        /// Initializes the packet. All data parsing must be done here.
        /// </summary>
        /// <returns>True if initialized successfully</returns>
        public override void Init()
        {
            Position = 0;

            oid = ReadShort();                   
            visibleWeaponSlots = ReadByte();     
            speed = ReadByte();                  
            m_helmAndCloakVisibile = ReadByte(); 
            hoodUp = ReadByte();                 
            count = ReadByte();                  

            items = new Item[count];

            for (int i = 0; i < count; i++)
            {
                Item item = new Item();

                item.guildBit_176 = false;
                byte slot = ReadByte();          
                item.slot = (byte)(slot & 0x7F);
                if (item.slot != slot)
                    item.guildBit_176 = true;

                int modelAndBits = ReadShort();
                item.model = (ushort)(modelAndBits & 0x1FFF);

                if (item.slot > 13 || item.slot < 10)
                    item.extension = ReadByte();

                if ((modelAndBits & 0x8000) != 0)
                    item.color = ReadShort();
                else if ((modelAndBits & 0x4000) != 0)
                    item.color = ReadByte();

                if ((modelAndBits & 0x2000) != 0)
                    item.effect = ReadShort();

                items[i] = item;
            }
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0x15_EquipmentUpdate_1124(int capacity) : base(capacity)
        {
        }
    }
}
