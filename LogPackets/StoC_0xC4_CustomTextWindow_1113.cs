using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC4, 1113, ePacketDirection.ServerToClient, "Custom text window v1113")]
	public class StoC_0xC4_CustomTextWindow_1113 : StoC_0xC4_CustomTextWindow_181
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("\n\tisTitle:{0} caption: \"{1}\" isGlued:0x{2:X2}", flagAnswers, caption, isGlued);

			for (int i = 0; i < lines.Length; i++)
			{
				LineEntry line = (LineEntry)lines[i];
				text.Write("\n\t{0,2}: \"{1}\"", line.number, line.text);
			}
			if (flagAnswers > 0)
			{

				text.Write("\n\tcountAnswers:{0}", countAnswers);
				for (int i = 0; i < countAnswers; i++)
				{
					TitleEntry line = (TitleEntry)titles[i];
					text.Write("\n\t{0,2}: \"{1}\"", line.number, line.text);
				}
			}

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
			lines = (LineEntry[])temp.ToArray(typeof(LineEntry));
			if (flagAnswers > 0)
				titles = (TitleEntry[])title.ToArray(typeof(TitleEntry));
		}
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC4_CustomTextWindow_1113(int capacity) : base(capacity)
		{
		}
	}
}