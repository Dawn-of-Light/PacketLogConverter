using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x62, -1, ePacketDirection.ServerToClient, "Keep/Tower claim")]
	public class StoC_0x62_KeepClaim : Packet
	{
		protected ushort keepId;
		protected ushort realm;
		protected ushort level;

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort Realm { get { return realm; } }
		public ushort Level { get { return level; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("keepId:0x{0:X4} realm:{1} level:{2}",
				keepId, realm, level);
			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();
			realm = ReadShort();
			level = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x62_KeepClaim(int capacity) : base(capacity)
		{
		}
	}
}