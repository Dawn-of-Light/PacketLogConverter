using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xBC, -1, ePacketDirection.ServerToClient, "Combat animation")]
	public class StoC_0xBC_CombatAnimation : Packet, IObjectIdPacket
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

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { attackerOid, defenderOid }; }
		}

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

		public enum eResult: byte
		{
			miss = 0,
			parry = 1,
			block = 2,
			evade = 3,
			fumble = 4,
			hitUnstyled = 0x0A,
			hitStyled = 0x0B,
			hitMagic = 0x14
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("attackerOid:0x{0:X4} defenderOid:0x{1:X4} attackerWeaponModel:0x{2:X4} defenderWeaponModel:0x{3:X4} styleId:0x{4:X4} stance:0x{5:X2} result:0x{6:X2} targetHealth:{7,3}%",
				attackerOid, defenderOid, weaponId, defenseWeapon, styleId, stance, result, targetHealthPercent);
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
			styleLow = ReadByte();
			stance = ReadByte();
			result = ReadByte(); // 0x80 bit is style > 255
			targetHealthPercent = ReadByte();
			styleId = styleLow;

			if ((result & 0x80) != 0)
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