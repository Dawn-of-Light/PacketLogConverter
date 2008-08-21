using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAD, 190, ePacketDirection.ServerToClient, "Status update 190")]
	public class StoC_0xAD_StatusUpdate_190 : StoC_0xAD_StatusUpdate
	{
		protected ushort health;
		protected ushort maxHealth;
		protected ushort power;
		protected ushort maxPower;
		protected ushort endurance;
		protected ushort concentration;
		protected ushort maxConcentration;
		protected ushort maxEndurance;

		#region public access properties

		public ushort Health { get { return health; } }
		public ushort MaxHealth { get { return maxHealth; } }
		public ushort Power { get { return power; } }
		public ushort MaxPower { get { return maxPower; } }
		public ushort Concentration { get { return concentration; } }
		public ushort MaxConcentration { get { return maxConcentration; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);

			text.Write(" health:{0}/{1} power:{2}/{3} endurance:{4}/{5} concentration:{6}/{7}",
				health, maxHealth, power, maxPower, endurance, maxEndurance, concentration, maxConcentration);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			healthPercent = ReadByte();       // 0x00
			manaPercent = ReadByte();         // 0x01
			sitting = ReadByte();             // 0x02
			endurancePercent = ReadByte();    // 0x03
			concentrationPercent = ReadByte();// 0x04
			unk1 = ReadByte();                // 0x05
//			alive = ReadShort();
			maxPower = ReadShort();           // 0x06
			maxEndurance = ReadShort();       // 0x08
			maxConcentration = ReadShort();   // 0x0A
			maxHealth = ReadShort();          // 0x0C
			health = ReadShort();             // 0x0E
			endurance = ReadShort();          // 0x10
			power = ReadShort();              // 0x12
			concentration = ReadShort();      // 0x14
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xAD_StatusUpdate_190(int capacity) : base(capacity)
		{
		}
	}
}