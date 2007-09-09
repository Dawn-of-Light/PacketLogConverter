using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x48, -1, ePacketDirection.ClientToServer, "Show warmap request")]
	public class CtoS_0x48_WarmapShowRequest: Packet
	{
		protected byte unk1;
		protected byte realm;
		protected ushort unk2;

		#region public access properties

		public byte Unk1 { get { return unk1 ; } }
		public byte Realm { get { return realm; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public enum eWindowFlag: byte
		{
			SetRealmPage = 0,
			Update = 1,
			Teleport = 2,
//			WindowMove = 0x64,
//			WindowClose = 0x65,
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("realm:{1} unk2:0x{2:X4} windowFlag:0x{0:X2}({3})", unk1, realm, unk2, (eWindowFlag)unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadByte();
			realm = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x48_WarmapShowRequest(int capacity) : base(capacity)
		{
		}
	}
}