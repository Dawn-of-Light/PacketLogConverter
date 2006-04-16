using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7C, -1, ePacketDirection.ClientToServer, "Player train request")]
	public class CtoS_0x7C_PlayerTrainRequest : Packet
	{
		protected uint playerX;
		protected uint playerY;
		protected ushort sessionId;
		protected byte unk1;
		protected byte skillIndex;

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public ushort SessionId { get { return sessionId; } }
		public byte Unk1 { get { return unk1; } }
		public byte SkillIndex { get { return skillIndex; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("playerX:{0,-6} playerY:{1,-6} sessionId:0x{2:X4} unk1:{3} skillIndex:{4}", playerX, playerY, sessionId, unk1, skillIndex);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerX = ReadInt();
			playerY = ReadInt();
			sessionId = ReadShort();
			unk1 = ReadByte();
			skillIndex = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x7C_PlayerTrainRequest(int capacity) : base(capacity)
		{
		}
	}
}