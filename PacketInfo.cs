using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter
{
	/// <summary>
	/// Holds packet and its text index.
	/// </summary>
	public struct PacketInfo
	{
		public Packet Packet;
		public int TextEndIndex;
		public PacketLocation PacketLocation;

		public static PacketInfo UNKNOWN = new PacketInfo(null, -1, PacketLogConverter.PacketLocation.UNKNOWN);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PacketInfo"/> class.
		/// </summary>
		/// <param name="newPacket">The new packet.</param>
		/// <param name="newTextIndex">New index of the text.</param>
		/// <param name="newPacketLocation">New packet location.</param>
		public PacketInfo(Packet newPacket, int newTextIndex, PacketLocation newPacketLocation)
		{
			Packet = newPacket;
			TextEndIndex = newTextIndex;
			PacketLocation = newPacketLocation;
		}
	}
}
