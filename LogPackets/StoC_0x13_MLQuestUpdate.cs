using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x13, -1, ePacketDirection.ServerToClient, "ML Quest Update")]
	public class StoC_0x13_MLQuestUpdate : Packet
	{
		protected byte exp1;
		protected byte exp2;
		protected byte level;
		protected byte unk1;
		protected byte requiredLevel;
		protected string[] descLines;

		#region public access properties

		public byte Exp1 { get { return exp1; } }
		public byte Exp2 { get { return exp2; } }
		public byte Level { get { return level; } }
		public byte Unk1 { get { return unk1; } }
		public byte RequiredLevel { get { return requiredLevel; } }
		public string[] DescLines { get { return descLines; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("requiredLevel:{0,-3} exp1:{1,-3} epx2:{2,-3} level:{3,-3} unk1:{4,-3}", requiredLevel, exp1, exp2, level, unk1);

			for (int i = 0; i < descLines.Length; i++)
			{
				string desc = descLines[i];
				text.Write("\n\tdesc: \"{0}\"", desc);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			ArrayList lines = new ArrayList();
			Position = 0;

			exp1 = ReadByte();
			exp2 = ReadByte();
			level = ReadByte();
			unk1 = ReadByte();
			requiredLevel = ReadByte();
			string desc = ReadPascalString();
			while(desc.Length > 0)
			{
				lines.Add(desc);
				desc = ReadPascalString();
			}
			descLines = (string[])lines.ToArray(typeof (string));
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x13_MLQuestUpdate(int capacity) : base(capacity)
		{
		}
	}
}