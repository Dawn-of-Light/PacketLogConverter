using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 170, ePacketDirection.ServerToClient, "Quest update v170")]
	public class StoC_0x83_QuestUpdate_170 : StoC_0x83_QuestUpdate
	{
		public override void Init()
		{
			Position = 0;
			index = ReadByte();
			lenName = ReadByte();
			lenDesc = ReadShort();
			if (lenName == 0 && lenDesc == 0)
			{
				name = "";
				desc = "";
			}
			else
			{
				name = ReadString(lenName);
				desc = ReadString(lenDesc);
			}
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x83_QuestUpdate_170(int capacity) : base(capacity)
		{
		}
	}
}