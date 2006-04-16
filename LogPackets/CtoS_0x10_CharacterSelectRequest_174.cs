using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, 174, ePacketDirection.ClientToServer, "Character select request v174")]
	public class CtoS_0x10_CharacterSelectRequest_174 : CtoS_0x10_CharacterSelectRequest
	{
		protected byte serverId;

		#region public access properties

		public byte ServerId { get { return serverId; } }

		#endregion

		public override string GetPacketDataString()
		{
			return "serverId:0x" + serverId.ToString("X2") + " " + base.GetPacketDataString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			unk1 = ReadShort();
			serverId = ReadByte();
			charName = ReadString(31);
			loginName = ReadString(48); // skip unknown
			unk2 = ReadInt();
			port = ReadShort();
			unk3 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x10_CharacterSelectRequest_174(int capacity) : base(capacity)
		{
		}
	}
}