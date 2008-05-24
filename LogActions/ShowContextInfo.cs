using System;
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Show the raw packet data
	/// </summary>
	[LogAction("Show context info", Priority=500)]
	public class ShowContextInfo: ILogAction
	{
		public bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			return true;
		}
		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			StringBuilder str = new StringBuilder();
			foreach(PacketLog log in context.LogManager.Logs)
			{
				str.AppendFormat("StreamName:{0}\n", log.StreamName);
				str.AppendFormat("PacketCounts:{0}\n", log.Count);
			}
			str.AppendFormat("PacketLocation(LogIndex:{0} PacketIndex:{1} GetHashCode:{2})\n", selectedPacket.LogIndex, selectedPacket.PacketIndex, selectedPacket.GetHashCode());
			Packet pak = context.LogManager.GetPacket(selectedPacket);
			str.AppendFormat("ver:{0} code:0x{1:X2} dir:{2} prot:{3}", (pak.Attribute == null ? "unk" : pak.Attribute.Version.ToString()), pak.Code, pak.Direction, pak.Protocol);
			str.Append('\n');
			
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Show Context info (right click to close)";
			infoWindow.Width = 640;
			infoWindow.Height = 300;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}
	}
}
