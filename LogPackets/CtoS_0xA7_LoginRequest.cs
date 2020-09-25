using System.Collections;
using System.IO;
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
		protected uint[] AunkI;
		protected uint edi;
		protected uint unk1;
		protected uint unk2;
		protected uint unk3;
		protected uint AunkB;
		protected uint aunk1;//client signature or CRC
		protected uint aunk2;
		protected uint aunk3;
		protected uint aunk4;
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
		public uint Signature { get { return aunk1; } }
		public uint Aunk2 { get { return aunk2; } }
		public uint Aunk3 { get { return aunk3; } }
		public uint Aunk4 { get { return aunk4; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("version:{0}.{1}.{2} accountName:\"{3}\" accountPassword:\"{4}\" cryptKeyRequests:0x{5:X2}",
				clientVersionMajor, clientVersionMinor, clientVersionBuild, clientAccountName, clientAccountPassword, cryptKeyRequests);
			if (flagsDescription)
			{
				if (AunkI.Length > 0)
				{
					text.Write("\n\tAunkI:");
					for (byte i = 0; i < AunkI.Length; i++)
						text.Write(" 0x{0:X8}", AunkI[i]);
				}
				text.Write(" unkB1:0x{0:X2} unkS1:0x{1:X4}", unkB1, unkS1);
				text.Write("\n\tinfo different with and without logger:");
				text.Write("\n\tAunk1:0x{0:X8} 0x{1:X8} 0x{2:X8} 0x{3:X8}", aunk1, aunk2, aunk3, aunk4);
				text.Write("\n\tunk1 :0x{0:X8} unk2:0x{1:X8} unk3:0x{2:X8}", unk1, unk2, unk3);
				text.Write("\n\tEDI: 0x{0:X8} stack?:0x{1:X8}", edi, AunkB);
			}
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			if (log.SubversionReinit)
				return;
			Position = 2;
			int major = ReadByte();
			int minor = ReadByte();
			int build = ReadByte();
			int version = major*100 + minor*10 + build;
			if (version >= 200)
				version = version + 900;
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
			unk3 = ReadIntLowEndian();
			aunk1 = ReadIntLowEndian();
			aunk2 = ReadIntLowEndian();
			aunk3 = ReadIntLowEndian();
			aunk4 = ReadIntLowEndian();
			unk1 = ReadIntLowEndian();
			edi = ReadIntLowEndian();
			AunkB = ReadIntLowEndian();
			cryptKeyRequests = 0;
			ArrayList arr = new ArrayList(4);
			for(byte i = 0; i < 3; i++)
			{
				arr.Add(ReadIntLowEndian());
			}
			AunkI = (uint[])arr.ToArray(typeof (uint));
			unk2 = ReadIntLowEndian();
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