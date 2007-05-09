using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x62, -1, ePacketDirection.ServerToClient, "Keep/Tower claim")]
	public class StoC_0x62_KeepClaim : Packet, IObjectIdPacket
	{
		protected ushort keepId;
		protected byte permission;
		protected byte keepType;
		protected byte targetLevel;
		protected byte level;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public byte Permission { get { return permission; } }
		public byte KeepType { get { return keepType; } }
		public byte TargetLevel { get { return targetLevel; } }
		public byte Level { get { return level; } }

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
					type = "stealth";
					break;
				default:
					type = "unknown";
					break;
			}
			str.AppendFormat("keepId:0x{0:X4} permission:{1} keepType:{2}({5}) to-level:{3} level:{4}",
				keepId, permission, keepType, targetLevel, level, type);
			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();
			permission = ReadByte();
			keepType = ReadByte();
			targetLevel = ReadByte();
			level = ReadByte();
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