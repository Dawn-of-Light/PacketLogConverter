using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, 184, ePacketDirection.ServerToClient, "Quest update v184")]
	public class StoC_0x83_QuestUpdate_184 : StoC_0x83_QuestUpdate
	{
		protected byte zone;
		public byte Zone { get { return zone; } }

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("index:{0,-2} NameLen:{1,-3} descLen:{2,-3} zone:{3}", index, lenName, lenDesc, zone);

			if (lenName == 0 && lenDesc == 0)
				return str.ToString();
			str.AppendFormat("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);

			return str.ToString();
		}

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
			zone = ReadByte();
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
		public StoC_0x83_QuestUpdate_184(int capacity) : base(capacity)
		{
		}
	}
}