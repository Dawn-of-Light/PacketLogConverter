using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x71, -1, ePacketDirection.ClientToServer, "Use item")]
	public class CtoS_0x71_UseItem: Packet
	{
		protected ushort flagSpeedData;
		protected byte slot;
		protected byte type;

		#region public access properties

		public ushort FlagSpeedData { get { return flagSpeedData; } }
		public byte Slot { get { return slot; } }
		public byte Type { get { return type; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("flagSpeedData:0x{0:X4} slot:{1} type:{2}", flagSpeedData, slot, type);
			if (flagsDescription)
			{
				str.AppendFormat(" (speed:{0}{1}", (flagSpeedData & 0x200) == 0x200 ? "-" : "", flagSpeedData & 0x1FF);
				if ((flagSpeedData & 0x800) == 0x800)
					str.Append(",PetInView");
				if ((flagSpeedData & 0x1000) == 0x1000)
					str.Append(",GTinView");
				if ((flagSpeedData & 0x4000) == 0x4000)
					str.Append(",Strafe");// Swim under water
				if ((flagSpeedData & 0x2000) == 0x2000)
					str.Append(",CheckTargetInView");
				if ((flagSpeedData & 0x8000) == 0x8000)
					str.Append(",TargetInView");
				str.Append(')');
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flagSpeedData = ReadShort();
			slot = ReadByte();
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