namespace PacketLogConverter
{
	public interface ILogAction
	{
		/// <summary>
		/// Activate log action
		/// </summary>
		/// <param name="log">The current log</param>
		/// <param name="selectedIndex">The selected packet index</param>
		/// <returns>True if log data tab should be updated</returns>
		bool Activate(PacketLog log, int selectedIndex);
	}
}
