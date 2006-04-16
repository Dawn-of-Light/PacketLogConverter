using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 173, ePacketDirection.ServerToClient, "Quest update v173")]
	public class StoC_0x83_QuestUpdate_173 : StoC_0x83_QuestUpdate
	{
		public override void Init()
		{
			Position = 0;
			index = ReadByte();
			if (index == 0)
			{
				lenName = ReadShortLowEndian();
				lenDesc = ReadByte();	
			}
			else
			{
				lenName = ReadByte();
				lenDesc = ReadShortLowEndian();
			}
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
		public StoC_0x83_QuestUpdate_173(int capacity) : base(capacity)
		{
		}
	}
}