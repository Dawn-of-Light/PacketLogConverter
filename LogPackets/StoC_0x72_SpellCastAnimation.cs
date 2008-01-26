using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x72, -1, ePacketDirection.ServerToClient, "Spell cast animation  ")]
	public class StoC_0x72_SpellCastAnimation : Packet, IObjectIdPacket
	{
		protected ushort casterOid;
		protected ushort spellId;
		protected ushort castingTime;
		protected ushort unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { casterOid }; }
		}

		#region public access properties

		public ushort CasterOid { get { return casterOid; } }
		public ushort SpellId { get { return spellId; } }
		public ushort CastingTime { get { return castingTime; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("casterOid:0x{0:X4} spellId:0x{1:X4} castingTime:{2,-3} unk1:0x{3:X4}", casterOid, spellId, castingTime, unk1);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			casterOid = ReadShort();
			spellId = ReadShort();
			castingTime = ReadShort();
			unk1 = ReadShort();
		}

		public StoC_0x72_SpellCastAnimation(int capacity) : base(capacity)
		{
		}
	}
}