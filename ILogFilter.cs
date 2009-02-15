using System.IO;

namespace PacketLogConverter
{
	/// <summary>
	/// All methods needed for a log filter
	/// </summary>
	public interface ILogFilter
	{
		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns>
		/// 	<code>true</code> if filter has changed and log should be updated.
		/// </returns>
		bool ActivateFilter();
		
		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		bool IsPacketIgnored(Packet packet);

		/// <summary>
		/// Gets or sets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		bool IsFilterActive { get; set; }
		
		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		bool Serialize(MemoryStream data);

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		bool Deserialize(MemoryStream data);
	}
}
