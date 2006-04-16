using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF8, -1, ePacketDirection.ClientToServer, "Effect remove request")]
	public class CtoS_0xF8_EffectRemoveRequest : Packet
	{
		protected ushort internalId;
		protected ushort unk1;

		#region public access properties

		public ushort InternalId { get { return internalId; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("internalId:{0,-5} unk1:0x{1:X4}", internalId, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			internalId = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xF8_EffectRemoveRequest(int capacity) : base(capacity)
		{
		}
	}
}