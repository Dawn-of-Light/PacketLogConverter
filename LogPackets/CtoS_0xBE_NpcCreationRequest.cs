using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBE, -1, ePacketDirection.ClientToServer, "Npc creation request")]
	public class CtoS_0xBE_NpcCreationRequest : Packet, IOidPacket
	{
		protected ushort npcOid;
		protected ushort unk1;

		public int Oid1 { get { return npcOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort NpcOid { get { return npcOid; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			return "oid:0x" + npcOid.ToString("X4");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			npcOid = ReadShort();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xBE_NpcCreationRequest(int capacity) : base(capacity)
		{
		}
	}
}