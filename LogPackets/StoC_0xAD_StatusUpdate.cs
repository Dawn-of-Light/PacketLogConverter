using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAD, -1, ePacketDirection.ServerToClient, "Status update")]
	public class StoC_0xAD_StatusUpdate : Packet
	{
		protected byte healthPercent;
		protected byte manaPercent;
		protected ushort alive;
		protected byte sitting;
		protected byte endurancePercent;
		protected byte concentrationPercent;
		protected byte unk1;

		#region public access properties

		public byte HealthPercent { get { return healthPercent; } }
		public byte ManaPercent { get { return manaPercent; } }
		public ushort Alive { get { return alive; } }
		public byte Sitting { get { return sitting; } }
		public byte EndurancePercent { get { return endurancePercent; } }
		public byte ConcentrationPercent { get { return concentrationPercent; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("health:{0,3}% mana:{1,3}% endu:{4,3}% conc:{5,3}% alive:0x{2:X4} sitting:{3} unk1:{6}",
				healthPercent, manaPercent, alive, sitting, endurancePercent, concentrationPercent, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			healthPercent = ReadByte();
			manaPercent = ReadByte();
			alive = ReadShort();
			sitting = ReadByte();
			endurancePercent = ReadByte();
			concentrationPercent = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xAD_StatusUpdate(int capacity) : base(capacity)
		{
		}
	}
}