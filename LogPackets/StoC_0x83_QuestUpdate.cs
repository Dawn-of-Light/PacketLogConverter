using System.Text;
namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, -1, ePacketDirection.ServerToClient, "Quest update")]
	public class StoC_0x83_QuestUpdate: Packet
	{
		protected byte index;
		protected ushort lenName;
		protected ushort lenDesc;
		protected string name;
		protected string desc;

		public byte Index { get { return index; } }
		public ushort LenName { get { return lenName; } }
		public ushort LenDesc { get { return lenDesc; } }
		public string Name { get { return name; } }
		public string Desc { get { return desc; } }

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("index:{0,-2} NameLen:{1,-3} descLen:{2,-3}", index, lenName, lenDesc);

			if (lenName == 0 && lenDesc == 0)
				return str.ToString();
			str.AppendFormat("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);

			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			index = ReadByte();
			lenName = ReadByte();
			lenDesc = ReadShortLowEndian();
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

		public StoC_0x83_QuestUpdate(int capacity) : base(capacity)
		{
		}
	}
}