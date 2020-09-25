using System;
using System.Collections;
using System.IO;
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

		#region Filter Helpers

		protected PlayerStatusData[] playerStatusData;
		public PlayerStatusData[] InPlayerStatusData
		{
			get
			{
				if (playerStatusData == null)
				{
					ArrayList list = new ArrayList();
					foreach(object o in updates)
					{
						if (o is PlayerStatusData)
							list.Add(o as PlayerStatusData);
					}
					playerStatusData = (PlayerStatusData[])list.ToArray(typeof (PlayerStatusData));
				}
				return playerStatusData;
			}
		}

		protected PlayerBuffsData[] playerBuffsData;
		public PlayerBuffsData[] InPlayerBuffsData
		{
			get
			{
				if (playerBuffsData == null)
				{
					ArrayList list = new ArrayList();
					foreach(object o in updates)
					{
						if (o is PlayerBuffsData)
							list.Add(o as PlayerBuffsData);
					}
					playerBuffsData = (PlayerBuffsData[])list.ToArray(typeof (PlayerBuffsData));
				}
				return playerBuffsData;
			}
		}

		#endregion

		public enum eStatus: byte
		{
			Linkdead = 0x10, // *%s*
			Dead = 0x01, // =%s=
			Mezzed = 0x02, // !%s!
			Diseased = 0x04, // -%s-
			Poisoned = 0x08, // $%s$
			Nearsighted = 0x40, // ^%s^
			InAnotherRegion = 0x20, // [%s]
			Normal = 0x00,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			for (int i = 0; i < updates.Length; i++)
			{
				text.Write("\n\t");
				text.Write(updates[i].ToString());
			}
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

		public virtual object ReadPlayerBuffs(byte playerIndex)
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
