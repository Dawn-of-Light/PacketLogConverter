using System;
using System.Collections.Generic;
using System.Text;

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
		}

		/// <summary>
		/// Fires the packet logs changed event.
		/// </summary>
		private void FirePacketLogsChangedEvent()
		{
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
		/// Gets the index of the packet index by text.
		/// </summary>
		/// <param name="textIndex">Index of the text.</param>
		public PacketLocation GetPacketIndexByTextIndex(int textIndex)
		{
			PacketLocation ret = PacketLocation.UNKNOWN;

			int currentLogIndex = 0;
			foreach (PacketLog log in m_logs)
			{
				int index = log.GetPacketIndexByTextIndex(textIndex);
				if (index >= 0)
				{
					ret.LogIndex = currentLogIndex;
					ret.PacketIndex = index;
				}
				currentLogIndex++;
			}

			return ret;
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
			m_logs = new List<PacketLog>(0).AsReadOnly();

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
	}
}
