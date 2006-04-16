using System;
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

		public bool ActivateFilter()
		{
			m_active = !m_active;
			if (IsFilterActive)
				FilterManager.AddFilter(this);
			else
				FilterManager.RemoveFilter(this);
			return true;
		}

		public bool IsPacketIgnored(Packet packet)
		{
			if (packet.GetType().Equals(typeof(Packet)))
				return true;
			return packet.PositionAfterInit == packet.Length && packet.Initialized;
		}

		public bool IsFilterActive
		{
			get { return m_active; }
		}

		#endregion
	}
}
