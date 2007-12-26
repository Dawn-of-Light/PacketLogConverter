using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Helper class for dynamic filters. Encapsulates event management.
	/// </summary>
	public struct DynamicFilterHelper
	{
		public LogAction Start;
		public LogAction Stop;
		public PacketAction ProcessPacket;

		/// <summary>
		/// Sets the events.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="add">if set to <c>true</c> events are added, removed otherwise.</param>
		public void SetEventHandlers(IExecutionContext context, bool add)
		{
			if (add)
			{
				// Add all events
				context.FilterManager.FilteringStartedEvent += Start;
				context.FilterManager.FilteringPacketEvent += ProcessPacket;
				context.FilterManager.FilteringStoppedEvent += Stop;
			}
			else
			{
				// Remove all events
				context.FilterManager.FilteringStoppedEvent -= Stop;
				context.FilterManager.FilteringPacketEvent -= ProcessPacket;
				context.FilterManager.FilteringStartedEvent -= Start;
			}
		}
	}
}
