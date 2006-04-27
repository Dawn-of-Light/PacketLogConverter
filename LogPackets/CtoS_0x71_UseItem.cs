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

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			string speed = (flagSpeedData & 0x1FF).ToString();
			if ((flagSpeedData & 0x200) == 0x200)
				speed = "-" + speed;
			if ((flagSpeedData & 0x800) == 0x800)
				speed += ",PetInView";
			if ((flagSpeedData & 0x1000) == 0x1000)
				speed += ",GTinView";
			if ((flagSpeedData & 0x4000) == 0x4000)
				speed += ",Strafe";// Swim under water
			if ((flagSpeedData & 0xA000) == 0xA000)
				speed += ",TargetInView";
			if ((flagSpeedData & 0xA000) == 0x8000)
				speed += ",TargetInViewLOSnot";
			if ((flagSpeedData & 0xA000) == 0x2000)
				speed += ",TargetNotInViewLOSInView";
			str.AppendFormat("flag/speed:0x{0:X4} index:{3} type:{4} (flag=0x{1:X2} speed={2})",
				flagSpeedData, (flagSpeedData & 0x1FF ^ flagSpeedData) >> 8 ,speed, index, type);

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