using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter
{
	public interface IExecutionContext
	{
		/// <summary>
		/// Gets the log manager.
		/// </summary>
		/// <value>The log manager.</value>
		LogManager LogManager { get; }

		/// <summary>
		/// Gets the filter manager.
		/// </summary>
		/// <value>The filter manager.</value>
		FilterManager FilterManager { get; }
	}
}
