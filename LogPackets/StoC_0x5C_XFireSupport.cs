using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x5C, 188, ePacketDirection.ServerToClient, "XFire support")]
	public class StoC_0x5C_XFireSupport: Packet, IObjectIdPacket
	{
		protected ushort objectId;
		protected byte flag;
		protected byte unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { objectId }; }
		}

		#region public access properties

		public ushort ObjectId { get { return objectId; } }
		public byte Flag { get { return flag; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} flag:{1} unk1:0x{2:X2}", objectId, flag, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectId = ReadShort();
			flag = ReadByte();
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x5C_XFireSupport(int capacity) : base(capacity)
		{
		}
	}
}