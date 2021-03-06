using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC4, 181, ePacketDirection.ServerToClient, "Custom text window v181")]
	public class StoC_0xC4_CustomTextWindow_181 : StoC_0xC4_CustomTextWindow_175
	{
		protected byte isGlued;

		#region public access properties

		public byte IsGlued { get { return isGlued; } }

		#endregion

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

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flagAnswers = ReadByte(); // new in 1.75
			isGlued = ReadByte(); // new in 1.81
			caption = ReadPascalString();
			InitLines();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC4_CustomTextWindow_181(int capacity) : base(capacity)
		{
		}
	}
}