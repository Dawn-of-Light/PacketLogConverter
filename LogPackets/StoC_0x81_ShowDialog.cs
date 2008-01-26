using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x81, -1, ePacketDirection.ServerToClient, "Show dialog")]
	public class StoC_0x81_ShowDialog : Packet, IObjectIdPacket, ISessionIdPacket
	{
		protected ushort dialogCode;
		protected ushort data1;
		protected ushort data2;
		protected ushort data3;
		protected ushort data4;
		protected byte dialogType;
		protected byte autoWrapText;
		protected ASubData subData;


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
					case eDialogCode.NewQuestFinish:
					case eDialogCode.NewQuestSubscribe:
					case eDialogCode.QuestSubscribe:
						return new ushort[] { data2 };
//					case eDialogCode.CustomDialog://data1=sessionId
					case eDialogCode.InvitedJoinGroup://data1=sessionId (responce in CtoS_0x98)
					case eDialogCode.InvitedJoinGuild://data1=oid
					case eDialogCode.InvitedToBoard://data1=oid
					case eDialogCode.RequestedPermissionToClaim://data1=oid
					case eDialogCode.Claim://data1=oid
						return new ushort[] { data1 };
					default: return new ushort[] { };
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

		public ushort DialogCode { get { return dialogCode; } }
		public ushort Data1 { get { return data1; } }
		public ushort Data2 { get { return data2; } }
		public ushort Data3 { get { return data3; } }
		public ushort Data4 { get { return data4; } }
		public byte DialogType { get { return dialogType; } }
		public byte AutoWrapText { get { return autoWrapText; } }
		public ASubData SubData { get { return subData; } }

		#endregion

		public enum eDialogCode: byte
		{
			MessageBox = 0x00,
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
//			TransferOwnerships = 0x12, ?
			TransferHomeToGuildHouse = 0x13,
			DepositHouseRent = 0x14,
			HouseUpgradeDowngrade = 0x15, // "Housing10" from help.txt(ihf.mpk)
			InvitedToBoard = 0x16,
			RequestedPermissionToClaim = 0x17,
			InvitedJoinBattleGroup = 0x18,
			Claim = 0x1A,
			PurchaseBanner = 0x1E,
			BuySingleRespec = 0x20,
			NewQuestFinish = 0x21,
			NewQuestSubscribe = 0x22,
			HiberniaWarmap = 0x30,
			AlbionWarmap= 0x31,
			MidgardWarmap = 0x32,
			QuestSubscribe = 0x64,

		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			string template = "(dialogCode:0x{0:X4}({7}) data1:0x{1:X4} data2:0x{2:X4} data3:0x{3:X4} data4:0x{4:X4} dialogType:{5} autoWrapText:{6}";

			switch((eDialogCode)dialogCode)
			{
				case eDialogCode.CustomDialog:
				case eDialogCode.InvitedJoinGroup:
					template = "dialogCode:0x{0:X4}({7}) sessionId:0x{1:X4} data2:0x{2:X4} data3:0x{3:X4} data4:0x{4:X4} dialogType:{5} autoWrapText:{6}";
					break;
				case eDialogCode.InvitedJoinGuild:
				case eDialogCode.InvitedToBoard:
				case eDialogCode.RequestedPermissionToClaim:
				case eDialogCode.Claim:
					template = "dialogCode:0x{0:X4}({7}) oid:0x{1:X4} data2:0x{2:X4} data3:0x{3:X4} data4:0x{4:X4} dialogType:{5} autoWrapText:{6}";
					break;
				case eDialogCode.NewQuestFinish:
				case eDialogCode.NewQuestSubscribe:
				case eDialogCode.QuestSubscribe:
					template = "dialogCode:0x{0:X4}({7}) questID:0x{1:X4} oid:0x{2:X4} data3:0x{3:X4} data4:0x{4:X4} dialogType:{5} autoWrapText:{6}";
					break;
			}
			text.Write(template, dialogCode, data1, data2, data3, data4, dialogType, autoWrapText, (eDialogCode)dialogCode);
			subData.MakeString(text, flagsDescription);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			dialogCode = ReadShort();
			data1 = ReadShort();
			data2 = ReadShort();
			data3 = ReadShort();
			data4 = ReadShort();
			dialogType = ReadByte();
			autoWrapText = ReadByte();
			InitSubcode(dialogCode);
		}

		protected void InitSubcode(ushort code)
		{
			switch ((eDialogCode)code)
			{
				case eDialogCode.NewQuestFinish:
				case eDialogCode.NewQuestSubscribe: InitNewQuestUpdate(); break;
				case eDialogCode.HiberniaWarmap:
				case eDialogCode.AlbionWarmap:
				case eDialogCode.MidgardWarmap: InitWarmapDialogUpdate(); break;
				default: InitDialogUpdate(); break;
			}
			return;
		}

		/// <summary>
		/// Base abstract class for all sub codes data
		/// </summary>
		public abstract class ASubData
		{
			abstract public void Init(StoC_0x81_ShowDialog pak);
			abstract public void MakeString(TextWriter text, bool flagsDescription);
		}

		protected virtual void InitDialogUpdate()
		{
			subData = new DialogUpdate();
			subData.Init(this);
		}

		public class DialogUpdate : ASubData
		{
			public string message;
			public override void Init(StoC_0x81_ShowDialog pak)
			{
				message = pak.ReadString();
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
				text.Write("\n\t\"{0}\"", message);
			}
		}

		protected virtual void InitWarmapDialogUpdate()
		{
			subData = new WarmapDialogUpdate();
			subData.Init(this);
		}

		public class WarmapDialogUpdate : ASubData
		{
			public override void Init(StoC_0x81_ShowDialog pak)
			{
			}

			public override void MakeString(TextWriter text, bool flagsDescription)
			{
			}
		}

		protected virtual void InitNewQuestUpdate()
		{
			subData = new NewQuestUpdate();
			subData.Init(this);
		}

		public class NewQuestUpdate: ASubData
		{
			public override void Init(StoC_0x81_ShowDialog pak)
			{
			}
			public override void MakeString(TextWriter text, bool flagsDescription)
			{
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x81_ShowDialog(int capacity) : base(capacity)
		{
		}
	}
}