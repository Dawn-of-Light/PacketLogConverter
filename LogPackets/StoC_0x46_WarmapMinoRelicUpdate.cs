using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("id:{0,-2} regionId:{1,-3} x:{2,-6} y:{3,-6} z:{4,-5}", id, regionId, x, y, z);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			id = ReadIntLowEndian();      // 0x00
			regionId = ReadIntLowEndian();// 0x04
			x = ReadIntLowEndian();       // 0x08
			y = ReadIntLowEndian();       // 0x0C
			z = ReadIntLowEndian();       // 0x10
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