using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 183, ePacketDirection.ServerToClient, "Quest update v183")]
	public class StoC_0x83_QuestUpdate_183 : StoC_0x83_QuestUpdate
	{
		public override void Init()
		{
			Position = 0;
			index = ReadByte();
			ushort temp = 0;
			if (index == 0)
			{
				lenName = ReadShortLowEndian();
				lenDesc = ReadByte();	
				temp = ReadByte();
			}
			else
			{
				lenName = ReadByte();
				lenDesc = ReadShortLowEndian();
				temp = ReadByte();
				lenName = (ushort) ((temp << 8) + lenName);
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
		public StoC_0x83_QuestUpdate_183(int capacity) : base(capacity)
		{
		}
	}
}