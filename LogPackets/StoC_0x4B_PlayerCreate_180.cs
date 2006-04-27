using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4B, 180, ePacketDirection.ServerToClient, "Player create v180")]
	public class StoC_0x4B_PlayerCreate_180 : StoC_0x4B_PlayerCreate_174
	{
		protected byte horseId;
		protected byte horseBoot;
		protected ushort horseBootColor; //Color/Emblem
		protected byte horseSaddle;
		protected byte horseSaddleColor;

		#region public access properties

		public byte HorseId { get { return horseId; } }
		public byte HorseBoot { get { return horseBoot; } }
		public ushort HorseBootColor { get { return horseBootColor; } }
		public byte HorseSaddle { get { return horseSaddle; } }
		public byte HorseSaddleColor { get { return horseSaddleColor; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			string str = base.GetPacketDataString(flagsDescription);
			str += string.Format(" horseId:{0}", horseId);
			if (horseId != 0)
				str += string.Format(" horseBoot:{0,-2} BootColor:0x{1:X4} horseSaddle:{2,-2} SaddleColor:0x{3:X2}",
					horseBoot, horseBootColor, horseSaddle, horseSaddleColor);
			return str;
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			base.Init();
			horseId = ReadByte();
			if(horseId != 0)
			{
				horseBoot = ReadByte();
				horseBootColor = ReadShort();
				horseSaddle = ReadByte();
				horseSaddleColor = ReadByte();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4B_PlayerCreate_180(int capacity) : base(capacity)
		{
		}
	}
}