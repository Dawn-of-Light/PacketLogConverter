using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC4, 181, ePacketDirection.ServerToClient, "Custom text window v181")]
	public class StoC_0xC4_CustomTextWindow_181 : StoC_0xC4_CustomTextWindow_175
	{
		protected byte unk1;

		#region public access properties

		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("\n\tisTitle:{0} caption: \"{1}\" unk1_181:0x{2:X2}", flagAnswers, caption, unk1);

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

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			flagAnswers = ReadByte(); // new in 1.75
			unk1 = ReadByte(); // new in 1.81
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