using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC9, -1, ePacketDirection.ServerToClient, "GS_F_PLAY_SOUND?")]
	public class StoC_0xC9_PlaySound : Packet
	{
		protected ushort unk1;
		protected ushort zoneId;
		protected ushort x;
		protected ushort y;
		protected ushort z;
		protected ushort radius;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort ZoneId { get { return zoneId; } }
		public ushort X { get { return x; } }
		public ushort Y { get { return y; } }
		public ushort Z { get { return z; } }
		public ushort Radius { get { return radius; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("id?:{0:X4} zoneId:{1,-3} x:{2,-5} y:{3,-5} z:{4,-5} radius:{5}",
				unk1, zoneId, x, y, z, radius);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			zoneId = ReadShort();
			x = ReadShort();
			y = ReadShort();
			z = ReadShort();
			radius = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC9_PlaySound(int capacity) : base(capacity)
		{
		}
	}
}