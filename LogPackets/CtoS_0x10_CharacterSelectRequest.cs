using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, -1, ePacketDirection.ClientToServer, "Character select request")]
	public class CtoS_0x10_CharacterSelectRequest : Packet, ISessionIdPacket
	{
		protected ushort sessionId;
		protected byte regionIndex;
		protected byte unk1;
		protected string charName;
		protected uint unk0;
		protected string loginName;
		protected uint unk2;
		protected ushort port; // socket ?
		protected ushort unk3;
		protected uint u1;
		protected uint u2;
		protected uint u3;
		protected uint u4;
		protected uint u5;
		protected uint u6;
		protected uint u7;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public byte RegionIndex { get { return regionIndex; } }
		public byte Unk1 { get { return unk1; } }
		public string CharName { get { return charName; } }
		public string LoginName { get { return loginName; } }
		public uint Unk2 { get { return unk2; } }
		public ushort Port { get { return port; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			string flags = (flagsDescription ? string.Format("{0:X8} {1:X8} {2:X8} {3:X8} {4:X8} {5:X8} {6:X8} ", u1, u2, u3, u4, u5, u6, u7) : "");
			text.Write("{8}sessionId:0x{1:X4} regionIndex:0x{2:X2} unk1:0x{3:X2} socket:{4,-5} unk0:0x{9:X8} unk2:0x{6:X8} unk3:0x{7:X4} login:\"{5}\" charName:\"{0}\"",
				charName, sessionId, regionIndex, unk1, port, loginName, unk2, unk3, flags, unk0);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			regionIndex = ReadByte();
			unk1 = ReadByte();
			charName = ReadString(24);
			unk0 = ReadIntLowEndian();
			loginName = ReadString(20);
			u1 = ReadIntLowEndian();
			u2 = ReadIntLowEndian();
			u3 = ReadIntLowEndian();
			u4 = ReadIntLowEndian();
			u5 = ReadIntLowEndian();
			u6 = ReadIntLowEndian();
			u7 = ReadIntLowEndian();
			unk2 = ReadIntLowEndian();
			port = ReadShort();
			unk3 = ReadShort();

		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x10_CharacterSelectRequest(int capacity) : base(capacity)
		{
		}
	}
}