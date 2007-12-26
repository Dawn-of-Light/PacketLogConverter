using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x14, -1, ePacketDirection.ClientToServer, "UDP Init Request")]
	public class CtoS_0x14_UdpInitRequest : Packet
	{
		protected string clientIp;
		protected uint signature;
		protected uint port;
		protected string checkIP;
		protected bool flagSignature;

		#region public access properties

		public string ClientIP { get { return clientIp; } }
		public uint Port { get { return port; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("client IP:\"{0}\":{1}", clientIp, port);
			if (flagsDescription)
			{
				if (flagSignature)
				{
					str.AppendFormat(" sig:0x{0:X8}", signature);
					if (Enum.IsDefined(typeof(signatures),(signatures)(signature & 0xFFFF)))
						str.AppendFormat(" ({0})", (signatures)(signature & 0xFFFF));
				}
				else
				{
					str.AppendFormat(" partIP:\"{0}\"", checkIP);
				}
			}
			return str.ToString();
		}

		public enum signatures: ushort
		{
			valid1 = 0,
			valid2 = 10000,
			Autokiller = 3,
			logger351 = 0x2406,
			logger35 = 0x2407,
			logger30 = 0x21F7,
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			long savePos;
			Position = 0;
			clientIp = ReadString(16);
			signature = ReadIntLowEndian();
			port = ReadInt();
			savePos = Position;
			Position = (1 + clientIp.Length > 12) ? 1 + clientIp.Length : 12;
			checkIP = ReadString(8);
			flagSignature = checkIP.Length == 0 | (clientIp.IndexOf(checkIP) == -1);
			Position = savePos;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x14_UdpInitRequest(int capacity) : base(capacity)
		{
		}
	}
}