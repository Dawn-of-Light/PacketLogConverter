using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("flagSpeedData:0x{0:X4} index:{1,-2} type:{2}", flagSpeedData, index, type);
			if (flagsDescription)
			{
				string speed = (flagSpeedData & 0x1FF).ToString();
				if ((flagSpeedData & 0x200) == 0x200)
					speed = "-" + speed;
				if ((flagSpeedData & 0x400) == 0x400)
					speed += ",UNKx0400";
				if ((flagSpeedData & 0x800) == 0x800)
					speed += ",PetInView";
				if ((flagSpeedData & 0x1000) == 0x1000)
					speed += ",GTinView";
				if ((flagSpeedData & 0x2000) == 0x2000)
					speed += ",CheckTargetInView";
				if ((flagSpeedData & 0x4000) == 0x4000)
					speed += ",Strafe";// Swim under water
				if ((flagSpeedData & 0x8000) == 0x8000)
					speed += ",TargetInView";
				text.Write(" (speed:{0})", speed);
			}

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