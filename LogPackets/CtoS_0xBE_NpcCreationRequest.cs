using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBE, -1, ePacketDirection.ClientToServer, "Npc creation request")]
	public class CtoS_0xBE_NpcCreationRequest : Packet, IObjectIdPacket
	{
		protected ushort npcOid;
		protected ushort unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { npcOid }; }
		}

		#region public access properties

		public ushort NpcOid { get { return npcOid; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x");
			text.Write(npcOid.ToString("X4"));
			if (flagsDescription)
			{
				text.Write(" unk1:0x");
				text.Write(unk1.ToString("X4"));
			}
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