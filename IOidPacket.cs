using System.Collections.Generic;

namespace PacketLogConverter
{
	/// <summary>
	/// Packet with session ID data.
	/// </summary>
	public interface ISessionIdPacket
	{
		/// <summary>
		/// Gets the session id of the packet.
		/// </summary>
		/// <value>The session id.</value>
		ushort SessionId { get; }
	}

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

	/// <summary>
	/// Packet with Keep ID data.
	/// </summary>
	public interface IKeepIdPacket
	{
		/// <summary>
		/// Gets the keep ids of the packet.
		/// </summary>
		/// <value>The keep ids.</value>
		ushort[] KeepIds { get; }
	}

	/// <summary>
	/// Packet with House ID data.
	/// </summary>
	public interface IHouseIdPacket
	{
		/// <summary>
		/// Gets the house id of the packet.
		/// </summary>
		/// <value>The house id.</value>
		ushort HouseId { get; }
	}
}
