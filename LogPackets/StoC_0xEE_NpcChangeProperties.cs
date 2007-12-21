using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEE, -1, ePacketDirection.ServerToClient, "NPC change properties")]
	public class StoC_0xEE_NpcChangeProperties : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected ushort level;
		protected string guildName;
		protected string name;
		protected byte trailingByte;

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
		public ushort Level { get { return level; } }
		public byte TrailingByte { get { return trailingByte; } }
		public string GuildName { get { return guildName; } }
		public string Name { get { return name; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			if (trailingByte == 0xFF)
				str.AppendFormat("oid:0x{0:X4} level:{1,-3} trailingByte:0x{2:X2}", oid, level, trailingByte);
			else
				str.AppendFormat("oid:0x{0:X4} level:{1,-3} guildName:\"{2}\" name:\"{3}\" trailingByte:0x{4:X2}",
			                 oid, level, guildName, name, trailingByte);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadShort();
			level = ReadShort();
			long tempPosition = Position;
			trailingByte = ReadByte();
			if (trailingByte == 0xFF)
			{
				guildName = "";
				name = "";
			}
			else
			{
				Position = tempPosition;
				guildName = ReadPascalString();
				name = ReadPascalString();
				if (Position < Length)
					trailingByte = ReadByte();
				else
					trailingByte = 0;
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xEE_NpcChangeProperties(int capacity) : base(capacity)
		{
		}
	}
}