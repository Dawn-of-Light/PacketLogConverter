using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x90, -1, ePacketDirection.ClientToServer, "Region change request")]
	public class CtoS_0x90_RegionChangeRequest : Packet
	{
		protected ushort jumpSpotId;
		protected ushort unk1;

		#region public access properties

		public ushort JumpSpotId { get { return jumpSpotId; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("jumpSpotId:{0} unk1:0x{1:X4}", jumpSpotId, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			jumpSpotId = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x90_RegionChangeRequest(int capacity) : base(capacity)
		{
		}
	}
}