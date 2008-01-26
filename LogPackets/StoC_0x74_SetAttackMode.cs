using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x74, -1, ePacketDirection.ServerToClient, "Set attack mode")]
	public class StoC_0x74_SetAttackMode : Packet
	{
		protected byte state;
		protected byte unk1;
		protected ushort unk2;

		#region public access properties

		public byte State { get { return state; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("state:{0} unk1:0x{1:X2} unk2:0x{2:X4}", state, unk1, unk2);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			state = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x74_SetAttackMode(int capacity) : base(capacity)
		{
		}
	}
}