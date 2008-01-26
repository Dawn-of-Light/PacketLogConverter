using System.IO;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x8D, -1, ePacketDirection.ServerToClient, "Player model change (wireframe)")]
	public class StoC_0x8D_PlayerWireframe: Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected byte flag;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public byte Flag { get { return flag; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public enum eFlagType: byte
		{
			None = 0,
			Unstealthed = 2,
			Stealth = 3
		}
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} flag:0x{1:X2}{3} unk1:0x{2:X2}", oid, flag, unk1,
				flagsDescription ? "(" + (eFlagType)flag + ")" : "");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadShort();
			flag = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x8D_PlayerWireframe(int capacity) : base(capacity)
		{
		}
	}
}