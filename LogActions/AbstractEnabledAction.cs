using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Base class for typical enabled actions.
	/// </summary>
	public abstract class AbstractEnabledAction : ILogAction
	{
		/// <summary>
		/// Determines whether the action is enabled.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns>Always <c>true</c>.</returns>
		public bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			return true;
		}

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public abstract bool Activate(IExecutionContext context, PacketLocation selectedPacket);
	}
}
