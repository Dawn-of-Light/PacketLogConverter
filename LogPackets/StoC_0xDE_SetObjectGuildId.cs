using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDE, -1, ePacketDirection.ServerToClient, "Set object guild id")]
	public class StoC_0xDE_SetObjectGuildId : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected ushort guildId;
		protected ushort unk1;
		protected byte serverId;
		protected byte unk2;

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
		public byte ServerId { get { return serverId; } }
		public byte Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} guildId:0x{1:X4} unk1:0x{2:X4} serverId:0x{3:X2} unk2:0x{4:X2}", oid, guildId, unk1, serverId, unk2);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();     // 0x00
			guildId = ReadShort(); // 0x02
			unk1 = ReadShort();    // 0x04
			serverId = ReadByte(); // 0x06
			unk2 = ReadByte();     // 0x07
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