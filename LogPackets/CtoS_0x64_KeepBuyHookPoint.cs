using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x64, -1, ePacketDirection.ClientToServer, "Keep/Tower buy hook point")]
	public class CtoS_0x64_KeepBuyHookPoint: Packet, IObjectIdPacket
	{
		protected ushort keepId;
		protected ushort componentId;
		protected ushort hookPointId;
		protected ushort itemSlot;
		protected byte payType;
		protected byte unk1;
		protected byte unk2;
		protected byte unk3;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { keepId }; }
		}

		#region public access properties

		public ushort KeepId { get { return keepId; } }
		public ushort ComponentId { get { return componentId; } }
		public ushort HookPointId { get { return hookPointId; } }
		public ushort ItemSlot { get { return itemSlot; } }
		public byte PayType { get { return payType; } }
		public byte Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }
		public byte Unk3 { get { return unk3; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("keepId:0x{0:X4} componentId:{1,-2} hookPointId:0x{2:X2} itemSlot:0x{3:X4} payType?:0x{4:X2} unk1:0x{5:X4} unk2:0x{6:X4} unk3:0x{7:X4}",
				keepId, componentId, hookPointId, itemSlot, payType, unk1, unk2, unk3);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			keepId = ReadShort();
			componentId= ReadShort();
			hookPointId = ReadShort();
			itemSlot = ReadShort();
			payType = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x64_KeepBuyHookPoint(int capacity) : base(capacity)
		{
		}
	}
}