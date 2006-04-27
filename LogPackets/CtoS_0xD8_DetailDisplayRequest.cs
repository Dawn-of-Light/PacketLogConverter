using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD8, -1, ePacketDirection.ClientToServer, "Detail display request")]
	public class CtoS_0xD8_DetailDisplayRequest : Packet
	{
		protected ushort objectType;
		protected ushort objectId;

		#region public access properties

		public ushort ObjectType { get { return objectType; } }
		public ushort ObjectId { get { return objectId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("objectType:0x{0:X4} objectId:{1}(0x{1:X4})", objectType, objectId);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectType = ReadShort();
			objectId = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD8_DetailDisplayRequest(int capacity) : base(capacity)
		{
		}
	}
}