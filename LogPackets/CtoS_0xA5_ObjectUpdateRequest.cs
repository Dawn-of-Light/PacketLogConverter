using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA5, -1, ePacketDirection.ClientToServer, "Update all GameObjects in visibility range")]
	public class CtoS_0xA5_ObjectUpdateRequest : Packet
	{
		private byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "unk1:0x"+unk1.ToString("X2");
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
		public CtoS_0xA5_ObjectUpdateRequest(int capacity) : base(capacity)
		{
		}
	}
}