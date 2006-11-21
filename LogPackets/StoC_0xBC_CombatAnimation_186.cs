using System;
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

		public override string GetPacketDataString(bool flagsDescription)
		{
			return base.GetPacketDataString(flagsDescription) + string.Format(" unk1:0x{0:X2}", unk1);
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