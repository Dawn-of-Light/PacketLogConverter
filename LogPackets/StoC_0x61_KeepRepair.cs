using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x61, -1, ePacketDirection.ServerToClient, "Keep/Tower shortinfo")]
	public class StoC_0x61_KeepRepair : Packet, IKeepIdPacket
	{
		protected ushort keepId;
		protected byte realm;
		protected byte hp; // ?
		protected byte level;
		protected byte targetLevel;
		protected byte keepType;
		protected string guild;
		protected byte unk1;
		// skip unknown (but where is WallID ? or it taked from CtoS 0x6F ?)

		public ushort[] KeepIds
		{
			get { return new ushort[] { keepId }; }
		}

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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
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

			text.Write("keepId:0x{0:X4} realm:{1} HP:{2,-3}% level:{3} to-level:{4} keepType:{5}({6}) guild:\"{7}\"",
				keepId, realm, hp, level, targetLevel, keepType, type, guild);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort();    // 0x00
			realm = ReadByte();      // 0x02
			hp = ReadByte();         // 0x03
			level = ReadByte();      // 0x04
			targetLevel = ReadByte();// 0x05
			keepType = ReadByte();   // 0x06
			guild = ReadString(32);  // 0x07+
			unk1 = ReadByte();       // 0x27
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