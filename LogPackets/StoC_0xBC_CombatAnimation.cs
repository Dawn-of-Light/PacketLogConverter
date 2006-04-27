using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBC, -1, ePacketDirection.ServerToClient, "Combat animation")]
	public class StoC_0xBC_CombatAnimation : Packet, IOidPacket
	{
		protected ushort attackerOid;
		protected ushort defenderOid;
		protected ushort weaponId; // model from objects.csv
		protected ushort defenseWeapon;
		protected byte styleLow;
		protected byte stance;
		protected byte result;
		protected byte targetHealthPercent;
		protected ushort styleId;

		public int Oid1 { get { return attackerOid; } }
		public int Oid2 { get { return defenderOid; } }

		#region public access properties

		public ushort AttackerOid { get { return attackerOid; } }
		public ushort DefenderOid { get { return defenderOid; } }
		public ushort WeaponId { get { return weaponId; } }
		public ushort DefenseWeapon { get { return defenseWeapon; } }
		public byte StyleLow { get { return styleLow; } }
		public byte Stance { get { return stance; } }
		public byte Result { get { return result; } }
		public byte TargetHealthPercent { get { return targetHealthPercent; } }
		public ushort StyleId { get { return styleId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("attackerOid:0x{0:X4} defenderOid:0x{1:X4} attackerWeaponModel:0x{2:X4} defenderWeaponModel:0x{3:X4} styleId:0x{4:X5} stance:0x{5:X2} result:0x{6:X2} targetHealth:{7,3}%",
				attackerOid, defenderOid, weaponId, defenseWeapon, styleId, stance, result, targetHealthPercent);

			return str.ToString();
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
			styleLow = ReadByte();
			stance = ReadByte();
			result = ReadByte(); // 0x80 bit is style > 255
			targetHealthPercent = ReadByte();
			styleId = styleLow;

			if ((result&0x80) != 0)
				styleId = (ushort)(styleLow | 0x100);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xBC_CombatAnimation(int capacity) : base(capacity)
		{
		}
	}
}