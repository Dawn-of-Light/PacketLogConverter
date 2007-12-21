using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x5F, 190, ePacketDirection.ServerToClient, "Set roleplay status v190")]
	public class StoC_0x5F_UnknownPacket: Packet, IObjectIdPacket
	{
		protected byte rp;
		protected ushort objectId;
		protected byte unk1;
		protected uint unk2;

		#region public access properties

		public byte RP { get { return rp; } }
		public ushort SessionId { get { return objectId; } }
		public byte Unk1 { get { return unk1; } }
		public uint Unk2 { get { return unk2; } }

		#endregion
		public ushort[] ObjectIds
		{
			get
			{
				return new ushort[] { objectId };
			}
		}


		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("objectId:0x{0:X4} RP:{1} unk1:{2} unk2:0x{3:X8}", objectId, rp, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			rp = ReadByte();
			objectId = ReadShortLowEndian();
			unk1 = ReadByte();
			unk2 = ReadInt();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x5F_UnknownPacket(int capacity) : base(capacity)
		{
		}
	}
}