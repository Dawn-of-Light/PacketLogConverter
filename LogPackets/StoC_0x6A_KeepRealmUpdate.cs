using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x6A, -1, ePacketDirection.ServerToClient, "Keep/Tower realm update")]
	public class StoC_0x6A_KeepRealmUpdate : Packet, IKeepIdPacket
	{
		protected ushort keepId;
		protected byte realm;
		protected byte level;

		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		public ushort[] KeepIds
		{
			get { return new ushort[] { keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public byte Realm  { get { return realm; } }
		public byte Level { get { return level; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("keepId:0x{0:X4} realm:{1} level:{2}", keepId, realm, level);
		}

		public override void Init()
		{
			Position = 0;
			keepId = ReadShort();         // 0x00
			realm = ReadByte();           // 0x02
			level = ReadByte();           // 0x03
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x6A_KeepRealmUpdate(int capacity) : base(capacity)
		{
		}
	}
}