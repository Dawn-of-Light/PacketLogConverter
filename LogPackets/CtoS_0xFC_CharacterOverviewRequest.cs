using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFC, -1, ePacketDirection.ClientToServer, "Character overview request")]
	public class CtoS_0xFC_CharacterOverviewRequest : Packet
	{
		protected string clientAccountName;
		protected uint unk1;

		#region public access properties

		public string ClientAccountName { get { return clientAccountName; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			if (flagsDescription)
			{
				text.Write(string.Format("unk1:0x{0:X8} ", unk1));
			}
			text.Write("clientAccountName:\"");
			text.Write(clientAccountName);
			text.Write('"');
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientAccountName = ReadString(20);
			unk1 = ReadIntLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFC_CharacterOverviewRequest(int capacity) : base(capacity)
		{
		}
	}
}