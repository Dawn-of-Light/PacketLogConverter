using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x59, -1, ePacketDirection.ServerToClient, "Mino relic realm")]
	public class StoC_0x59_UnknownPacket: Packet
	{
		protected uint id;
		protected uint type;

		#region public access properties

		public uint ID { get { return id; } }
		public uint Type { get { return type; } }

		#endregion

		public enum eRelicType: byte
		{
			None = 0,
			Yellow = 1,
			Green = 2,
			Red = 3,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("id:{0} type:{1}", id, type);
			if (flagsDescription)
				text.Write("({0})", (eRelicType)type);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			id = ReadIntLowEndian();
			type = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x59_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}