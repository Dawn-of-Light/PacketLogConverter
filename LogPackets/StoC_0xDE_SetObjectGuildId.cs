using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDE, -1, ePacketDirection.ServerToClient, "Set object guild id")]
	public class StoC_0xDE_SetObjectGuildId : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected ushort guildId;
		protected ushort unk1;
		protected ushort unk2;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort GuildId { get { return guildId; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} guildId:0x{1:X4} unk1:0x{2:X4} unk2:0x{3:X4}", oid, guildId, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			guildId = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDE_SetObjectGuildId(int capacity) : base(capacity)
		{
		}
	}
}