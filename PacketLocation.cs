using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter
{
	public struct PacketLocation
	{
		public static PacketLocation UNKNOWN = new PacketLocation(-1, -1);

		public int LogIndex;
		public int PacketIndex;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PacketLocation"/> class.
		/// </summary>
		/// <param name="logIndex">Index of the log.</param>
		/// <param name="packetIndex">Index of the packet.</param>
		public PacketLocation(int logIndex, int packetIndex)
		{
			LogIndex = logIndex;
			PacketIndex = packetIndex;
		}

		/// <summary>
		/// Compares 2 packet locations.
		/// </summary>
		/// <param name="location1">The location1.</param>
		/// <param name="location2">The location2.</param>
		/// <returns><c>true</c> if both locations are equal.</returns>
		public static bool operator ==(PacketLocation location1, PacketLocation location2)
		{
			bool ret = location1.PacketIndex == location2.PacketIndex;
			ret &= location1.LogIndex == location2.LogIndex;
			return ret;
		}

		/// <summary>
		/// Compares 2 packet locations.
		/// </summary>
		/// <param name="location1">The location1.</param>
		/// <param name="location2">The location2.</param>
		/// <returns><c>true</c> if both locations are equal.</returns>
		public static bool operator !=(PacketLocation location1, PacketLocation location2)
		{
			bool ret = location1 == location2;
			return !ret;
		}
	}
}
