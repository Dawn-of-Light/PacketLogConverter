namespace PacketLogConverter
{
	/// <summary>
	/// An action on selected in the log packet.
	/// </summary>
	public interface ILogAction
	{
#warning TODO: Add IsEnabled(packet) method to disable certain actions

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		bool Activate(IExecutionContext context, PacketLocation selectedPacket);
	}
}
