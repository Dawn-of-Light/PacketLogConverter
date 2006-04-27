using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC7, -1, ePacketDirection.ClientToServer, "Player sit request")]
	public class CtoS_0xC7_PlayerSitRequest : Packet
	{
		protected byte status;
		protected byte unk1;
		protected ushort unk2;

		#region public access properties

		public byte Status { get { return status; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("status:{0} unk1:{1} unk2:{2}", status, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			status = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xC7_PlayerSitRequest(int capacity) : base(capacity)
		{
		}
	}
}