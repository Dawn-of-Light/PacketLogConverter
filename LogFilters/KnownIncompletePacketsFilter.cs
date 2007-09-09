using System;
using System.IO;
using System.Windows.Forms;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Summary description for IncompletePacketFilter.
	/// </summary>
	[LogFilter("Known but incomplete packets", Shortcut.CtrlK, Priority=500)]
	public class KnownIncompletePacketsFilter : ILogFilter
	{
		private bool m_active;

		#region ILogFilter Members

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns><code>true</code> if filter has changed.</returns>
		public bool ActivateFilter()
		{
			m_active = !m_active;
			if (IsFilterActive)
				FilterManager.AddFilter(this);
			else
				FilterManager.RemoveFilter(this);
			return true;
		}

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPacketIgnored(Packet packet)
		{
			if (packet.GetType().Equals(typeof(Packet)))
				return true;
			return packet.PositionAfterInit == packet.Length && packet.Initialized;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsFilterActive
		{
			get { return m_active; }
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public bool Serialize(MemoryStream data)
		{
			data.WriteByte((byte) (m_active ? 1 : 0));
			return true;
		}

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		public bool Deserialize(MemoryStream data)
		{
			int active = data.ReadByte();
			m_active = 0 != active;
			return true;
		}

		#endregion
	}
}
