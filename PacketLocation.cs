using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter
{
	public struct PacketLocation : IEquatable<PacketLocation>
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

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="packetLocation">The packet location.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public bool Equals(PacketLocation packetLocation)
		{
			return this == packetLocation;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>
		/// true if obj and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (!(obj is PacketLocation)) return false;
			return Equals((PacketLocation) obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return LogIndex + 29*PacketIndex;
		}
	}
}
