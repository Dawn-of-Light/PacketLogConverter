using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBC, 186, ePacketDirection.ServerToClient, "Combat animation 186")]
	public class StoC_0xBC_CombatAnimation_186 : StoC_0xBC_CombatAnimation
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("attackerOid:0x{0:X4} defenderOid:0x{1:X4} attackerWeaponModel:0x{2:X4} defenderWeaponModel:0x{3:X4} styleId:0x{4:X4} stance:0x{5:X2} result:0x{6:X2} targetHealth:{7,3}% unk1:0x{8:X2}",
				attackerOid, defenderOid, weaponId, defenseWeapon, styleId, stance, result, targetHealthPercent, unk1);
			if(flagsDescription)
				text.Write(" ({0})", (eResult)(result & 0x7F));

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			attackerOid = ReadShort();
			defenderOid = ReadShort();
			weaponId = ReadShort();
			defenseWeapon = ReadShort();
			styleId = ReadShortLowEndian();
			stance = ReadByte();
			result = ReadByte(); // 0x80 bit is style > 255
			targetHealthPercent = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xBC_CombatAnimation_186(int capacity) : base(capacity)
		{
		}
	}
}