using System;
using System.Collections;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x70, 173, ePacketDirection.ServerToClient, "Player group update v173")]
	public class StoC_0x70_PlayerGroupUpdate_173: StoC_0x70_PlayerGroupUpdate_169
	{
		#region Filter Helpers

		protected PlayerMapData[] playerMapData;
		public PlayerMapData[] InPlayerMapData
		{
			get
			{
				if (playerMapData == null)
				{
					ArrayList list = new ArrayList();
					foreach(object o in updates)
					{
						if (o is PlayerMapData)
							list.Add(o as PlayerMapData);
					}
					playerMapData = (PlayerMapData[])list.ToArray(typeof (PlayerMapData));
				}
				return playerMapData;
			}
		}

		#endregion

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
					case 0x00: groupUpdates.Add(ReadPlayerStatus(playerIndex)); break;
					case 0x40: groupUpdates.Add(ReadPlayerMap(playerIndex)); break;
					case 0x80: groupUpdates.Add(ReadPlayerBuffs(playerIndex)); break;
					default: throw new Exception("unknow player position code:0x"+(playerIndex & 0xF0).ToString("X2"));
				}
				playerIndex = ReadByte();
			}

			updates = (object[])groupUpdates.ToArray(typeof (object));
		}

		#region ReadPlayerMap

		public virtual object ReadPlayerMap(byte playerIndex)
		{
			PlayerMapData mapData = new PlayerMapData();

			mapData.player = (byte)(playerIndex & 0x0F);
			mapData.zone = ReadShort();
			mapData.x = ReadShort();
			mapData.y = ReadShort();

			return mapData;
		}

		public class PlayerMapData
		{
			public byte player;
			public ushort x, y, zone;

			public override string ToString()
			{
				return string.Format("player{0}: zone:{1,-3} x:{2,-5} y:{3,-5}", player, zone, x, y);
			}
		}

		#endregion

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x70_PlayerGroupUpdate_173(int capacity) : base(capacity)
		{
		}
	}
}
