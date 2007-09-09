using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PacketLogConverter.Utils;

namespace PacketLogConverter
{
	public delegate void FilterAction(ILogFilter filter);
	public delegate void StatusChange(bool newValue);

	/// <summary>
	/// Manages log filters
	/// </summary>
	public static class FilterManager
	{
		private static readonly ArrayList m_filters = new ArrayList();
		private static bool m_combineFilters;
		private static bool m_invertCheck;

		public static event FilterAction FilterAddedEvent;
		public static event FilterAction FilterRemovedEvent;
		public static event StatusChange CombineFiltersChangedEvent;
		public static event StatusChange InvertCheckChangedEvent;

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

		#region Static data

		/// <summary>
		/// Gets or sets a value indicating whether to combine filters.
		/// </summary>
		/// <value><c>true</c> if filter checks combined; otherwise, <c>false</c>.</value>
		public static bool CombineFilters
		{
			get { return m_combineFilters; }
			set
			{
				m_combineFilters = value;
				RaiseStatusChangeEvent(CombineFiltersChangedEvent, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to invert filter check.
		/// </summary>
		/// <value><c>true</c> if filter check is inverted invert; otherwise, <c>false</c>.</value>
		public static bool InvertCheck
		{
			get { return m_invertCheck; }
			set
			{
				m_invertCheck = value;
				RaiseStatusChangeEvent(InvertCheckChangedEvent, value);
			}
		}

		/// <summary>
		/// Raises the status change event.
		/// </summary>
		/// <param name="e">The event delegates.</param>
		/// <param name="value">New value.</param>
		private static void RaiseStatusChangeEvent(StatusChange e, bool value)
		{
			if (null != e)
			{
				e(value);
			}
		}

		#endregion

		#region Add/Remove

		/// <summary>
		/// Adds the filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns><code>true</code> if filter is added, <code>false</code> if already active.</returns>
		public static bool AddFilter(ILogFilter filter)
		{
			if (filter == null)
				return false;
			lock (m_filters.SyncRoot)
			{
				if (m_filters.Contains(filter))
					return false;
				m_filters.Add(filter);
				RaiseFilterAddedEvent(filter);
				return true;
			}
		}

		/// <summary>
		/// Removes the filter.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns><code>true</code> if filter is removed, <code>false</code> if not active already.</returns>
		public static bool RemoveFilter(ILogFilter filter)
		{
			if (filter == null)
				return false;
			lock (m_filters.SyncRoot)
			{
				if (!m_filters.Contains(filter))
					return false;
				m_filters.Remove(filter);
				RaiseFilterRemovedEvent(filter);
				return true;
			}
		}

		/// <summary>
		/// Raises the filter added event.
		/// </summary>
		/// <param name="filter">The filter.</param>
		private static void RaiseFilterAddedEvent(ILogFilter filter)
		{
			FilterAction e = FilterAddedEvent;
			if (e != null)
				e(filter);
		}

		/// <summary>
		/// Raises the filter removed event.
		/// </summary>
		/// <param name="filter">The filter.</param>
		private static void RaiseFilterRemovedEvent(ILogFilter filter)
		{
			FilterAction e = FilterRemovedEvent;
			if (e != null)
				e(filter);
		}
		
		#endregion

		#region Save/Load
		
		public static void SaveFilters(string path)
		{
			using (FilterWriter writer = new FilterWriter(path))
			{
				try
				{
					// Clone list of filters to avoid multithreading problems
					ArrayList allFilters = null;
					lock (m_filters.SyncRoot)
					{
						allFilters = (ArrayList)m_filters.Clone();
					}
					
					// Copy data
					writer.CombineFilters	= CombineFilters;
					writer.InvertCheck		= InvertCheck;

					// Save all active filters
					writer.ProcessHeader();
					writer.WriteFilters(allFilters);
					writer.ProcessEpilogue();
				}
				catch(Exception e)
				{
					Log.Error("Saving filters", e);
				}
			}
		}

		/// <summary>
		/// Loads the filters.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="allFilters">Collection with all loaded filter instances.</param>
		public static void LoadFilters(string path, ICollection allFilters)
		{
			// Deactivate all filters
			foreach (ILogFilter filter in allFilters)
			{
				RemoveFilter(filter);
			}
		
			// Load filters
			List<ILogFilter> loadedFilters = null;
			using (FilterReader reader = new FilterReader(path))
			{
				try
				{
					// Read file
					reader.ProcessHeader();
					loadedFilters = reader.ReadFilters(allFilters);
					reader.ProcessEpilogue();

					// Copy data
					CombineFilters	= reader.CombineFilters;
					InvertCheck		= reader.InvertCheck;
				}
				catch(Exception e)
				{
					Log.Error("Loading filters", e);
				}
			}
			
			// Activate loaded filters
			foreach (ILogFilter filter in loadedFilters)
			{
				AddFilter(filter);
			}
		}

		#endregion
	}
}
