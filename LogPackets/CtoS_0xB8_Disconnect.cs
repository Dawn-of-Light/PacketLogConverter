using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB8, -1, ePacketDirection.ClientToServer, "Disconnect")]
	public class CtoS_0xB8_Disconnect : Packet
	{
		protected byte flag;

		#region public access properties

		public byte Flag { get { return flag; } }

		#endregion

		public enum eFlagType: byte
		{
			QTD = 0,
			NormalQuit= 1,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("Code:{0}{1}", flag, flagsDescription ? "(" + (eFlagType)flag + ")" : "");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flag = ReadByte();

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xB8_Disconnect(int capacity) : base(capacity)
		{
		}
	}
}