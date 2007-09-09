using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x5F, 190, ePacketDirection.ServerToClient, "Unknown packet")]
	public class StoC_0x5F_UnknownPacket: Packet
	{
		protected byte unk1;
		protected ushort sessionId;
		protected byte unk2;
		protected uint unk3;

		#region public access properties

		public byte Unk1 { get { return unk1; } }
		public ushort SessionId { get { return sessionId; } }
		public byte Unk2 { get { return unk2; } }
		public uint Unk3 { get { return unk3; } }

		#endregion
		public ushort[] ObjectIds
		{
			get
			{
				return new ushort[] { sessionId };
			}
		}


		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("sessionId:0x{0:X4} unk1:{1} unk2:{2} unk3:0x{3:X8}", sessionId, unk1, unk2, unk3);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadByte();
			sessionId = ReadShortLowEndian();
			unk2 = ReadByte();
			unk3 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x5F_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}