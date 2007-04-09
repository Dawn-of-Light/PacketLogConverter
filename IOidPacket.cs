using System.Collections.Generic;

namespace PacketLogConverter
{
	/// <summary>
	/// Packet with Object ID data.
	/// </summary>
	public interface IObjectIdPacket
	{
		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		ushort[] ObjectIds { get; }
	}
}
