using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7C, 178, ePacketDirection.ClientToServer, "Player train request v178")]
	public class CtoS_0x7C_PlayerTrainRequest_178 : Packet
	{
		protected uint playerX;
		protected uint playerY;
		protected byte idLine;
		protected byte type;
		protected byte row;
		protected byte skillIndex;

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public byte IdLine { get { return idLine; } }
		public byte Type { get { return type; } }
		public byte Row { get { return row; } }
		public byte SkillIndex { get { return skillIndex; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("playerX:{0,-6} playerY:{1,-6} IdLine:{2,-2} type:{3} row:{4} skillIndex:{5}", playerX, playerY, idLine, type, row, skillIndex);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerX = ReadInt();    // 0x00
			playerY = ReadInt();    // 0x04
			idLine = ReadByte();    // 0x08
			type = ReadByte();      // 0x09
			row = ReadByte();       // 0x0A
			skillIndex = ReadByte();// 0x0B
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x7C_PlayerTrainRequest_178(int capacity) : base(capacity)
		{
		}
	}
}