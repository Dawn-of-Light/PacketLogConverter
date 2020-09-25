using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xAF, 1127, ePacketDirection.ServerToClient, "msg 1127")]
	public class StoC_0xAF_Message_1127 : StoC_0xAF_Message
	{
		
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("({0})", (eChatType)type);
						
			string s = Text;
			if (s.StartsWith("@@"))
			{
				text.Write(", chat");
				s = s.Substring(2);
			}
			else if (s.StartsWith("##"))
			{
				text.Write(",popup");
				s = s.Substring(2);
			}

			text.Write(": \"{0}\"", s);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
						
			type = ReadByte();			
			text = ReadString(2048);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xAF_Message_1127(int capacity) : base(capacity)
		{
		}
	}
}