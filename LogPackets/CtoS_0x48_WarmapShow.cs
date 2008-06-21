using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x48, -1, ePacketDirection.ClientToServer, "Show warmap request")]
	public class CtoS_0x48_WarmapShowRequest: Packet
	{
		protected byte requestType;
		protected byte realm;
		protected byte keepNumber;
		protected byte unk1;

		#region public access properties

		public byte RequestType { get { return requestType; } }
		public byte Realm { get { return realm; } }
		public byte KeepNumber { get { return keepNumber; } }
		public byte Unk1 { get { return unk1 ; } }

		#endregion

		public enum eWindowFlag: byte
		{
			SetRealmPage = 0,
			Update = 1,
			Teleport = 2,
//			WindowMove = 0x64,
//			WindowClose = 0x65,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("realm:{0} keepNumber:0x{1:X2} unk1:0x{2:X2} windowFlag:0x{3:X2}", realm, keepNumber, unk1, requestType);
			if (flagsDescription)
				text.Write("({0})", (eWindowFlag)requestType);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			requestType = ReadByte();
			realm = ReadByte();
			keepNumber = ReadByte();
			unk1 = ReadByte();
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