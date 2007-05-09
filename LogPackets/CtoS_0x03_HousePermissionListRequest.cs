using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x03, -1, ePacketDirection.ClientToServer, "House Friend Permission List request")]
	public class CtoS_0x03_HousePermissionListRequest: Packet, IObjectIdPacket
	{
		protected ushort unk1;
		protected ushort houseOid;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { houseOid }; }
		}

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort Oid { get { return houseOid; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X4} houseOid:0x{1:X4}", unk1, houseOid);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			houseOid = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x03_HousePermissionListRequest(int capacity) : base(capacity)
		{
		}
	}
}