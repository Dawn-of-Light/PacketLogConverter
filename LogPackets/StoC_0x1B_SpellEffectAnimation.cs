using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x1B, -1, ePacketDirection.ServerToClient, "Spell effect animation")]
	public class StoC_0x1B_SpellEffectAnimation : Packet, IObjectIdPacket
	{
		protected ushort casterOid;
		protected ushort spellId;
		protected ushort targetOid;
		protected ushort boltTime;
		protected byte noSound;
		protected byte success;
		protected ushort unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { casterOid, targetOid }; }
		}

		#region public access properties

		public ushort CasterOid { get { return casterOid; } }
		public ushort SpellId { get { return spellId; } }
		public ushort TargetOid { get { return targetOid; } }
		public ushort BoltTime { get { return boltTime; } }
		public byte NoSound { get { return noSound; } }
		public byte Success { get { return success; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("casterOid:0x{0:X4} spellId:{1,-5} targetOid:0x{2:X4} boltTime:{3,-3} noSound:{4} success:0x{5:X2} unk1:0x{6:X4}",
				casterOid, spellId, targetOid, boltTime, noSound, success, unk1);
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
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x1B_SpellEffectAnimation(int capacity) : base(capacity)
		{
		}
	}
}