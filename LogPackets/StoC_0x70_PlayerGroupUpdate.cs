using System;
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x70, -1, ePacketDirection.ServerToClient, "Player group update")]
	public class StoC_0x70_PlayerGroupUpdate : Packet
	{
		protected object[] updates;
		protected byte playerIndex;

		#region public access properties

		public object[] Updates { get { return updates; } }
		public byte PlayerIndex { get { return (byte)(playerIndex & 0xF); } }
		public byte Type { get { return (byte)(playerIndex & 0xF0); } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			for (int i = 0; i < updates.Length; i++)
			{
				str.Append("\n\t");
				str.Append(updates[i].ToString());
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			ArrayList groupUpdates = new ArrayList();
			playerIndex = ReadByte();
			while (playerIndex != 0)
			{
				switch (playerIndex & 0xF0)
				{
					case 0x00: groupUpdates.Add(ReadPlayerStatus(playerIndex)); break;
					case 0x80: groupUpdates.Add(ReadPlayerBuffs(playerIndex)); break;
					default: throw new Exception("unknow player position code:0x"+(playerIndex & 0xF0).ToString("X2"));
				}
				playerIndex = ReadByte();
			}

			updates = (object[])groupUpdates.ToArray(typeof (object));
		}

		#region ReadPlayerStatus

		protected virtual object ReadPlayerStatus(byte playerIndex)
		{
			PlayerStatusData data = new PlayerStatusData();

			data.playerIndex = (byte)((playerIndex&0x0F) - 1);
			data.health = ReadByte();
			data.mana = ReadByte();
			data.status = ReadByte();

			return data;
		}

		public class PlayerStatusData
		{
			public byte playerIndex = byte.MaxValue;
			public byte health = byte.MaxValue;
			public byte mana = byte.MaxValue;
			public byte status = byte.MaxValue;

			public override string ToString()
			{
				return string.Format("player{0}: health:{1,3}% mana:{2,3}% status:0x{3:X2}", playerIndex, health, mana, status);
			}
		}

		#endregion

		#region ReadPlayerBuffs

		public object ReadPlayerBuffs(byte playerIndex)
		{
			PlayerBuffsData buffsData = new PlayerBuffsData();

			buffsData.playerIndex = (byte)(playerIndex & 0x0F);
			buffsData.count = ReadByte();
			BuffData[] buffs = buffsData.buffs = new BuffData[buffsData.count];
			for (int i = 0; i < buffsData.count; i++)
			{
				buffs[i].type = ReadByte();
				buffs[i].spellId = ReadShort();
			}

			return buffsData;
		}

		public class PlayerBuffsData
		{
			public byte playerIndex;
			public byte count;
			public BuffData[] buffs;

			public override string ToString()
			{
				StringBuilder str = new StringBuilder(32);

				str.AppendFormat("player{0}: effects count:{1} (", playerIndex, count);
				for (int i = 0; i < count; i++)
				{
					if (i>0) str.Append(',');
					str.AppendFormat("type:{0} 0x{1:X4}", buffs[i].type, buffs[i].spellId);
				}
				str.Append(")");

				return str.ToString();
			}
		}

		public struct BuffData
		{
			public byte type;
			public ushort spellId;
		}

		#endregion

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x70_PlayerGroupUpdate(int capacity) : base(capacity)
		{
		}
	}
}
