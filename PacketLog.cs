using System;
using System.Collections;
using System.Windows.Forms;

namespace PacketLogConverter
{
	/// <summary>
	/// Holds all the log data and all information about it
	/// </summary>
	public class PacketLog : IEnumerable
	{
		private string m_streamName = "";

		public string StreamName
		{
			get { return m_streamName; }
			set { m_streamName = value; }
		}

		private ArrayList m_packets = new ArrayList();

		public Packet this[int index]
		{
			get { return (Packet)m_packets[index]; }
		}

		public void AddPacket(Packet pak)
		{
			if (pak == null)
				throw new ArgumentNullException("pak");
			m_packets.Add(pak);
		}

		public void AddRange(ICollection collection)
		{
			m_packets.AddRange(collection);
		}

		public int Count
		{
			get { return m_packets.Count; }
		}

		private float m_version = -1;

		public float Version
		{
			get { return m_version; }
			set
			{
				if (IgnoreVersionChanges)
					return;
				if (m_version != value)
					m_reinitRequired = true;
				m_version = value;
			}
		}

		private bool m_reinitRequired;

		public bool ReinitRequired
		{
			get { return m_reinitRequired; }
		}

		private int m_unknownPacketsCount;

		public int UnknownPacketsCount
		{
			get { return m_unknownPacketsCount; }
		}

		private bool m_ignoreVersionChanges;

		public bool IgnoreVersionChanges
		{
			get { return m_ignoreVersionChanges; }
			set { m_ignoreVersionChanges = value; }
		}

		/// <summary>
		/// Loads the packet parsers based on current version
		/// </summary>
		public void Init(int maxRepeat, ProgressCallback callback)
		{
			if (maxRepeat < 0)
			{
				Log.Info("Log info keep changing, giving up version auto detection.");
				return;
			}

			m_reinitRequired = false;

			int workTotal = m_packets.Count;
			int workDone = 0;

			for (int i = 0; i < m_packets.Count; i++)
			{
				if (callback != null && (workDone++ & 0xFFF) == 0)
					callback(workDone, workTotal);

				Packet packet = (Packet)m_packets[i];
				if (packet.AllowClassChange)
					packet = PacketManager.ChangePacketClass(packet, Version);
				packet.InitLog(this);
				m_packets[i] = packet;

				if (ReinitRequired)
				{
					Init(maxRepeat-1, callback);
					return; // version changed by the packets, start again...
				}
			}

			m_unknownPacketsCount = 0;
			foreach (Packet packet in m_packets)
			{
//				if (callback != null && (workDone++ & 0xFFF) == 0)
//					callback(workDone, workTotal);

				try
				{
					packet.InitException = null;
					packet.Initialized = false;
					packet.Position = 0;
					packet.Init();
					packet.Initialized = true;
				}
				catch (OutOfMemoryException e)
				{
					packet.InitException = e;
					packet.Initialized = false;
					Log.Info(string.Format("{0}: {1}", e.Message, packet.GetType().ToString()));
				}
				catch (Exception e)
				{
					packet.InitException = e;
					packet.Initialized = false;
				}
				packet.PositionAfterInit = packet.Position;
				if (packet.GetType() == typeof(Packet))
					++m_unknownPacketsCount;
			}
		}

		public int GetPacketIndexByTextIndex(int textIndex)
		{
			int pakIndex = m_packets.Count;
			while(--pakIndex >= 0)
			{
				Packet pak = (Packet)m_packets[pakIndex];
				if (pak.LogTextIndex < 0) continue;
				if (pak.LogTextIndex > textIndex) continue;

				return pakIndex;
			}
			return -1;
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return m_packets.GetEnumerator();
		}

		#endregion

	}
}
