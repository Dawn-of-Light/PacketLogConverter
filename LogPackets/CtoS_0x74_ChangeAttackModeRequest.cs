using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x74, -1, ePacketDirection.ClientToServer, "Change attack mode request")]
	public class CtoS_0x74_ChangeAttackModeRequest : Packet
	{
		protected byte mode;
		protected byte userRequest;
		protected ushort unk1;

		#region public access properties

		public byte Mode { get { return mode; } }
		public byte UserRequest { get { return userRequest; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("mode:{0} userRequest:{1} unk1:0x{2:X4}", mode, userRequest, unk1);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			mode = ReadByte();
			userRequest = ReadByte();
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x74_ChangeAttackModeRequest(int capacity) : base(capacity)
		{
		}
	}
}