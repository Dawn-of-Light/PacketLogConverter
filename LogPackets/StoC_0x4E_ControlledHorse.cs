using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x4E, -1, ePacketDirection.ServerToClient, "Controlled Horse")]
	public class StoC_0x4E_ControlledHorse: Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected byte horseId;
		protected byte horseBoot;
		protected ushort horseBootColor; //Color/Emblem
		protected byte horseSaddle;
		protected byte horseSaddleColor;
		protected byte horseSlots;
		protected byte horseArmor;
		protected string horseName;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public byte HorseId { get { return horseId; } }
		public byte HorseBoot { get { return horseBoot; } }
		public ushort HorseBootColor { get { return horseBootColor; } }
		public byte HorseSaddle { get { return horseSaddle; } }
		public byte HorseSaddleColor { get { return horseSaddleColor; } }
		public byte HorseSlots { get { return horseSlots; } }
		public byte HorseArmor { get { return horseArmor; } }
		public string HorseName { get { return horseName; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} horseId:{1,-2} horseBoot:{2,-2} BootColor:0x{3:X4} horseSaddle:{4,-2} SaddleColor:0x{5:X2}",
				oid, horseId, horseBoot, horseBootColor, horseSaddle, horseSaddleColor);
			if (oid == 0 && horseId != 0)
				text.Write(" slots:{0} weight:{1} name:\"{2}\"", horseSlots, horseArmor, horseName);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			horseId = ReadByte();
			horseBoot = ReadByte();
			horseBootColor = ReadShort();
			horseSaddle = ReadByte();
			horseSaddleColor = ReadByte();
			if (oid == 0 && horseId != 0)
			{
				horseSlots = ReadByte();
				horseArmor = ReadByte();
				horseName = ReadPascalString();
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x4E_ControlledHorse(int capacity) : base(capacity)
		{
		}
	}
}