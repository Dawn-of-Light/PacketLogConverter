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
		/// <param name="add">if set to <c>true</c> events are added, removed otherwise.</param>
		public void SetEventHandlers(bool add)
		{
			if (add)
			{
				// Add all events
				FilterManager.FilteringStartedEvent += Start;
				FilterManager.FilteringPacketEvent += ProcessPacket;
				FilterManager.FilteringStoppedEvent += Stop;
			}
			else
			{
				// Remove all events
				FilterManager.FilteringStoppedEvent -= Stop;
				FilterManager.FilteringPacketEvent -= ProcessPacket;
				FilterManager.FilteringStartedEvent -= Start;
			}
		}
	}
}
