using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD3, -1, ePacketDirection.ServerToClient, "Craft result ?")]
	public class StoC_0xD3_CraftResult : Packet
	{
		protected ushort unk1;
		protected ushort result;
		protected uint unk2;
		protected uint unk3;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort Result { get { return result; } }
		public uint Unk2 { get { return unk2; } }
		public uint Unk3 { get { return unk3; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X4} result:0x{1:X4} unk2:0x{2:X8} unk3:0x{3:X8}", unk1, result, unk2, unk3);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			result = ReadShort();
			unk2 = ReadInt();
			unk3 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xD3_CraftResult(int capacity) : base(capacity)
		{
		}
	}
}