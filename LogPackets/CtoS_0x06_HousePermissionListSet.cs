using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x06, -1, ePacketDirection.ClientToServer, "House permission list set")]
	public class CtoS_0x06_HousePermissionListSet: Packet, IHouseIdPacket
	{
		protected byte index;
		protected byte level;
		protected ushort houseOid;

		#region public access properties

		public byte Index { get { return index; } }
		public byte Level { get { return level; } }
		public ushort HouseId { get { return houseOid; } }

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
			Remove = 100
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			string levelDescription = "";
			if (flagsDescription)
			{
				levelDescription = "("+(eLevelType)level+")";
			}
			str.AppendFormat("index:{0,-2} level:{1}{3} houseOid:0x{2:X4}", index, level, houseOid, levelDescription);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			index = ReadByte();
			level = ReadByte();
			houseOid = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x06_HousePermissionListSet(int capacity) : base(capacity)
		{
		}
	}
}