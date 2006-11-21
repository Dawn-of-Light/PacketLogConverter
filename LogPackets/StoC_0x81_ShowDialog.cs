using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x81, -1, ePacketDirection.ServerToClient, "Show dialog")]
	public class StoC_0x81_ShowDialog : Packet, IOidPacket
	{
		protected ushort dialogCode;
		protected ushort data1;
		protected ushort data2;
		protected ushort data3;
		protected ushort data4;
		protected byte dialogType;
		protected byte autoWrapText;
		protected ASubData subData;


		public int Oid1
		{
			get
			{
				switch (dialogType)
				{
					default: return ushort.MinValue;
				}
			}
		}

		public int Oid2
		{
			get
			{
				switch (dialogType)
				{
					default: return ushort.MinValue;
				}
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

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string template = "dialogCode:0x{0:X4} data1:0x{1:X4} data2:0x{2:X4} data3:0x{3:X4} data4:0x{4:X4} dialogType:{5} autoWrapText:{6}";

			switch(dialogCode)
			{
				case 6: template = "CUSTOM DIALOG sessionId:0x{1:X4} customDialog?:0x{2:X4} data3:0x{3:X4} data4:0x{4:X4} dialogType:{5} autoWrapText:{6}"; break;
			}
			str.AppendFormat(template, dialogCode, data1, data2, data3, data4, dialogType, autoWrapText);
			subData.MakeString(str, flagsDescription);

			return str.ToString();
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
			switch (code)
			{
				case 0x21:
				case 0x22: InitNewQuestUpdate(); break;
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
			abstract public void MakeString(StringBuilder str, bool flagsDescription);
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

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				str.AppendFormat("\n\t\"{0}\"", message);
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
			public override void MakeString(StringBuilder str, bool flagsDescription)
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