using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x40, -1, ePacketDirection.ClientToServer, "Finish newQuest response")]
	public class CtoS_0x40_FinishNewQuest: Packet, IObjectIdPacket
	{
		protected byte response;
		protected byte optionalRewardsSelected;
		protected byte[] optionalRewardsChoice = new byte[8];
		protected ushort unk1;
		protected uint unk2;
		protected ushort questId;
		protected ushort objectId;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { objectId }; }
		}

		#region public access properties

		public byte Response { get { return response; } }
		public byte OptionalRewardsSelected { get { return optionalRewardsSelected; } }
		public byte[] OptionalRewardsChoice { get { return optionalRewardsChoice; } }
		public ushort Unk1 { get { return unk1; } }
		public uint Unk2 { get { return unk2; } }
		public ushort QuestId { get { return questId; } }
		public ushort ObjectId { get { return objectId; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("response:{0} rewardsSelected:{1} questID:0x{2:X4} objectId:0x{3:X4} unk1:0x{4:X4} unk2:0x{5:X8}\n\t",
				response, optionalRewardsSelected, questId, objectId, unk1, unk2);

			int i = 0;
			if (i < optionalRewardsSelected)
				text.Write("selected rewards:[");
			bool skipFirstSeparator = true;
			for (; i < optionalRewardsSelected; i++)
			{
				if (!skipFirstSeparator)
					text.Write(',');
				text.Write("{0}", optionalRewardsChoice[i]);
				skipFirstSeparator = false;
			}
			if (i > 0)
				text.Write("]");
			if (i < 8)
				text.Write(" not used :(");
			skipFirstSeparator = true;
			for (; i < 8; i++)
			{
				if (!skipFirstSeparator)
					text.Write(',');
				text.Write("{0}", optionalRewardsChoice[i]);
				skipFirstSeparator = false;
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			response = ReadByte();
			optionalRewardsSelected = ReadByte();
			for (int i = 0; i < 8; i++ )
				optionalRewardsChoice[i] = ReadByte();
			unk1 = ReadShort();
			unk2 = ReadInt();
			questId = ReadShort();
			objectId = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x40_FinishNewQuest(int capacity) : base(capacity)
		{
		}
	}
}