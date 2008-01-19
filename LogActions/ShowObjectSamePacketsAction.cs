using System;
using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows info about object
	/// </summary>
	[LogAction("Show object same packets", Priority=995)]
	public class ShowObjectSamePacketsAction : ILogAction
	{

		#region ILogAction Members

		/// <summary>
		/// Determines whether the action is enabled.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns>
		/// 	<c>true</c> if the action is enabled; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			Packet pak = context.LogManager.GetPacket(selectedPacket);
			if (pak is IObjectIdPacket)
				return true;
			return false;
		}

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.Logs[selectedPacket.LogIndex];
			int selectedIndex = selectedPacket.PacketIndex;

			Packet originalPak = log[selectedIndex];
			ushort[] objectIds = null;
			if (originalPak is IObjectIdPacket)
				objectIds = (originalPak as IObjectIdPacket).ObjectIds;
			StringBuilder str = new StringBuilder();
			if ((objectIds == null || objectIds.Length == 0))
			{
				str.AppendFormat("packet not have any ID\n");
				return false;
			}
			TimeSpan zeroTimeSpan = new TimeSpan(0);
			for (int i = 0; i < log.Count; i++)
			{
				Packet pak = log[i];
				if (pak.Code == originalPak.Code && pak.Direction == originalPak.Direction && pak.Protocol == originalPak.Protocol)
				{
					if (pak is IObjectIdPacket && pak.Length > 0 && (pak as IObjectIdPacket).ObjectIds[0] == objectIds[0])
					{
						str.Append(pak.ToHumanReadableString(zeroTimeSpan, true));
						str.Append('\n');
					}
				}
			}

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Show object same packets (right click to close)";
			infoWindow.Width = 620;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion
	}
}
