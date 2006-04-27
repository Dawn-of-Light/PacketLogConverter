using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x61, -1, ePacketDirection.ServerToClient, "Keep/Tower shortinfo")]
	public class StoC_0x61_KeepRepair : Packet
	{
		protected ushort keepId;
		protected byte realm;
		protected byte hp; // ?
		protected byte level;
		protected byte targetLevel;
		protected byte keepType; // height
		protected string guild;
		protected byte unk1;
		// skip unknown (but where is WallID ? or it taked from CtoS 0x6F ?)

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public byte Realm { get { return realm; } }
		public byte HP { get { return hp; } }
		public byte Level { get { return level; } }
		public byte TargetLevel { get { return targetLevel; } }
		public byte KeepType { get { return keepType; } }
		public byte Unk1 { get { return unk1; } }
		public string Guild { get { return guild; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{

			StringBuilder str = new StringBuilder();
			string type;
			switch (keepType)
			{
				case 0:
					type = "generic";
					break;
				case 1:
					type = "melee";
					break;
				case 2:
					type = "magic";
					break;
				case 4:
					type = "stealh";
					break;
				default:
					type = "unknown";
					break;
			}

			str.AppendFormat("keepId:0x{0:X4} realm:{1} HP:{2}% level:{3} to-level:{4} keepType:{5}({6}) guild:\"{7}\"",
				keepId, realm, hp, level, targetLevel, keepType, type, guild);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort();
			realm = ReadByte();
			hp = ReadByte();
			level = ReadByte();
			targetLevel = ReadByte();
			keepType = ReadByte();
			guild = ReadString(32);
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x61_KeepRepair(int capacity) : base(capacity)
		{
		}
	}
}