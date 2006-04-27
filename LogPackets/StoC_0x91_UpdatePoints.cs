using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x91, -1, ePacketDirection.ServerToClient, "Update points")]
	public class StoC_0x91_UpdatePoints : Packet
	{
		protected uint realmPoints;
		protected ushort levelPermill;
		protected ushort skillSpecPoints;
		protected uint bountyPoints;
		protected ushort realmSpecPoints;
		protected ushort unk1;

		#region public access properties

		public uint RealmPoints { get { return realmPoints; } }
		public ushort LevelPermill { get { return levelPermill; } }
		public ushort SkillSpecPoints { get { return skillSpecPoints; } }
		public uint BountyPoints { get { return bountyPoints; } }
		public ushort RealmSpecPoints { get { return realmSpecPoints; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("realmPoints:{0,-8} levelPermill:{1,-6} skillSpecPoints:{2,-4} bountyPoints:{3,-6} realmSpecPoints:{4,-3} unk1:{5}",
				realmPoints, levelPermill, skillSpecPoints, bountyPoints, realmSpecPoints, unk1);

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
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x91_UpdatePoints(int capacity) : base(capacity)
		{
		}
	}
}