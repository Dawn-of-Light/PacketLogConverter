using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x10, 175, ePacketDirection.ClientToServer, "Character select request v174+")]
	public class CtoS_0x10_CharacterSelectRequest_175 : CtoS_0x10_CharacterSelectRequest
	{
		protected byte serverId;
		protected uint u1;
		protected uint u2;
		protected uint u3;
		protected uint u4;
		protected uint u5;
		protected uint u6;
		protected uint u7;

		#region public access properties

		public byte ServerId { get { return serverId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			string flags = (flagsDescription ? string.Format(" {0:X8} {1:X8}  {2:X8}  {3:X8}  {4:X8}  {5:X8}  {6:X8}", u1, u2, u3, u4, u5, u6, u7) : "");
			return "serverId:0x" + serverId.ToString("X2") + " " + base.GetPacketDataString(flagsDescription) + flags;
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
			loginName = ReadString(20); // skip unknown
			u1 = ReadIntLowEndian();
			u2 = ReadIntLowEndian();
			u3 = ReadIntLowEndian();
			u4 = ReadIntLowEndian();
			u5 = ReadIntLowEndian();
			u6 = ReadIntLowEndian();
			u7 = ReadIntLowEndian();
			unk2 = ReadInt();
			port = ReadShort();
			unk3 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x10_CharacterSelectRequest_175(int capacity) : base(capacity)
		{
		}
	}
}