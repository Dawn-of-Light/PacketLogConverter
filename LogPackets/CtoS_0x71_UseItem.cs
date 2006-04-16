using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x71, -1, ePacketDirection.ClientToServer, "Use item")]
	public class CtoS_0x71_UseItem: Packet
	{
		protected ushort flagSpeedData;
		protected byte index;
		protected byte type;

		#region public access properties

		public ushort FlagSpeedData { get { return flagSpeedData; } }
		public byte Index { get { return index; } }
		public byte Type { get { return type; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("flag/speed:0x{0:X4}(flag=0x{1:X2} speed={2}) index:{3} type:{4}",
				flagSpeedData, (flagSpeedData & 0x1FF ^ flagSpeedData) >> 8 ,flagSpeedData & 0x1FF, index, type);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flagSpeedData = ReadShort();
			index = ReadByte();
			type = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x71_UseItem(int capacity) : base(capacity)
		{
		}
	}
}