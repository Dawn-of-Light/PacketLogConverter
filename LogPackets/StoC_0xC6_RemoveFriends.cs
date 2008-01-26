using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC6, -1, ePacketDirection.ServerToClient, "Remove friends")]
	public class StoC_0xC6_RemoveFriends : Packet
	{
		private string[] friendNames;

		#region public access properties

		public string[] FriendNames { get { return friendNames; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			if (friendNames.Length > 0)
			{
				for (int i = 0; i < friendNames.Length; i++)
				{
					if (i > 0)
						text.Write(',');
					string friendName = (string)friendNames[i];
					text.Write('\"'+friendName+'\"');
				}
			}
			else
			{
				text.Write("none");
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			ArrayList temp = new ArrayList();
			Position = 0;

			string friendName = ReadPascalString();
			while(friendName.Length > 0)
			{
				temp.Add(friendName);
				friendName = ReadPascalString();
			}
			friendNames = (string[])temp.ToArray(typeof (string));
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC6_RemoveFriends(int capacity) : base(capacity)
		{
		}
	}
}