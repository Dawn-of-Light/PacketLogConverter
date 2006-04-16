using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC4, -1, ePacketDirection.ServerToClient, "Custom text window")]
	public class StoC_0xC4_CustomTextWindow : Packet
	{
		protected string caption;
		protected LineEntry[] lines;

		#region public access properties

		public string Caption { get { return caption; } }
		public LineEntry[] Lines { get { return lines; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("\n\tcaption: \"{0}\"", caption);

			for (int i = 0; i < lines.Length; i++)
			{
				LineEntry line = lines[i];
				str.AppendFormat("\n\t{0,2}: \"{1}\"", line.number, line.text);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			caption = ReadPascalString();
			InitLines();
		}

		protected virtual void InitLines()
		{
			ArrayList temp = new ArrayList();
			byte lineNum = ReadByte();
			while (lineNum != 0)
			{
				LineEntry line = new LineEntry();

				line.number = lineNum;
				line.text = ReadPascalString();
				temp.Add(line);

				lineNum = ReadByte();
			}
			lines = (LineEntry[])temp.ToArray(typeof (LineEntry));
		}

		public struct LineEntry
		{
			public byte number;
			public string text;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC4_CustomTextWindow(int capacity) : base(capacity)
		{
		}
	}
}