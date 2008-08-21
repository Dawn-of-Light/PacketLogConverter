using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x37, -1, ePacketDirection.ClientToServer, "Client crash")]
	public class CtoS_0x37_ClientCrash: Packet
	{
		protected string module;
		protected string version;
		protected uint errorCode;
		protected uint cs;
		protected uint eip;
		protected uint options;
		protected byte[] gsxm = new byte[16];
		protected byte clnRegionExpantions;
		protected byte clnType;
		protected byte osType;
		protected byte terrainOption;
		protected ushort region;
		protected ushort unk1;
		protected uint uptime;
		protected uint stack1;
		protected uint stack2;
		protected uint stack3;
		protected uint stack4;

		#region public access properties

		public string Module { get { return module; } }
		public string Version { get { return version; } }
		public uint ErrorCode { get { return errorCode; } }
		public uint CS { get { return cs; } }
		public uint EIP { get { return eip; } }
		public uint Options { get { return options; } }
		public byte ClnRegionExpantions { get { return clnRegionExpantions; } }
		public byte ClnType { get { return clnType; } }
		public byte OS { get { return osType; } }
		public byte TerrainOption { get { return terrainOption; } }
		public ushort Region { get { return region; } }
		public ushort Unk1 { get { return unk1; } }
		public uint Uptime { get { return uptime; } }
		public uint Stack1 { get { return stack1; } }
		public uint Stack2 { get { return stack1; } }
		public uint Stack3 { get { return stack1; } }
		public uint Stack4 { get { return stack1; } }

		#endregion

		public enum eClientType : int
		{
			Classic = 1,
			TrialsOfAtlantis = 2,
			Catacombs = 3,
			ShroudedIsles = 4,
			DarknessRising = 6,
			Labyrinth = 7,
		}

		public enum eErrorCode: uint
		{
			a_Control_C = 0x40010005,
			a_Control_Break = 0x40010008,
			a_Datatype_Misalignment = 0x80000002,
			a_Breakpoint = 0x80000003,
			an_Access_Violation = 0xc0000005,
			an_In_Page_Error = 0xc0000006,
			a_No_Memory = 0xc0000017,
			an_Illegal_Instruction = 0xc000001d,
			a_Noncontinuable_Exception = 0xc0000025,
			an_Invalid_Disposition = 0xc0000026,
			a_Array_Bounds_Exceeded = 0xc000008c,
			a_Float_Denormal_Operand = 0xc000008d,
			a_Float_Divide_by_Zero = 0xc000008e,
			a_Float_Inexact_Result = 0xc000008f,
			a_Float_Invalid_Operation = 0xc0000090,
			a_Float_Overflow = 0xc0000091,
			a_Float_Stack_Check = 0xc0000092,
			a_Float_Underflow = 0xc0000093,
			an_Integer_Divide_by_Zero = 0xc0000094,
			an_Integer_Overflow = 0xc0000095,
			a_Privileged_Instruction = 0xc0000096,
			a_Stack_Overflow = 0xc00000fD,
			a_DLL_Initialization_Failed = 0xc0000142,
			a_Microsoft_CPP_Exception = 0xe06d7363
		}

		public enum eOSType : int
		{
			WIN95 = 1,
			WIN98 = 2,
			WindowsMe = 3,
			NT351 = 4,
			NT4 = 5,
			WIN2000 = 6,
			WINXP = 7,
			WIN2003 = 8,
		}

		public enum eOptionFlags: int
		{
			WindowMode = 0x01,
			SecondCopyDaoc = 0x02,
			UseAtlantisTerrain = 0x04, // OldTerrainFlag
			PerfOpt_OldTerrainFlag = 0x08, //?
			PerfOpt_AtlantisTreeFlag = 0x10, // Atlantis TreeFlag ?
			CustomSkin = 0x20, // Custom Skin ?
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("module:{0} version:{1} errorCode:0x{2:X8} CS=0x{3:X8} EIP=0x{4:X8}",
				module, version, errorCode, cs, eip);
			text.Write("\n\tOS:{0} region:{1,-3} uptime:{2}s gameOptions:0x{3:X2}",
				osType, region, uptime, options);
			if (flagsDescription && options > 0)
			{
				text.Write('(');
				uint uOptionFlags = options;
				byte i = 0;
				foreach(eOptionFlags eOption in Enum.GetValues(typeof(eOptionFlags)))
				{
					if ((options & (uint)eOption) == (uint)eOption)
					{
						uOptionFlags ^= (uint)eOption;
						if (i++ > 0)
							text.Write(", ");
						text.Write(eOption.ToString());
					}
				}
				if (uOptionFlags > 0)
					text.Write(" uOptionFlags:0x{0:X2})", uOptionFlags);
				text.Write(')');
			}
			text.Write("\n\tstack:0x{0:X8} 0x{1:X8} 0x{2:X8} 0x{3:X8}", stack1, stack2, stack3, stack4);
			if (flagsDescription)
			{
				text.Write("\n\tclnRegionExpantions:{0} clnType:{1}({4}) OS:{2}({5}) terrainOption:{3}", clnRegionExpantions, clnType, osType, terrainOption, (eClientType)clnType, (eOSType)osType);
				text.Write("\n\t{0} caused {1} in module {2} at {3:X4}:{4:X8}.", "?", ((eErrorCode)errorCode).ToString().Replace("_", " "), module, cs, eip);
				text.Write("\n\tGSXM:");
				for(int i = 0; i < 16; i++)
					text.Write(" {0:0000}", gsxm[i]);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			module = ReadString(32); // 0x00
			version = ReadString(8); // 0x20
			errorCode = ReadInt();   // 0x28
			cs = ReadInt();          // 0x2C
			eip = ReadInt();         // 0x30
			options = ReadInt(); // 0x34
			for (int i = 0; i < 16; i++) // 0x38+
				gsxm[i] = ReadByte(); // *(ebp + i * 4 - 0x40)
			clnRegionExpantions = ReadByte(); // 0x48 hardcoded
			clnType = ReadByte(); // 0x49 (3-7)
			osType = ReadByte(); // 0x4A
			terrainOption = ReadByte(); // 0x4B
			region = ReadShort(); // 0x4C
			unk1 = ReadShort(); // always = 0
			uptime = ReadInt(); // 0x50
			stack1 = ReadIntLowEndian(); // 0x54
			stack2 = ReadIntLowEndian(); // 0x58
			stack3 = ReadIntLowEndian(); // 0x5c
			stack4 = ReadIntLowEndian(); // 0x60
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x37_ClientCrash(int capacity) : base(capacity)
		{
		}
	}
}