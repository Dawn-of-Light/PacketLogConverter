using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC8, -1, ePacketDirection.ServerToClient, "Player ride")]
	public class StoC_0xC8_PlayerRide: Packet, IObjectIdPacket
	{
		protected ushort riderOid;
		protected ushort mountOid;
		protected byte flag;
		protected byte slot;
		protected ushort unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { riderOid, mountOid }; }
		}

		#region public access properties

		public ushort RiderOid { get { return riderOid; } }
		public ushort MountOid { get { return mountOid; } }
		public byte Flag { get { return flag; } }
		public byte Slot { get { return slot; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("riderOid:0x{0:X4} mountOid:0x{1:X4} flag:{2} slot:0x{3:X2} unk1:0x{4:X4}",
				riderOid, mountOid, flag, slot, unk1);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			riderOid= ReadShort();
			mountOid= ReadShort();
			flag = ReadByte();
			slot = ReadByte();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC8_PlayerRide(int capacity) : base(capacity)
		{
		}
	}
}