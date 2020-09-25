using System;
using System.Collections;
using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x70, 1125, ePacketDirection.ServerToClient, "Player group update v1125")]
	public class StoC_0x70_PlayerGroupUpdate_1125: StoC_0x70_PlayerGroupUpdate_173
    {
        public int updateType;

        /// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			ArrayList groupUpdates = new ArrayList(8);
			playerIndex = ReadByte();
			while (playerIndex != 0)
			{
				switch (playerIndex & 0xF0)
				{
					case 0x20: //groupUpdates.Add("player status update");
                        updateType = 1;
                               groupUpdates.Add(ReadPlayerStatus(playerIndex));break;
					case 0x40:
                        //groupUpdates.Add("player map loc update");
                        updateType = 2;
                        groupUpdates.Add(ReadPlayerMap(playerIndex)); break;
					case 0x80:
                        //groupUpdates.Add("player icon update");
                        updateType = 3;
                        groupUpdates.Add(ReadPlayerBuffs(playerIndex)); break;
					default: throw new Exception("unknow player position code:0x"+(playerIndex & 0xF0).ToString("X2"));
				}
				playerIndex = ReadByte();
			}

			updates = (object[])groupUpdates.ToArray(typeof (object));
		}        

        protected override object ReadPlayerStatus(byte playerIndex)
        {
            PlayerStatusData_169 data = new PlayerStatusData_169
            {
                playerIndex = (byte)((playerIndex & 0x0F) + 1),
                health = ReadByte(),
                mana = ReadByte(),
                endurance = ReadByte(),
                status = ReadByte()
            };

            return data;
        }

        public override object ReadPlayerMap(byte playerIndex)
        {
            PlayerMapData mapData = new PlayerMapData();

            mapData.player = (byte)((playerIndex & 0x0F) + 1);
            mapData.zone = ReadShort();
            mapData.x = ReadShort();
            mapData.y = ReadShort();

            return mapData;

        }
        public override object ReadPlayerBuffs(byte playerIndex)
        {
            PlayerBuffsData buffsData = new PlayerBuffsData();

            buffsData.playerIndex = (byte)((playerIndex & 0x0F) + 1);
            buffsData.count = ReadByte();
            BuffData[] buffs = buffsData.buffs = new BuffData[buffsData.count];
            for (int i = 0; i < buffsData.count; i++)
            {
                buffs[i].type = ReadByte();
                buffs[i].spellId = ReadShort();
            }

            return buffsData;
        }

        public override void GetPacketDataString(TextWriter text, bool flagsDescription)
        {
            switch (updateType)
            {
                case 1: text.Write("player status updates"); break;
                case 2: text.Write("player map updates"); break;
                case 3: text.Write("player icon updates"); break;
                default: text.Write("unknown update type"); break;
            }
            for (int i = 0; i < updates.Length; i++)
            {
                text.Write("\n\t");
                text.Write(updates[i].ToString());
            }
        }

        /// <summary>
        /// Constructs new instance with given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public StoC_0x70_PlayerGroupUpdate_1125(int capacity) : base(capacity)
		{
		}
	}
}
