using PacketLogConverter;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// This class encapsulates common logic of a filter which needs to be notified by <see cref="FilterManager"/>.
	/// </summary>
	public abstract class FilterManagerAwareFilter : AbstractFilter
	{
		protected DynamicFilterHelper	m_filterHelper;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:FilterManagerAwareFilter"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
        public FilterManagerAwareFilter(IExecutionContext context) : base(context)
		{
			// Initialize helper class
			m_filterHelper.Start			= FilterManager_OnFilteringStartedEvent;
			m_filterHelper.Stop				= FilterManager_OnFilteringStoppedEvent;
			m_filterHelper.ProcessPacket	= FilterManager_OnFilteringPacketEvent;
        }

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public override bool IsFilterActive
		{
			set
			{
				// Update handlers only if state changes
				if (value != base.IsFilterActive)
				{
					base.IsFilterActive = value;
					m_filterHelper.SetEventHandlers(m_context, value);
				}
			}
		}

		/// <summary>
		/// Processes start of log filtering event.
		/// </summary>
		/// <param name="log">The packets log.</param>
		protected virtual void FilterManager_OnFilteringStartedEvent(IExecutionContext log)
		{
		}

		/// <summary>
		/// Processes stop of log filtering event.
		/// </summary>
		/// <param name="log"></param>
		protected virtual void FilterManager_OnFilteringStoppedEvent(IExecutionContext log)
		{
		}

		/// <summary>
		/// Processes start of filtering of a packet by filter manager.
		/// </summary>
		/// <param name="packet"></param>
		protected virtual void FilterManager_OnFilteringPacketEvent(Packet packet)
		{
		}
	}
}