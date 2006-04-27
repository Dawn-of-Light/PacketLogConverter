using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x72, -1, ePacketDirection.ServerToClient, "Spell cast animation  ")]
	public class StoC_0x72_SpellCastAnimation : Packet, IOidPacket
	{
		protected ushort casterOid;
		protected ushort spellId;
		protected ushort castingTime;
		protected ushort unk1;

		public int Oid1 { get { return casterOid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort CasterOid { get { return casterOid; } }
		public ushort SpellId { get { return spellId; } }
		public ushort CastingTime { get { return castingTime; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("casterOid:0x{0:X4} spellId:{1,-5} castingTime:{2,-3} unk1:0x{3:X4}", casterOid, spellId, castingTime, unk1);
			return str.ToString();
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