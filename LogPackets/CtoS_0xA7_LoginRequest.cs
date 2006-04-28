using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, -1, ePacketDirection.ClientToServer, "Login request")]
	public class CtoS_0xA7_LoginRequest : Packet
	{
		protected ushort clientType;
		protected byte clientVersionMajor;
		protected byte clientVersionMinor;
		protected byte clientVersionBuild;
		protected string clientAccountPassword;
		protected string clientAccountName;
		protected uint[] Aunk1;
		protected uint[] Aunk2;
		protected byte[] AunkB;
		protected uint unk1;
		protected uint unk2;
		protected uint unk3;
		protected ushort unkS1;
		protected byte unkB1;
		protected byte cryptKeyRequests;

		#region public access properties

		public ushort ClientType { get { return clientType; } }
		public byte ClientVersionMajor { get { return clientVersionMajor; } }
		public byte ClientVersionMinor { get { return clientVersionMinor; } }
		public byte ClientVersionBuild { get { return clientVersionBuild; } }
		public string ClientAccountPassword { get { return clientAccountPassword; } }
		public string ClientAccountName { get { return clientAccountName; } }
		public byte CryptKeyRequests { get { return cryptKeyRequests; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("version:{0}.{1}.{2} accountName:\"{3}\" accountPassword:\"{4}\" cryptKeyRequests:0x{5:X2}",
				clientVersionMajor, clientVersionMinor, clientVersionBuild, clientAccountName, clientAccountPassword, cryptKeyRequests);
			if (flagsDescription)
			{
				if (Aunk1.Length > 0)
				{
					str.Append("\n\tAunk1:");
					for (int i = 0; i < Aunk1.Length; i++)
						str.AppendFormat(" 0x{0:X8}", Aunk1[i]);
				}
				if (Aunk2.Length > 0)
				{
					str.Append("\n\tAunk2:");
					for (int i = 0; i < Aunk2.Length; i++)
						str.AppendFormat(" 0x{0:X8}", Aunk2[i]);
				}
				if (AunkB.Length > 0)
				{
					str.Append("\n\tAunkB:");
					for (int i = 0; i < AunkB.Length; i++)
						str.AppendFormat(" 0x{0:X2}", AunkB[i]);
				}
				str.AppendFormat("\n\tunk1:0x{0:X8} unk2:0x{1:X8} unk3:0x{2:X8} unkB1:0x{3:X2} unkS1:0x{4:X4}", unk1, unk2, unk3, unkB1, unkS1);
			}

			return str.ToString();
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			Position = 2;
			int major = ReadByte();
			int minor = ReadByte();
			int build = ReadByte();
			int version = major*100 + minor*10 + build;
			log.Version = version;
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientType = ReadShort();
			clientVersionMajor = ReadByte();
			clientVersionMinor = ReadByte();
			clientVersionBuild = ReadByte();
			clientAccountPassword = ReadString(19);
			unk3 = ReadInt();
			ArrayList arr = new ArrayList(8);
			for(byte i = 0; i < 4; i++)
			{
				arr.Add(ReadInt());
			}
			Aunk1 = (uint[])arr.ToArray(typeof (uint));
			unk1 = ReadInt();
			arr.Clear();
			for(byte i = 0; i < 8; i++)
				arr.Add(ReadByte());
			AunkB = (byte[])arr.ToArray(typeof (byte));
			cryptKeyRequests = AunkB[3];
			arr.Clear();
			for(byte i = 0; i < 3; i++)
			{
				arr.Add(ReadInt());
			}
			Aunk2 = (uint[])arr.ToArray(typeof (uint));
			unk2 = ReadInt();
			unkB1 = ReadByte();
			unkS1 = ReadShort();
//			Skip(50);
			clientAccountName = ReadString(20);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest(int capacity) : base(capacity)
		{
		}
	}
}