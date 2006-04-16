namespace PacketLogConverter
{
	/// <summary>
	/// All methods needed for a log filter
	/// </summary>
	public interface ILogFilter
	{
		/// <summary>
		/// Activates the filter
		/// </summary>
		/// <returns>True if filter has changed</returns>
		bool ActivateFilter();
		bool IsPacketIgnored(Packet packet);
		bool IsFilterActive { get; }
	}
}
