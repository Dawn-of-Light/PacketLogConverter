using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x86, -1, ePacketDirection.ServerToClient, "Find group member window update")]
	public class StoC_0x86_FindGroupMemberUpdate: Packet
	{
		protected byte count;
		protected Player[] players;

		#region public access properties

		public byte Count { get { return count; } }
		public Player[] Players{ get { return players; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("count:{0}", count);
			for (int i = 0; i < count; i++)
			{
				WritePlayerInfo(i, str);
			}
			return str.ToString();
		}

		protected virtual void WritePlayerInfo(int i, StringBuilder str)
		{
			Player player = (Player)players[i];
			str.AppendFormat("\n\t\tindex:{0,-2} level:{1,-2} class:{2,4} zoneId:{3,-3} duration:0x{4:X2} objective:0x{5:X2} Unk1:{6} Unk2:{7} flagGrp:{8} Unk3:{9} name:{10}",
				player.index, player.level, player.className, player.zone, player.duration, player.objective, player.unk1, player.unk2, player.flagGrp, player.unk3, player.name);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			count = ReadByte();
			players = new Player[count];
			for (int i = 0; i < count; i++)
			{
				ReadPlayerInfo(i);
			}
		}

		protected virtual void ReadPlayerInfo(int index)
		{
			Player player = new Player();

			player.index = ReadByte();
			player.level = ReadByte();
			player.name = ReadPascalString();
			player.className = ReadString(4);
			player.zone = ReadByte();
			player.duration = ReadByte();
			player.objective = ReadByte();
			player.unk1 = ReadByte();
			player.unk2 = ReadByte();
			player.flagGrp = ReadByte();
			player.unk3 = ReadByte();

			players[index] = player;
		}

		public struct Player
		{
			public byte index;
			public byte level;
			public string name;
			public string className;
			public ushort zone;
			public byte duration;
			public byte objective;
			public byte unk1;
			public byte unk2;
			public byte flagGrp;
			public byte unk3;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x86_FindGroupMemberUpdate(int capacity) : base(capacity)
		{
		}
	}
}