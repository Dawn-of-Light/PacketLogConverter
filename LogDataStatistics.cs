using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter
{
	public struct LogDataStatistics
	{
		public static readonly LogDataStatistics Zero;

		public int PacketsCount;
		public int PacketsCountInTCP;
		public int PacketsCountOutTCP;
		public int PacketsCountInUDP;
		public int PacketsCountOutUDP;
	}
}
