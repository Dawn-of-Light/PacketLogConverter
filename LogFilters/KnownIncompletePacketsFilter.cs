using System;
using System.IO;
using System.Windows.Forms;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Summary description for IncompletePacketFilter.
	/// </summary>
	[LogFilter("Known but incomplete packets", Shortcut.CtrlK, Priority=500)]
	public class KnownIncompletePacketsFilter : AbstractFilter
	{
		#region ILogFilter Members

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsPacketIgnored(Packet packet)
		{
			if (packet.GetType().Equals(typeof(Packet)))
				return true;
			return packet.PositionAfterInit == packet.Length && packet.Initialized;
		}

		#endregion
	}
}
