using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC3, -1, ePacketDirection.ServerToClient, "Bad name check response")]
	public class StoC_0xC3_BadNameCheckResponse: Packet
	{
		protected string charName;
		protected string loginName;
		protected byte code;

		#region public access properties

		public string CharName { get { return charName; } }
		public string LoginName { get { return loginName; } }
		public byte CheckCode { get { return code; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("charName:\"{0}\" login:\"{1}\" code:{2}({3})", charName, loginName, code, (code == 0 ? "bad" : "good"));

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			charName = ReadString(30);
			loginName = ReadString(20);
			code = ReadByte();
			Skip(3);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC3_BadNameCheckResponse(int capacity) : base(capacity)
		{
		}
	}
}