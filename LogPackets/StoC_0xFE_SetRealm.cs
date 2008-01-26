using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFE, -1, ePacketDirection.ServerToClient, "Set realm")]
	public class StoC_0xFE_SetRealm : Packet
	{
		protected byte realm;

		#region public access properties

		public byte Realm { get { return realm; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("realm:" + realm);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			realm = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xFE_SetRealm(int capacity) : base(capacity)
		{
		}
	}
}