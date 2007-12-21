using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA2, -1, ePacketDirection.ServerToClient, "Remove object")]
	public class StoC_0xA2_RemoveObject : Packet, IObjectIdPacket
	{
		private ushort oid;
		private ushort objectType;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort ObjectType { get { return objectType; } }

		#endregion

		public enum eObjectType: byte
		{
			Object = 0,
			NPC = 1,
			Player = 2,
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} objectType:{1}", oid, objectType);
			if (flagsDescription)
				str.AppendFormat("({0})", (eObjectType)objectType);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			oid = ReadShort();
			objectType = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xA2_RemoveObject(int capacity) : base(capacity)
		{
		}
	}
}