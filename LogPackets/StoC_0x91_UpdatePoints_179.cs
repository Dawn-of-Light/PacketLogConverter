using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x91, 179, ePacketDirection.ServerToClient, "Update points 179")]
	public class StoC_0x91_UpdatePoints_179 : StoC_0x91_UpdatePoints
	{
		protected ushort champLevelPermill;

		#region public access properties

		public ushort ChampLevelPermill { get { return champLevelPermill; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("realmPoints:{0,-8} levelPermill:{1:00.0}%  skillSpecPoints:{2,-4} bountyPoints:{3,-6} realmSpecPoints:{4,-3} champLevelPermill:{5:00.0}%",
				realmPoints, 0.1 * levelPermill, skillSpecPoints, bountyPoints, realmSpecPoints, 0.1 * champLevelPermill);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			realmPoints = ReadInt();
			levelPermill = ReadShort();
			skillSpecPoints = ReadShort();
			bountyPoints = ReadInt();
			realmSpecPoints = ReadShort();
			champLevelPermill = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x91_UpdatePoints_179(int capacity) : base(capacity)
		{
		}
	}
}