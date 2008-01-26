using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x2B, -1, ePacketDirection.ClientToServer, "Player Wolrd initialized")]
	public class CtoS_0x2B_PlayerWorldInialized: Packet
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1 ; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:{0}", unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x2B_PlayerWorldInialized(int capacity) : base(capacity)
		{
		}
	}
}