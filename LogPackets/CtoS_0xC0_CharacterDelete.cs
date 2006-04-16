using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC0, -1, ePacketDirection.ClientToServer, "Character delete")]
	public class CtoS_0xC0_CharacterDelete: Packet
	{
		protected string charName;
		protected string loginName;

		#region public access properties

		public string CharName { get { return charName; } }
		public string LoginName { get { return loginName; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("charName:\"{0}\" login:\"{1}\"", charName, loginName);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			charName = ReadString(30);
			loginName = ReadString(24);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xC0_CharacterDelete(int capacity) : base(capacity)
		{
		}
	}
}