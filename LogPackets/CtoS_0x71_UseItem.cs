using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("flagSpeedData:0x{0:X4} slot:{1} type:{2}", flagSpeedData, slot, type);
			if (flagsDescription)
			{
				text.Write(" (speed:{0}{1}", (flagSpeedData & 0x200) == 0x200 ? "-" : "", flagSpeedData & 0x1FF);
				if ((flagSpeedData & 0x800) == 0x800)
					text.Write(",PetInView");
				if ((flagSpeedData & 0x1000) == 0x1000)
					text.Write(",GTinView");
				if ((flagSpeedData & 0x4000) == 0x4000)
					text.Write(",Strafe");// Swim under water
				if ((flagSpeedData & 0x2000) == 0x2000)
					text.Write(",CheckTargetInView");
				if ((flagSpeedData & 0x8000) == 0x8000)
					text.Write(",TargetInView");
				text.Write(')');
			}
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