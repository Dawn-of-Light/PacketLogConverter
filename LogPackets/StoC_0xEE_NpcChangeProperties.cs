using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEE, -1, ePacketDirection.ServerToClient, "NPC change properties")]
	public class StoC_0xEE_NpcChangeProperties : Packet, IObjectIdPacket
	{
		protected ushort npcOid;
		protected byte unk1;
		protected byte level;
		protected string guildName;
		protected string lastName;
		protected byte trailingByte;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { npcOid }; }
		}

		#region public access properties

		public ushort NpcOid { get { return npcOid; } }
		public byte Unk1 { get { return unk1; } }
		public byte Level { get { return level; } }
		public byte TrailingByte { get { return trailingByte; } }
		public string GuildName { get { return guildName; } }
		public string LastName { get { return lastName; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			if (trailingByte == 0xFF)
				str.AppendFormat("oid:0x{0:X4} unk1:0x{1:X2} level:{2,-2} trailingByte:0x{3:X2}", npcOid, unk1, level, trailingByte);
			else
				str.AppendFormat("oid:0x{0:X4} unk1:0x{1:X2} level:{2,-2} guildName:\"{3}\" lastName:\"{4}\" trailingByte:0x{5:X2}",
			                 npcOid, unk1, level, guildName, lastName, trailingByte);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			npcOid = ReadShort();
			unk1 = ReadByte();
			level = ReadByte();
			long tempPosition = Position;
			trailingByte = ReadByte();
			if (trailingByte == 0xFF)
			{
				guildName = "";
				lastName = "";
			}
			else
			{
				Position = tempPosition;
				guildName = ReadPascalString();
				lastName = ReadPascalString();
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