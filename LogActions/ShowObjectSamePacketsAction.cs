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

		public bool Activate(PacketLog log, int selectedIndex)
		{
			ushort[] objectIds = null;
			Packet originalPak = log[selectedIndex];
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
