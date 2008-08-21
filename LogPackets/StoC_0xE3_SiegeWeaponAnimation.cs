using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE3, -1, ePacketDirection.ServerToClient, "Siege Weapon Animation")]
	public class StoC_0xE3_SiegeWeaponAnimation : Packet, IObjectIdPacket
	{
		protected uint oid;
		protected uint x;
		protected uint y;
		protected uint z;
		protected uint targetOid; // target ?
		protected ushort effect;
		protected ushort timer; // in sec/10
		protected byte action;
		protected byte unk4; // unused
		protected ushort unk5; // unused

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { (ushort)oid, (ushort)targetOid }; }
		}

		#region public access properties

		public ushort Oid { get { return (ushort)oid; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public uint Z { get { return z; } }
		public ushort TargetOid { get { return (ushort)targetOid; } }
		public ushort Effect { get { return effect; } }
		public ushort Timer { get { return timer; } }
		public byte Action { get { return action; } }

		#endregion

		public enum eActionAnimation: byte
		{
			Aiming = 1,
			Arming = 2,
			Loading = 3,
			Fire = 4,
		}

		public enum eSiegeWeaponEffect: ushort
		{
			None = 0,
			old_Ballista_I = 2200,
			old_Catapult_I = 2201,
			Scorpion_Bolt = 2202,
			Ballista_Bolt = 2203,
			Trebuchet_Catapult_Boulder = 2204,
			Catapult_Flaming_Pitchball = 2205,
			Catapult_Coldball = 2206,
			Catapult_Stone_Shot = 2207,
			Catapult_Disease_Carcass = 2208, 
			Boiling_Oil = 2209,
			Catapult_Poison_Ball = 2210,
			Catapult_Essence_Ball = 2211,
			Stone_Throw = 2212,
			Flying_Stone = 2213,
			Flying_Cold_Ball = 2214,
			Saw_Blade_Projectile = 2215,
			// where 2216 in spells.csv ?
			Carcass_90 = 2217,
			Cold_Ball_90 = 2218,
			Pitch_Ball_90 = 2219,
			Stone_Shot_90 = 2220,
			Scorpion_90 = 2221
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} targetOid:0x{1:X4}(x:{2,-6} y:{3,-6} z:{4,-5}) spellId:0x{5:X4} timer:{6,-3} action:{7}",
				oid, targetOid, x, y, z, effect, timer, action);
			if (flagsDescription)
			{
				text.Write("({0})", (eActionAnimation)action);
				if (effect != 0)
					text.Write(" spellId:{0}", (eSiegeWeaponEffect)effect);
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadInt();          // 0x00
			x = ReadInt();            // 0x04
			y = ReadInt();            // 0x08
			z = ReadInt();            // 0x0C
			targetOid = ReadInt();    // 0x10
			effect = ReadShort();     // 0x14
			timer = ReadShort();      // 0x16
			action = ReadByte();      // 0x18
			unk4 = ReadByte();        // 0x19 unused
			unk5 = ReadShort();       // 0x1A unused
			                          // 0x1C+ ? 3 * uint ?
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xE3_SiegeWeaponAnimation(int capacity) : base(capacity)
		{
		}
	}
}