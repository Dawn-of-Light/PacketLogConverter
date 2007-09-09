using System.Collections;
namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x70, 169, ePacketDirection.ServerToClient, "Player group update v169")]
	public class StoC_0x70_PlayerGroupUpdate_169: StoC_0x70_PlayerGroupUpdate
	{
		#region Filter Helpers

		protected PlayerStatusData_169[] playerStatusData_169;
		public PlayerStatusData_169[] InPlayerStatusData_169
		{
			get
			{
				if (playerStatusData_169 == null)
				{
					ArrayList list = new ArrayList();
					foreach(object o in updates)
					{
						if (o is PlayerStatusData_169)
							list.Add(o as PlayerStatusData_169);
					}
					playerStatusData_169 = (PlayerStatusData_169[])list.ToArray(typeof (PlayerStatusData_169));
				}
				return playerStatusData_169;
			}
		}

		#endregion

		protected override object ReadPlayerStatus(byte playerIndex)
		{
			PlayerStatusData_169 data = new PlayerStatusData_169();

			data.playerIndex = (byte)((playerIndex&0x0F) - 1);
			data.health = ReadByte();
			data.mana = ReadByte();
			data.endurance = ReadByte();
			data.status = ReadByte();

			return data;
		}

		public class PlayerStatusData_169 : PlayerStatusData
		{
			public byte endurance = byte.MaxValue;

			public override string ToString()
			{
				return string.Format("player{0}: health:{1,3}% mana:{2,3}% endurance:{3,3}% status:0x{4:X2}", playerIndex, health, mana, endurance, status);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x70_PlayerGroupUpdate_169(int capacity) : base(capacity)
		{
		}
	}
}
