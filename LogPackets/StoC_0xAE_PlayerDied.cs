using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAE, -1, ePacketDirection.ServerToClient, "Player died")]
	public class StoC_0xAE_PlayerDied : Packet, IObjectIdPacket
	{
		protected ushort killedOid;
		protected ushort killerOid;
		protected ushort unk1;
		protected ushort unk2;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { killedOid, killedOid }; }
		}

		#region public access properties

		public ushort KilledOid { get { return killedOid; } }
		public ushort KillerOid { get { return killerOid; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("killedOid:0x{0:X4} killerOid:0x{1:X4} unk1:0x{2:X4} unk2:0x{3:X4}", killedOid, killerOid, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			killedOid = ReadShort();
			killerOid = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xAE_PlayerDied(int capacity) : base(capacity)
		{
		}
	}
}