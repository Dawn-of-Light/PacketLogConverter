using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x82, -1, ePacketDirection.ClientToServer, "Dialog response")]
	public class CtoS_0x82_DialogResponse : Packet, IObjectIdPacket, ISessionIdPacket
	{
		protected ushort data1;
		protected ushort data2;
		protected ushort data3;
		protected byte dialogCode;
		protected byte response;

		// TODO when resopnse type is parsed
		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get
			{
				switch ((eDialogCode)dialogCode)
				{
//					case eDialogCode.CustomDialog://data1=sessionId
					case eDialogCode.InvitedJoinGuild://data1=oid
					case eDialogCode.InvitedToBoard://data1=oid
					case eDialogCode.RequestedPermissionToClaim://data1=oid
					case eDialogCode.Claim:
						return new ushort[] { data1 };
					case eDialogCode.QuestSubscribe:
						return new ushort[] { data2 };
					default:
						return new ushort[] { };
				}
			}
		}

		public ushort SessionId
		{
			get
			{
				if ((eDialogCode)dialogCode == eDialogCode.CustomDialog)
					return data1;
				return 0;
			}
		}
		#region public access properties

		public ushort Data1 { get { return data1; } }
		public ushort Data2 { get { return data2; } }
		public ushort Data3 { get { return data3; } }
		public byte DialogCode { get { return dialogCode ; } }
		public byte Response { get { return response; } }

		#endregion

		public enum eDialogCode: byte
		{
			InvitedJoinGroup = 0x02,
			InvitedJoinGuild = 0x03,
			InvitedJoinChatGroup = 0x04,
			SetLastname = 0x05,
			CustomDialog = 0x06,
			RestoreConstitution = 0x07,
			LeaveGuild = 0x08,
			Enchant = 0x09,
			Appeal = 0x0A,
			SelectCraftOrder = 0x0B,
			AddEmblem = 0x0C,
			Repair = 0x0D,
			Recharge = 0x0F,
			RespecAllSkills = 0x10,
			PurchaseHouseLot = 0x11,
			TransferHomeToGuildHouse = 0x13,
			DepositHouseRent = 0x14,
			HouseUpgradeDowngrade = 0x15,
			InvitedToBoard = 0x16,
			RequestedPermissionToClaim = 0x17,
			InvitedJoinBattleGroup = 0x18,
			ML = 0x19,
			Claim = 0x1A,
			PurchaseBanner = 0x1E,
			BuySingleRespec = 0x20,
//			NewQuestFinish = 0x21,//Not exist
//			NewQuestSubscribe = 0x22,//Not exist
			HiberniaWarmap = 0x30,
			AlbionWarmap= 0x31,
			MidgardWarmap = 0x32,
			QuestSubscribe = 0x64,

		}

		public enum eResponseType: byte
		{
			Decline = 0,
			Accept = 1
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string template = "data1:0x{0:X4} data2:0x{1:X4} data3:0x{2:X4} dialogCode:0x{3:X4}({5}) response:{4}";

			switch((eDialogCode)dialogCode)
			{
				case eDialogCode.CustomDialog:
					template = "sessionId:0x{0:X4} data2:0x{1:X4} data3:0x{2:X4} dialogCode:0x{3:X4}({5}) response:{4}";
					break;
				case eDialogCode.ML:
					template = "data1:0x{0:X4} data2:0x{1:X4} data3:0x{2:X4} dialogCode:0x{3:X4}({5}) level:{4}";
					break;
				case eDialogCode.InvitedToBoard:
				case eDialogCode.InvitedJoinGuild:
				case eDialogCode.RequestedPermissionToClaim:
				case eDialogCode.Claim:
					template = "oid:0x{0:X4} data2:0x{1:X4} data3:0x{2:X4} dialogCode:0x{3:X4}({5}) response:{4}";
					break;
				case eDialogCode.QuestSubscribe:
					template = "questID:0x{0:X4} oid:0x{1:X4} data3:0x{2:X4} dialogCode:0x{3:X4}({5}) response:{4}";
					break;
			}
			str.AppendFormat(template, data1, data2, data3, dialogCode, response, (eDialogCode)dialogCode);

			if(flagsDescription && dialogCode != (byte)eDialogCode.ML)
				str.AppendFormat("({0})", (eResponseType)response);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			data1 = ReadShort();
			data2 = ReadShort();
			data3 = ReadShort();
			dialogCode = ReadByte();
			response = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x82_DialogResponse(int capacity) : base(capacity)
		{
		}
	}
}