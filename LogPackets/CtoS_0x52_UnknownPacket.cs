using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x52, 199, ePacketDirection.ClientToServer, "Unknown packet")]
	public class CtoS_0x52_UnknownPacket: Packet
	{
		protected ushort unk1;
		protected string charName;
		protected uint unk2;
		protected ushort unk3;

		#region public access properties

		public ushort Unk1 { get { return unk1 ; } }
		public string CharName { get { return charName; } }
		public uint Unk2 { get { return unk2 ; } }
		public ushort Unk3 { get { return unk3 ; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:0x{0:X4}", unk1);
			if (flagsDescription)
			{
				text.Write(" unk2:0x{0:X8} unk3:0x{1:X4}", unk2, unk3);
			}
			text.Write(" name:\"{0}\"", charName);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			unk1 = ReadShort();
			charName = ReadString(18);
			unk2 = ReadInt();
			unk3 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x52_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}