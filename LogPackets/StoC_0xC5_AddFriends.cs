using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC5, -1, ePacketDirection.ServerToClient, "Add friends")]
	public class StoC_0xC5_AddFriends : Packet
	{
		private string[] friendNames;

		#region public access properties

		public string[] FriendNames { get { return friendNames; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			if (friendNames.Length > 0)
			{
				for (int i = 0; i < friendNames.Length; i++)
				{
					if (i > 0)
						str.Append(',');
					string friendName = (string)friendNames[i];
					str.Append('\"'+friendName+'\"');
				}
			}
			else
			{
				str.Append("none");
			}

			return str.ToString();
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
		public StoC_0xC5_AddFriends(int capacity) : base(capacity)
		{
		}
	}
}