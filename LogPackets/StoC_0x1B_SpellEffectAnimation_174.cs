using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1B, 174, ePacketDirection.ServerToClient, "Spell effect animation v174")]
	public class StoC_0x1B_SpellEffectAnimation_174 : StoC_0x1B_SpellEffectAnimation
	{
		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("casterOid:0x{0:X4} spellId:{1,-5} targetOid:0x{2:X4} boltTime:{3,-3} noSound:{4} success:0x{5:X2}",
				casterOid, spellId, targetOid, boltTime, noSound, success);
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
			targetOid = ReadShort();
			boltTime = ReadShort();
			noSound = ReadByte();
			success = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x1B_SpellEffectAnimation_174(int capacity) : base(capacity)
		{
		}
	}
}