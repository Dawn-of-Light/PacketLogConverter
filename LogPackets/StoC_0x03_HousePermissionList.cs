using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x03, -1, ePacketDirection.ServerToClient, "House Friend Permission List")]
	public class StoC_0x03_HousePermissionList: Packet, IObjectIdPacket
	{
		protected byte count;
		protected byte unk1;
		protected ushort houseOid;
		protected Access[] m_permission;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { houseOid }; }
		}

		#region public access properties

		public byte Count { get { return count; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Oid { get { return houseOid; } }
		public Access[] Permission { get { return m_permission; } }

		#endregion

		public enum eLevelType: byte
		{
			None = 0,
			Visitor = 1,
			Guest = 2,
			Resident = 3,
			Tenant = 4,
			Acquintance = 5,
			Associate = 6,
			Friend = 7,
			Ally = 8,
			Partner = 9,
		}

		public enum eType: byte
		{
			None = 0,
			Player = 1,
			Guild = 2,
			GuildRank= 3,
			Account = 4,
			Everyone = 5,
			Class = 6,
			Race = 7
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("count:{0} unk1:0x{1:X2} houseOid:0x{2:X4}", count, unk1, houseOid);
			string levelDescription;
			string typeDescription;
			for (int i = 0; i < count; i++)
			{
				Access permission = (Access)m_permission[i];
				if (flagsDescription)
				{
					typeDescription = "("+(eType)permission.type+")";
					levelDescription = "("+(eLevelType)permission.level+")";
				}
				else
				{
					typeDescription = "";
					levelDescription = "";
				}
				str.AppendFormat("\n\tindex:{0,-2} unk1:0x{1:X4} type:{2}{5} level:{3}{6} name:\"{4}\"",
					permission.index, permission.unk1, permission.type, permission.level, permission.name, typeDescription, levelDescription);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			count = ReadByte();
			unk1 = ReadByte();
			houseOid = ReadShort();
			m_permission = new Access[count];
			for (int i = 0; i < count; i++)
			{
				Access permission = new Access();
				permission.index = ReadByte();
				permission.unk1 = ReadShort();
				permission.type = ReadByte();
				permission.level = ReadByte();
				permission.name = ReadPascalString();
				m_permission[i] = permission;
			}
		}

		public struct Access
		{
			public byte index;
			public ushort unk1;
			public byte type;
			public byte level;
			public string name;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x03_HousePermissionList(int capacity) : base(capacity)
		{
		}
	}
}