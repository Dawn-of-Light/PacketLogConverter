using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using PacketLogConverter.Utils;

namespace PacketLogConverter
{
	public delegate void FilterAction(ILogFilter filter);
	public delegate void StatusChange(bool newValue);
	public delegate void LogAction(PacketLog log);
	public delegate void PacketAction(Packet packet);

	/// <summary>
	/// Manages log filters
	/// </summary>
	public static class FilterManager
	{
		private static readonly ArrayList m_filters = new ArrayList();
		private static bool m_combineFilters;
		private static bool m_invertCheck;
		private static bool m_loading;
		private static bool m_IgnoreFilters;

		public static event FilterAction FilterAddedEvent;
		public static event FilterAction FilterRemovedEvent;
		public static event StatusChange CombineFiltersChangedEvent;
		public static event StatusChange InvertCheckChangedEvent;
		public static event StatusChange IgnoreFiltersChangedEvent;

		/// <summary>
		/// Event is raised when log filtering is started.
		/// </summary>
		/// <remarks>
		/// Calling thread is not guaranteed to be UI thread, but all three events are called from the same thread.
		/// </remarks>
		public static event LogAction FilteringStartedEvent;
		/// <summary>
		/// Event is raised when log filtering is stopped.
		/// </summary>
		/// <remarks>
		/// Calling thread is not guaranteed to be UI thread, but all three events are called from the same thread.
		/// </remarks>
		public static event LogAction FilteringStoppedEvent;
		/// <summary>
		/// Event is raised when packet is being processed.
		/// </summary>
		/// <remarks>
		/// Calling thread is not guaranteed to be UI thread, but all three events are called from the same thread.
		/// </remarks>
		public static event PacketAction FilteringPacketEvent;

		/// <summary>
		/// Determines whether the specified packet is ignored.
		/// </summary>
		/// <param name="pak">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if the specified packet is ignored; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsPacketIgnored(Packet pak)
		{
			if (m_IgnoreFilters) // temporary Ignore filters
				return false;

			// Notify all listeners
			RaiseFilteringPacketEvent(pak);

			bool ret = false;

			if (m_loading)
			{
				ret = true;
			}
			else
			{
				if (CombineFilters) // allow if not ignored by at least on filter
				{
					ret = true;
					for (int i = 0; i < m_filters.Count; i++)
					{
						ILogFilter filter = (ILogFilter)m_filters[i];
						if (filter.IsPacketIgnored(pak) == m_invertCheck) // not ignored = allow
						{
							ret = false;
							break;
						}
					}
				}
				else
				{
					ret = false;
					for (int i = 0; i < m_filters.Count; i++)
					{
						ILogFilter filter = (ILogFilter)m_filters[i];
						if (filter.IsPacketIgnored(pak) != m_invertCheck) // not ignored = allow
						{
							ret = true;
							break;
						}
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// Raises the filtering packet event.
		/// </summary>
		/// <param name="pak">The pak.</param>
		private static void RaiseFilteringPacketEvent(Packet pak)
		{
			try
			{
				PacketAction e = FilteringPacketEvent;
				if (e != null)
					e(pak);
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
			}
		}

		/// <summary>
		/// Gets the filters count.
		/// </summary>
		/// <value>The filters count.</value>
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
		/// Gets or sets a value indicating whether to Ignore filter check.
		/// </summary>
		/// <value><c>true</c> if filter check is Ignored; otherwise, <c>false</c>.</value>
		public static bool IgnoreFilters
		{
			get { return m_IgnoreFilters; }
			set
			{
				m_IgnoreFilters = value;
				RaiseStatusChangeEvent(IgnoreFiltersChangedEvent, value);
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

		/// <summary>
		/// Saves the filters.
		/// </summary>
		/// <param name="path">The path.</param>
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
			m_loading = true;

			try
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
						CombineFilters = reader.CombineFilters;
						InvertCheck = reader.InvertCheck;
					}
					catch (Exception e)
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
			finally
			{
				m_loading = false;
			}
		}

		#endregion

		#region Filtering start/stop events

		/// <summary>
		/// Manager is notified that filtering is started.
		/// </summary>
		/// <param name="log">The log which is being filtered.</param>
		public static void LogFilteringStarted(PacketLog log)
		{
			try
			{
				// To be safe
				Thread.MemoryBarrier();

				LogAction e = FilteringStartedEvent;
				if (e != null)
					e(log);
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
			}
		}

		/// <summary>
		/// Manager is notified that filtering is stopped.
		/// </summary>
		/// <param name="log">The log which is being filtered.</param>
		public static void LogFilteringStopped(PacketLog log)
		{
			try
			{
				LogAction e = FilteringStoppedEvent;
				if (e != null)
					e(log);

				// To be safe
				Thread.MemoryBarrier();
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
			}
		}

		#endregion
	}
}
