using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1B, 174, ePacketDirection.ServerToClient, "Spell effect animation v174")]
	public class StoC_0x1B_SpellEffectAnimation_174 : StoC_0x1B_SpellEffectAnimation
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("casterOid:0x{0:X4} spellId:0x{1:X4} targetOid:0x{2:X4} boltTime:{3,-3} noSound:{4} success:0x{5:X2}",
				casterOid, spellId, targetOid, boltTime, noSound, success);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			casterOid = ReadShort(); // 0x00
			spellId = ReadShort();   // 0x02
			targetOid = ReadShort(); // 0x04
			boltTime = ReadShort();  // 0x06
			noSound = ReadByte();    // 0x08
			success = ReadByte();    // 0x09
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