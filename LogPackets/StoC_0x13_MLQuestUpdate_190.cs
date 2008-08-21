using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x13, 190, ePacketDirection.ServerToClient, "ML Quest Update 190")]
	public class StoC_0x13_MLQuestUpdate_190 : StoC_0x13_MLQuestUpdate
	{
		protected ushort unk2;
		protected ushort unk3;

		#region public access properties

		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("requiredLevel:{0,-3} exp1:{1,-3} exp2:{2,-3} level:{3,-3} unk1:{4,-3} unk2:0x:{5:X4} unk3:0x{6:X4}",
				requiredLevel, exp1, exp2, level, unk1, unk2, unk3);

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

			exp1 = ReadByte();   // 0x00
			exp2 = ReadByte();   // 0x01
			level = ReadByte();  // 0x02
			unk1 = ReadByte();   // 0x03
			unk2 = ReadShortLowEndian(); // 0x04
			unk3 = ReadShortLowEndian(); // 0x06
			requiredLevel = ReadByte();  // 0x08
			string desc = ReadPascalString(); // 0x09
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
		public StoC_0x13_MLQuestUpdate_190(int capacity) : base(capacity)
		{
		}
	}
}