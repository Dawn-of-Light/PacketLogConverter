using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB7, 174, ePacketDirection.ServerToClient, "Region change v174")]
	public class StoC_0xB7_RegionChange_174: StoC_0xB7_RegionChange_173
	{
		protected byte serverId;
		protected byte unk3_174;
		protected ushort unk4_174;

		#region public access properties

		public byte ServerId { get { return serverId; } }
		public byte Unk3_174 { get { return unk3_174; } }
		public ushort Unk4_174 { get { return unk4_174; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("{0} serverId:0x{1:X2} unk3_174:0x{2:X2} unk3_174:0x{3:X4}", base.GetPacketDataString(), serverId, unk3_174, unk4_174);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			base.Init();
			serverId = ReadByte();
			unk3_174 = ReadByte();
			unk4_174 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xB7_RegionChange_174(int capacity) : base(capacity)
		{
		}
	}
}