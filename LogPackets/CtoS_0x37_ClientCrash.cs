using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x37, -1, ePacketDirection.ClientToServer, "Client crash")]
	public class CtoS_0x37_ClientCrash: Packet
	{
		protected string module;
		protected string version;
		protected byte unk1;
		protected ushort cs;
		protected uint eip;
		protected byte options;
		protected byte unk4;
		protected byte clnType;
		protected byte procType;
		protected byte unk6;
		protected ushort region;
		protected uint uptime;
		protected uint stack1;
		protected uint stack2;
		protected uint stack3;
		protected uint stack4;

		#region public access properties

		public string Module { get { return module; } }
		public string Version { get { return version; } }
		public byte Unk1 { get { return unk1; } }
		public ushort CS { get { return cs; } }
		public uint EIP { get { return eip; } }
		public byte Options { get { return options; } }
		public byte Unk4 { get { return unk4; } }
		public byte ClnType { get { return clnType; } }
		public byte ProcType { get { return procType; } }
		public byte Unk6 { get { return unk6; } }
		public ushort Region { get { return region; } }
		public uint Uptime { get { return uptime; } }
		public uint Stack1 { get { return stack1; } }
		public uint Stack2 { get { return stack1; } }
		public uint Stack3 { get { return stack1; } }
		public uint Stack4 { get { return stack1; } }

		#endregion

		public enum eClientType : int
		{
			Classic = 1,
//			ShroudedIsles = ?,
			TrialsOfAtlantis = 2,
			Catacombs = 3,
			DarknessRising = 6,
			Labyrinth = 7,
		}

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("module:{0} version:{1} CS=0x{2:X4} EIP=0x{3:X8} processorType:{4} region:{5,-3} uptime:{6}s codeError:0x{7:X2}(windowMode:{8}{9} error:{10})",
				module, version, cs, eip, procType, region, uptime, options, options & 0x1, ((options & 0x02) == 0x02) ? ", SecondCopyDaoc" : "", options >> 2);
			str.AppendFormat("\n\tstack:0x{0:X8} 0x{1:X8} 0x{2:X8} 0x{3:X8}", stack1, stack2, stack3, stack4);
			if (flagsDescription)
				str.AppendFormat("\n\tunk4:{0} clnType?:{1}({4}) procType?:{2} unk6:{3}", unk4, clnType, procType, unk6, (eClientType)clnType);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			module = ReadString(32);
			version = ReadString(11);
			unk1 = ReadByte();
			Skip(2);
			cs = ReadShort();
			eip= ReadInt();
			Skip(3);
			options = ReadByte();
			Skip(16);
			unk4 = ReadByte();
			clnType = ReadByte();
			procType = ReadByte();
			unk6 = ReadByte();
			region = ReadShort();
			Skip(2);
			uptime = ReadInt();
			stack1 = ReadIntLowEndian();
			stack2 = ReadIntLowEndian();
			stack3 = ReadIntLowEndian();
			stack4 = ReadIntLowEndian();
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