using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x46, -1, ePacketDirection.ServerToClient, "mino relic update on map")]
	public class StoC_0x46_UnknownPacket: Packet
	{
		protected uint id;
		protected uint regionId;
		protected uint x;
		protected uint y;
		protected uint z;

		#region public access properties

		public uint Id { get { return id; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("id:{0,-2} regionId:{1,-3} x:{2,-6} y:{3,-6} z:{4,-5}", id, regionId, x, y, z);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			id = ReadIntLowEndian();
			regionId = ReadIntLowEndian();
			x = ReadIntLowEndian();
			y = ReadIntLowEndian();
			z = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x46_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}