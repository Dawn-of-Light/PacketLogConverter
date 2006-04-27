using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE3, -1, ePacketDirection.ServerToClient, "Siege Weapon Animation")]
	public class StoC_0xE3_SiegeWeaponAnimation : Packet, IOidPacket
	{
		protected ushort unk1;
		protected ushort oid;
		protected uint x;
		protected uint y;
		protected ushort unk2;
		protected ushort z;
		protected ushort unk3; // some oid ?
		protected ushort targetOid; // target ?
		protected ushort effect;
		protected ushort timer; // in sec/10
		protected byte action;
		protected byte unk4;
		protected ushort unk5;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return targetOid; } }

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public ushort Oid { get { return oid; } }
		public uint X { get { return x; } }
		public uint Y { get { return y; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Z { get { return z; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort TargetOid { get { return targetOid; } }
		public ushort Effect { get { return effect; } }
		public ushort Timer { get { return timer; } }
		public byte Action { get { return action; } }
		public byte Unk4 { get { return unk4; } }
		public ushort Unk5 { get { return unk5; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("unk1:0x{0:X4} oid:0x{1:X4} targetOid:0x{2:X4}(x:{3,-6} y:{4,-6} unk2:0x{5:X4} z:{6,-5}) unk3:{7:X4} spellId:{8,-5} timer:{9,-3} action:{10} unk4:0x{11:X2} unk5:0x{12:X4}",
			                 unk1, oid, targetOid, x, y, unk2, z, unk3, effect, timer, action, unk4, unk5);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadShort();
			oid = ReadShort();
			x = ReadInt();
			y = ReadInt();
			unk2 = ReadShort();
			z = ReadShort();
			unk3 = ReadShort();
			targetOid = ReadShort();
			effect = ReadShort();
			timer = ReadShort();
			action = ReadByte();
			unk4 = ReadByte();
			unk5 = ReadShort();
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