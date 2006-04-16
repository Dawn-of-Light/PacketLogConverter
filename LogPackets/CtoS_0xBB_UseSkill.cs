using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBB, -1, ePacketDirection.ClientToServer, "Use skill")]
	public class CtoS_0xBB_UseSkill : Packet
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

			str.AppendFormat("falgs/speed:0x{0:X4} index:{1,-2} type:{2}", flagSpeedData, index, type);

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
		public CtoS_0xBB_UseSkill(int capacity) : base(capacity)
		{
		}
	}
}