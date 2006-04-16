using System;
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x85, -1, ePacketDirection.ClientToServer, "Find looking for group")]
	public class CtoS_0x85_FindLookingForGroupFilter: CtoS_0x84_LookingForGroupFilter
	{

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x85_FindLookingForGroupFilter(int capacity) : base(capacity)
		{
		}
	}
}