using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 188, ePacketDirection.ClientToServer, "Region list request v188")]
	public class CtoS_0x9D_RegionListRequest_188 : CtoS_0x9D_RegionListRequest_183
	{
		public override sbyte RaceID
		{
			get
			{
				sbyte rc = (sbyte)(race - 150);
				return (sbyte)(rc > 0 ? rc : rc + 50);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_188(int capacity) : base(capacity)
		{
		}
	}
}