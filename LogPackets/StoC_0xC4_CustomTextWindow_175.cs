using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC4, 175, ePacketDirection.ServerToClient, "Custom text window v175")]
	public class StoC_0xC4_CustomTextWindow_175 : StoC_0xC4_CustomTextWindow
	{
		protected byte flagAnswers;
		protected byte countAnswers;
		protected TitleEntry[] titles;

		#region public access properties

		public byte FlagAnswers { get { return flagAnswers; } }
		public byte CountAnswers { get { return countAnswers; } }
		public TitleEntry[] Titles { get { return titles; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("\n\tisTitle:{0} caption: \"{1}\"", flagAnswers, caption);

			for (int i = 0; i < lines.Length; i++)
			{
				LineEntry line = (LineEntry)lines[i];
				str.AppendFormat("\n\t{0,2}: \"{1}\"", line.number, line.text);
			}
			if (flagAnswers > 0)
			{

				str.AppendFormat("\n\tcountAnswers:{0}", countAnswers);
				for (int i = 0; i < countAnswers; i++)
				{
					TitleEntry line = (TitleEntry)titles[i];
					str.AppendFormat("\n\t{0,2}: \"{1}\"", line.number, line.text);
				}
			}

			return str.ToString();
		}

		protected override void InitLines()
		{
			ArrayList temp = new ArrayList();
			ArrayList title = new ArrayList();
			byte lineNum = ReadByte();
			while (lineNum != 0)
			{
				if (flagAnswers == 1 && lineNum == 200)
				{
					lineNum = ReadByte();
					countAnswers = ReadByte();
					for (byte i = 0; i < countAnswers; i++)
					{
						TitleEntry line = new TitleEntry();
						line.number = ReadByte();
						line.text = ReadPascalString();
						title.Add(line);
					}
				}
				else
				{
					LineEntry line = new LineEntry();

					line.number = lineNum;
					line.text = ReadPascalString();
					temp.Add(line);
				}
				lineNum = ReadByte();
			}
			lines = (LineEntry[])temp.ToArray(typeof (LineEntry));
			if (flagAnswers > 0)
				titles = (TitleEntry[])title.ToArray(typeof (TitleEntry));
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flagAnswers = ReadByte(); // new in 1.75
			caption = ReadPascalString();
			InitLines();
		}

		public struct TitleEntry
		{
			public byte number;
			public string text;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC4_CustomTextWindow_175(int capacity) : base(capacity)
		{
		}
	}
}