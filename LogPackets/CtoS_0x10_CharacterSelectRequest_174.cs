using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			string temp = flagsDescription ? " 0x" + unks0.ToString("X4") + unks1.ToString("X2") : "";
			text.Write("serverId:0x");
			text.Write(serverId.ToString("X2"));
			text.Write(temp);
			text.Write(" ");
//			((Packet) this).GetPacketDataString(text, flagsDescription); // this crash program
			base.GetPacketDataString(text, flagsDescription);
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
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			// Reinit only on for 190 version and subversion lower 190.1
			if (!log.IgnoreVersionChanges && log.Version >= 190 && log.Version < 190.1f)
			{
				if (Length == 100)
				{
					log.Version = 190.1f;
					log.SubversionReinit = true;
//					log.IgnoreVersionChanges = true;
				}
			}
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