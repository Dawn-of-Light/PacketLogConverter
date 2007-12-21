using System;
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Show the raw packet data
	/// </summary>
	[LogAction("Show prev same packet", Priority=500)]
	public class ShowPrevSamePacket: ILogAction
	{
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
			bool found = false;
			Packet pak = originalPak;
			for (int i = selectedIndex - 1; i > 0; --i)
			{
				pak = log[i];
				if (pak.Code == originalPak.Code && pak.Direction == originalPak.Direction && pak.Protocol == originalPak.Protocol)
				{
					found = true;
					break;
				}
			}
			if (!found)
				return false;
			StringBuilder str = new StringBuilder();
			str.Append(pak.ToHumanReadableString(pak.Time, true));
			str.Append('\n');
			str.Append('\n');
			str.Append("Raw data, can be copy/pasted to CustomPacket.cs script to send  directly to the client.\n");
			string header = string.Format("ver:{0} code:0x{1:X2} dir:{2} prot:{3}", (pak.Attribute == null ? "unk" : pak.Attribute.Version.ToString()), pak.Code, pak.Direction, pak.Protocol);
			str.Append(header).Append('\n');

			pak.Position = 0;
			while (pak.Position < pak.Length)
			{
				if (((int)pak.Position & 0x0F) == 0)
					str.Append('\n');
				str.Append(pak.ReadByte().ToString("X2")).Append(' ');
			}
			str.Append("\n\n\n");


			str.Append("Another copy with first byte indexes (dec), Len:").Append(pak.Length.ToString("D"));
			ArrayList bytes = new ArrayList();
			pak.Position = 0;
			while (pak.Position < pak.Length)
			{
				if (((int)pak.Position & 0x0F) == 0)
				{
					AppendChars(bytes, str);
					str.AppendFormat("\n{0:D4}: ", pak.Position);
				}
				byte pakByte = pak.ReadByte();
				str.Append(pakByte.ToString("X2")).Append(' ');
				bytes.Add(pakByte);
			}
			AppendChars(bytes, str);


			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Packet raw data " + header + " (right click to close)";
			infoWindow.Width = 640;
			infoWindow.Height = 300;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		private static void AppendChars(ArrayList bytes, StringBuilder str)
		{
			if (bytes.Count > 0)
			{
				for (int i = bytes.Count; i < 16; i++)
					str.Append("   ");
				str.Append("  ");
				foreach(char c in Encoding.ASCII.GetChars((byte[])bytes.ToArray(typeof (byte))))
				{
					if (c > 31) str.Append(c);
					else str.Append('.');
				}
				bytes.Clear();
			}
		}
	}
}
