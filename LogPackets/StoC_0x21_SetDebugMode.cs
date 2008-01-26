using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x21, -1, ePacketDirection.ServerToClient, "Set debug mode")]
	public class StoC_0x21_SetDebugMode : Packet
	{
		protected byte state;
		protected byte unk1;

		#region public access properties

		public byte State { get { return state; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("state:{0} unk1:{1}", state, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			state = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x21_SetDebugMode(int capacity) : base(capacity)
		{
		}
	}
}