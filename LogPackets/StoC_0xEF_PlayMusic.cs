using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEF, -1, ePacketDirection.ServerToClient, "Play Music")]
	public class StoC_0xEF_PlayMusic : Packet, IObjectIdPacket
	{
		protected ushort playerOid;
		protected byte type;
		protected byte soundId;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { playerOid }; }
		}

		#region public access properties

		public ushort PlayerOid { get { return playerOid; } }
		public byte Type { get { return type; } }
		public byte SoundId { get { return soundId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("playerOid:0x{0:X4} type:{1} soundId:{2}",
				playerOid, type, soundId);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerOid = ReadShort();
			type = ReadByte();
			soundId = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xEF_PlayMusic(int capacity) : base(capacity)
		{
		}
	}
}