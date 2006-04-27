using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE2, -1, ePacketDirection.ClientToServer, "Player Choose Emblem response")]
	public class CtoS_0xE2_PlayerChooseEmblemResponse: Packet
	{
		protected byte logo;
		protected byte pattern;
		protected byte color1;
		protected byte color2;

		#region public access properties

		public byte Logo { get { return logo; } }
		public byte Pattern { get { return pattern; } }
		public byte Color1 { get { return color1; } }
		public byte Color2 { get { return color2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("logo:{0,-3} pattern:{1} primaryColor:{2,-2} secondaryColor:{3}", logo, pattern, color1, color2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			color1 = ReadByte();
			color2 = ReadByte();
			pattern = ReadByte();
			logo = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xE2_PlayerChooseEmblemResponse(int capacity) : base(capacity)
		{
		}
	}
}