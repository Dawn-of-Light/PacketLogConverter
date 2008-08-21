using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PacketLogConverter.Utils;

namespace PacketLogConverter
{
	public delegate void LogManagerUpdate(LogManager logManager);

	/// <summary>
	/// This class manages packet logs.
	/// </summary>
	public sealed class LogManager
	{
		public event LogManagerUpdate OnPacketLogsChanged;

		private IList<PacketLog> m_logs = new List<PacketLog>(0).AsReadOnly();
		private readonly PacketTextLocationManager m_packetTextLocations = new PacketTextLocationManager();

		/// <summary>
		/// Gets the logs.
		/// </summary>
		/// <value>The logs.</value>
		public IList<PacketLog> Logs
		{
			get { return m_logs; }
		}

		/// <summary>
		/// Adds the log.
		/// </summary>
		/// <param name="packetLog">The packet log.</param>
		public void AddLog(PacketLog packetLog)
		{
			// Update logs list
			List<PacketLog> logs = new List<PacketLog>(m_logs);
			logs.Add(packetLog);
			m_logs = logs.AsReadOnly();

			// Fire event
			FirePacketLogsChangedEvent();

			// Add packets change event handler
			packetLog.OnPacketsChanged += packetLog_OnPacketsChanged;
		}

		/// <summary>
		/// Adds the log range.
		/// </summary>
		/// <param name="packetLogs">The packet logs.</param>
		public void AddLogRange(ICollection<PacketLog> packetLogs)
		{
			// Update logs list
			List<PacketLog> logs = new List<PacketLog>(m_logs);
			logs.AddRange(packetLogs);
			m_logs = logs.AsReadOnly();

			// Fire event
			FirePacketLogsChangedEvent();

			// Add packets change event handler
			foreach (PacketLog log in packetLogs)
			{
				log.OnPacketsChanged += packetLog_OnPacketsChanged;
			}
		}

		/// <summary>
		/// Gets the packet log.
		/// </summary>
		/// <param name="logIndex">Index of the log.</param>
		/// <returns>Packet log if it exists, <c>null</c> otherwise.</returns>
		public PacketLog GetPacketLog(int logIndex)
		{
			PacketLog log = null;
			if (0 <= logIndex && m_logs.Count > logIndex)
			{
				log = m_logs[logIndex];
			}
			return log;
		}

		/// <summary>
		/// Fires the packet logs changed event.
		/// </summary>
		private void FirePacketLogsChangedEvent()
		{
			OnPacketsCountChange();

			LogManagerUpdate evnt = OnPacketLogsChanged;
			if (evnt != null)
			{
				evnt(this);
			}
		}

		/// <summary>
		/// Gets the stream names of all opened logs.
		/// </summary>
		/// <value>The stream names.</value>
		public string GetStreamNames()
		{
			StringBuilder ret = new StringBuilder(m_logs.Count * 32);
			foreach (PacketLog log in m_logs)
			{
				if (ret.Length > 0)
				{
					ret.Append("; ");
				}
				ret.Append(log.StreamName);
			}

			return ret.ToString();
		}

		/// <summary>
		/// Initializes all the logs.
		/// </summary>
		/// <param name="depth">The depth.</param>
		/// <param name="callback">The callback.</param>
		public void InitLogs(int depth, ProgressCallback callback)
		{
			foreach (PacketLog log in m_logs)
			{
				// Init only dirty logs
				if (log.IsDirty)
				{
					log.Init(this, depth, callback);
					log.IsDirty = false;
				}
			}
		}

		/// <summary>
		/// Clears the logs.
		/// </summary>
		public void ClearLogs()
		{
			// Remove event handlers from logs
			foreach (PacketLog log in m_logs)
			{
				log.OnPacketsChanged -= packetLog_OnPacketsChanged;
			}

			m_logs = new List<PacketLog>(0).AsReadOnly();

			FirePacketLogsChangedEvent();
		}

		/// <summary>
		/// Clears the log.
		/// </summary>
		/// <param name="packetLogs">The packet logs.</param>
		public void ClearLogRange(ICollection<PacketLog> packetLogs)
		{
			List<PacketLog> logs = new List<PacketLog>(m_logs);
			foreach (PacketLog log in packetLogs)
			{
				log.OnPacketsChanged -= packetLog_OnPacketsChanged;
				logs.Remove(log);
			}
			m_logs = logs.AsReadOnly();
			FirePacketLogsChangedEvent();
		}

		/// <summary>
		/// Clears the log.
		/// </summary>
		/// <param name="packetLog">The packet log.</param>
		public void ClearLog(PacketLog packetLog)
		{
			List<PacketLog> logs = new List<PacketLog>(m_logs);
			packetLog.OnPacketsChanged -= packetLog_OnPacketsChanged;
			logs.Remove(packetLog);
			m_logs = logs.AsReadOnly();

			FirePacketLogsChangedEvent();
		}

		/// <summary>
		/// Counts the packets.
		/// </summary>
		/// <returns>Count of all packets in onpen logs.</returns>
		public int CountPackets()
		{
			int ret = 0;
			foreach (PacketLog log in m_logs)
			{
				ret += log.Count;
			}

			return ret;
		}

		/// <summary>
		/// Counts the unknown packets.
		/// </summary>
		/// <returns>Count of all unknown packets in onpen logs.</returns>
		public int CountUnknownPackets()
		{
			int ret = 0;
			foreach (PacketLog log in m_logs)
			{
				ret += log.UnknownPacketsCount;
			}

			return ret;
		}

		/// <summary>
		/// Gets the packet.
		/// </summary>
		/// <param name="packetLocation">The packet location.</param>
		/// <returns>Instance of found packet, <c>null</c> otherwise.</returns>
		public Packet GetPacket(PacketLocation packetLocation)
		{
			Packet ret = null;
			if (packetLocation.LogIndex >= 0 && m_logs.Count > packetLocation.LogIndex)
			{
				PacketLog log = m_logs[packetLocation.LogIndex];
				if (packetLocation.PacketIndex >= 0 && log.Count > packetLocation.PacketIndex)
				{
					ret = log[packetLocation.PacketIndex];
				}
			}

			return ret;
		}

		#region PacketTextLocation

		/// <summary>
		/// Packets the log_ on packets changed.
		/// </summary>
		/// <param name="logManager">The log manager.</param>
		void packetLog_OnPacketsChanged(PacketLog logManager)
		{
			OnPacketsCountChange();
		}

		/// <summary>
		/// Called when count of packets changes.
		/// </summary>
		private void OnPacketsCountChange()
		{
			int newPacketsCount = CountPackets();
			m_packetTextLocations.Capacity = newPacketsCount;
		}

		/// <summary>
		/// Sets the visible packets count.
		/// </summary>
		/// <param name="newCount">The new count.</param>
		public void SetVisiblePacketsCount(int newCount)
		{
			m_packetTextLocations.VisiblePacketsCount = newCount;
		}

		/// <summary>
		/// Sets the visible packet.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="packetInfo">The packet info.</param>
		public void SetVisiblePacket(int index, PacketInfo packetInfo)
		{
			m_packetTextLocations.SetVisiblePacket(index, packetInfo);
		}

		/// <summary>
		/// Finds the text index by packet.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns></returns>
		public int FindTextIndexByPacket(Packet packet)
		{
			return m_packetTextLocations.FindTextIndexByPacket(packet);
		}
		/// <summary>
		/// Finds the index of the packet by text.
		/// </summary>
		/// <param name="textIndex">Index of the text.</param>
		/// <returns>Found <see cref="PacketInfo"/> or <see cref="PacketInfo.UNKNOWN"/>.</returns>
		public PacketInfo FindPacketInfoByTextIndex(int textIndex)
		{
			return m_packetTextLocations.FindPacketInfoByTextIndex(textIndex);
		}

		#endregion

	}
}
