using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x40, -1, ePacketDirection.ClientToServer, "Finish newQuest response")]
	public class CtoS_0x40_FinishNewQuest: Packet, IObjectIdPacket
	{
		protected byte response;
		protected byte rewardSelected;
		protected byte rewardOptionalChoice;
		protected byte unk1;
		protected uint unk2;
		protected uint unk3;
		protected uint unk4;
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
		public byte RewardSelected { get { return rewardSelected; } }
		public byte RewardOptionalChoice { get { return rewardOptionalChoice; } }
		public byte Unk1 { get { return unk1; } }
		public uint Unk2 { get { return unk2; } }
		public uint Unk3 { get { return unk3; } }
		public uint Unk4 { get { return unk4; } }
		public ushort QuestId { get { return questId; } }
		public ushort ObjectId { get { return objectId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("response:{0} rewardSelected:{1} rewardOptionalChoice:{2} questID:0x{4:X4} objectId:0x{5:X4} unk1:0x{3:X2} 0x{6:X8} 0x{7:X8} 0x{8:X8}",
				response, rewardSelected, rewardOptionalChoice, unk1, questId, objectId, unk2, unk3, unk4);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			response = ReadByte();
			rewardSelected = ReadByte();
			rewardOptionalChoice = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadIntLowEndian();
			unk3 = ReadIntLowEndian();
			unk4 = ReadIntLowEndian();
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