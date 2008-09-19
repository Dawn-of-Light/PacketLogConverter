using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("count:{0}", count);
			for (int i = 0; i < count; i++)
			{
				WritePlayerInfo(i, text, flagsDescription);
			}
		}

		protected virtual void WritePlayerInfo(int i, TextWriter text, bool flagsDescription)
		{
			Player player = (Player)players[i];
			text.Write("\n\t\tindex:{0,-2} level:{1,-2} class:{2,4} zoneId:{3,-3} duration:{4}",
				player.index, player.level, player.className, player.zone, player.duration);
			if (flagsDescription)
			{
				string durationStr;
				switch(player.duration)
				{
					case 0:
						durationStr = "Unspecified";
						break;
					case 1:
						durationStr = "1 Hour";
						break;
					case 2:
						durationStr = "2 Hours";
						break;
					case 3:
						durationStr = "3 Hours";
						break;
					case 4:
						durationStr = "4 Hours";
						break;
					case 5:
						durationStr = "5 Hours+";
						break;
					default:
						durationStr = "Unknown";
						break;
				}
				text.Write("(\"");
				text.Write(durationStr);
				text.Write("\")");
			}
			text.Write(" objective:{0}", player.objective);
			if (flagsDescription)
			{
				string objectiveStr;
				switch(player.objective)
				{
					case 0:
						objectiveStr = "";
						break;
					case 1:
						objectiveStr = "RVR";
						break;
					case 2:
						objectiveStr = "Darkness Falls";
						break;
					case 3:
						objectiveStr = "EXP";
						break;
					case 4:
						objectiveStr = "Epic Raid";
						break;
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
						objectiveStr = "Master Level " + player.duration.ToString("D");
						break;
					default:
						objectiveStr = "Unknown";
						break;
				}
				text.Write("(\"");
				text.Write(objectiveStr);
				text.Write("\")");
			}
			text.Write(" unk1:{0} unk2:{1} flagGrp:{2} unk3:{3} name:{4}",
				player.unk1, player.unk2, player.flagGrp, player.unk3, player.name);
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

			player.index = ReadByte(); // max 50 ?
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