using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x05, -1, ePacketDirection.ClientToServer, "House Friend Level permission request")]
	public class CtoS_0x05_HousePermissionRequest: Packet, IHouseIdPacket
	{
		protected ushort unk1;
		protected ushort houseOid;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort HouseId { get { return houseOid; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:0x{0:X4} houseOid:0x{1:X4}", unk1, houseOid);
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
		public CtoS_0x05_HousePermissionRequest(int capacity) : base(capacity)
		{
		}
	}
}