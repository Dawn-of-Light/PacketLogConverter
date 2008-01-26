using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x66, -1, ePacketDirection.ServerToClient, "Warmap bonuses")]
	public class StoC_0x66_WarmapBonuses : Packet
	{
		protected byte keeps;
		protected byte relics;
		protected byte ownerDFrealm;

		#region public access properties

		public byte Keeps { get { return keeps; } }
		public byte Relics { get { return relics; } }
		public byte OwnerDF { get { return ownerDFrealm; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keeps:{0} relics:0x{1:X2}(power:{3} strength:{4}) DFownerRealm:{2}",
				keeps, relics, ownerDFrealm, relics >> 4, relics & 0xF);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keeps = ReadByte();
			relics = ReadByte();
			ownerDFrealm = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x66_WarmapBonuses(int capacity) : base(capacity)
		{
		}
	}
}