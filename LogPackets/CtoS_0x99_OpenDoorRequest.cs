using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x99, -1, ePacketDirection.ClientToServer, "Open door request  ")]
	public class CtoS_0x99_OpenDoorRequest : Packet
	{
		protected uint doorId;
		protected byte openState;
		protected byte unk1;
		protected ushort unk2;

		#region public access properties

		public uint DoorId { get { return doorId; } }
		public byte OpenState { get { return openState; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("doorId:0x{0:X8} openState:{1} unk1:0x{2:X2} unk2:0x{3:X4}", doorId, openState, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			doorId = ReadInt();
			openState = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x99_OpenDoorRequest(int capacity) : base(capacity)
		{
		}
	}
}