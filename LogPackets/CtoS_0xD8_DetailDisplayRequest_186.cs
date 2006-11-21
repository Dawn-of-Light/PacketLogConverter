using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD8, 186, ePacketDirection.ClientToServer, "Detail display request 186")]
	public class CtoS_0xD8_DetailDisplayRequest_186 : CtoS_0xD8_DetailDisplayRequest
	{
		protected uint unk1;

		#region public access properties

		public uint Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("objectType:0x{0:X4} objectId:{1:X4} unk1:{2:X8}", objectType, objectId, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectType = ReadShort();
			unk1 = ReadInt();
			objectId = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD8_DetailDisplayRequest_186(int capacity) : base(capacity)
		{
		}
	}
}