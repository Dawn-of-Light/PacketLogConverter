using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x6E, 190, ePacketDirection.ServerToClient, "Unknown packet")]
	public class StoC_0x6E_UnknownPacket: Packet, IKeepIdPacket
	{
		protected ushort unk1; // keep part oid ?
		protected ushort unk2;

		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
		{
			get { return new ushort[] { unk1 }; }
		}

		#region public access properties
		
		public ushort Unk1 { get { return unk1 ; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("keepId?:0x{0:X4} unk2:0x{1:X4}", unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x6E_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}