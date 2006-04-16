using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDA, 171, ePacketDirection.ServerToClient, "Npc create v171")]
	public class StoC_0xDA_NpcCreate_171 : StoC_0xDA_NpcCreate
	{
		protected byte flag2;
		protected ushort unk1_171;
		protected byte instance;

		#region public access properties
		public byte Flag2 {get {return flag2; } }
		public ushort Unk1_171 { get { return unk1_171; } }
		public byte Instance {get {return instance; } }

		#endregion

		public override string GetPacketDataString()
		{
			Position = 0;

			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} speed:{1,-3} heading:0x{2:X4} x:{3,-6} y:{4,-6} z:{5,-5} speedZ:{6, -3} model:0x{7:X4} size:{8,-3} level:{9,-3} flags:0x{10:X2}(realm:{18},0x{19:X2}) maxStick:{11,-3} flag2:0x{12:X2} unk1_171:0x{13:X4} instance:0x{14:X2} name:\"{15}\" guild:\"{16}\" unk1:{17}",
			                 oid, speed, heading, x, y, z, speedZ, model, size, level, flags, maxStick, flag2, unk1_171, instance, name, guildName, unk1, flags >> 6, flags & 0x3F);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			speed = ReadShort();
			heading = ReadShort();
			z = ReadShort();
			x = ReadInt();
			y = ReadInt();
			speedZ = (short)ReadShort();
			model = ReadShort();
			size = ReadByte();
			level = ReadByte();
			flags = ReadByte();
			maxStick = ReadByte();
			flag2 = ReadByte();
			unk1_171 = ReadShort();
			instance = ReadByte();
			name = ReadPascalString();
			guildName = ReadPascalString();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDA_NpcCreate_171(int capacity) : base(capacity)
		{
		}
	}
}