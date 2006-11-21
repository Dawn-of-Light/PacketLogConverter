using System.Text;
namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x83, -1, ePacketDirection.ServerToClient, "Quest update")]
	public class StoC_0x83_QuestUpdate: Packet
	{
		public byte subCode;
		protected ASubData subData;

		public byte SubCode { get { return subCode; } }
		public ASubData SubData { get { return subData; } }

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			if (subData == null)
				str.AppendFormat(" UNKNOWN SUBCODE");
			else
				subData.MakeString(str, flagsDescription);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			subCode = 0;
			InitSubcode(subCode);
		}

		protected void InitSubcode(byte code)
		{
			switch (code)
			{
				case 0: InitQuestUpdate(); break;
				case 1: InitNewQuestUpdate(); break;
				default: subData = null; break;
			}
			return;
		}

		/// <summary>
		/// Base abstract class for all sub codes data
		/// </summary>
		public abstract class ASubData
		{
			abstract public void Init(StoC_0x83_QuestUpdate pak);
			abstract public void MakeString(StringBuilder str, bool flagsDescription);
		}

		protected virtual void InitQuestUpdate()
		{
			subData = new QuestUpdate();
			subData.Init(this);
		}

		public class QuestUpdate : ASubData
		{
			public byte index;
			public ushort lenName;
			public ushort lenDesc;
			public string name;
			public string desc;
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
				index = pak.ReadByte();
				lenName = pak.ReadByte();
				lenDesc = pak.ReadShortLowEndian();
				if (lenName == 0 && lenDesc == 0)
				{
					name = "";
					desc = "";
				}
				else
				{
					name = pak.ReadString(lenName);
					desc = pak.ReadString(lenDesc);
				}
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("index:{0,-2} NameLen:{1,-3} descLen:{2,-3}", index, lenName, lenDesc);

				if (lenName == 0 && lenDesc == 0)
					return;
				str.AppendFormat("\n\tname: \"{0}\"\n\tdesc: \"{1}\"", name, desc);
			}
		}

		protected virtual void InitNewQuestUpdate()
		{
			subData = new NewQuestUpdate();
			subData.Init(this);
		}

		public class NewQuestUpdate : ASubData
		{
			public override void Init(StoC_0x83_QuestUpdate pak)
			{
			}
			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
			}
		}

		public StoC_0x83_QuestUpdate(int capacity) : base(capacity)
		{
		}
	}
}