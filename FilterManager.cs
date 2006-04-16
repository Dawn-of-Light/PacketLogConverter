using System.Collections;

namespace PacketLogConverter
{
	public delegate void FilterAdded(ILogFilter filter);
	public delegate void FilterRemoved(ILogFilter filter);

	/// <summary>
	/// Manages log filters
	/// </summary>
	public sealed class FilterManager
	{
		private static readonly ArrayList m_filters = new ArrayList();
		private static bool m_combineFilters;
		private static bool m_invertCheck;

		public static event FilterAdded FilterAddedEvent;
		public static event FilterRemoved FilterRemovedEvent;

		public static bool IsPacketIgnored(Packet pak)
		{
			if (CombineFilters) // allow if not ignored by at least on filter
			{
				for (int i = 0; i < m_filters.Count; i++)
				{
					ILogFilter filter = (ILogFilter)m_filters[i];
					if (filter.IsPacketIgnored(pak) == m_invertCheck) // not ignored = allow
						return false;
				}
				return true;
			}
			else
			{
				for (int i = 0; i < m_filters.Count; i++)
				{
					ILogFilter filter = (ILogFilter)m_filters[i];
					if (filter.IsPacketIgnored(pak) != m_invertCheck) // not ignored = allow
						return true;
				}
				return false;
			}
		}

		public static int FiltersCount
		{
			get { return m_filters.Count; }
		}

		public static bool AddFilter(ILogFilter filter)
		{
			if (filter == null)
				return false;
			lock(m_filters.SyncRoot)
			{
				if (m_filters.Contains(filter))
					return false;
				m_filters.Add(filter);
				RaiseFilterAddedEvent(filter);
				return true;
			}
		}

		public static bool CombineFilters
		{
			get { return m_combineFilters; }
			set { m_combineFilters = value; }
		}

		public static bool InvertCheck
		{
			get { return m_invertCheck; }
			set { m_invertCheck = value; }
		}

		private static void RaiseFilterAddedEvent(ILogFilter filter)
		{
			FilterAdded e = FilterAddedEvent;
			if (e != null)
				e(filter);
		}

		public static bool RemoveFilter(ILogFilter filter)
		{
			if (filter == null)
				return false;
			lock(m_filters.SyncRoot)
			{
				if (!m_filters.Contains(filter))
					return false;
				m_filters.Remove(filter);
				RaiseFilterRemovedEvent(filter);
				return true;
			}
		}

		private static void RaiseFilterRemovedEvent(ILogFilter filter)
		{
			FilterRemoved e = FilterRemovedEvent;
			if (e != null)
				e(filter);
		}
	}
}
