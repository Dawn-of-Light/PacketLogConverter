using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, 174, ePacketDirection.ClientToServer, "Character select request v174")]
	public class CtoS_0x10_CharacterSelectRequest_174 : CtoS_0x10_CharacterSelectRequest
	{
		protected byte serverId;
		protected ushort unks0;
		protected byte unks1;

		#region public access properties

		public byte ServerId { get { return serverId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			string temp = flagsDescription ? " 0x" + unks0.ToString("X4") + unks1.ToString("X2") : "";
			return "serverId:0x" + serverId.ToString("X2") + temp + " " + base.GetPacketDataString(flagsDescription);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			regionIndex = ReadByte();
			unk1 = ReadByte();
			serverId = ReadByte();
			charName = ReadString(24);
			unks0 = ReadShort();
			unks1 = ReadByte();
			unk0 = ReadIntLowEndian();
			loginName = ReadString(20);
			u1 = ReadIntLowEndian();
			u2 = ReadIntLowEndian();
			u3 = ReadIntLowEndian();
			u4 = ReadIntLowEndian();
			u5 = ReadIntLowEndian();
			u6 = ReadIntLowEndian();
			u7 = ReadIntLowEndian();
			unk2 = ReadIntLowEndian();
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