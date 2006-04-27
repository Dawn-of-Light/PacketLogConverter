using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD4, -1, ePacketDirection.ClientToServer, "World init request")]
	public class CtoS_0xD4_WorldInitRequest : Packet
	{
		protected uint unk1;
		protected uint unk2;
		protected short regionId;

		#region public access properties
		
		public short RegionId { get { return regionId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("unk1:0x{0:X8} unk2:0x{1:X8} regionId:{2,-3}", unk1, unk2, regionId);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadInt();
			unk2 = ReadInt();
			regionId = (byte)ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD4_WorldInitRequest(int capacity) : base(capacity)
		{
		}
	}
}