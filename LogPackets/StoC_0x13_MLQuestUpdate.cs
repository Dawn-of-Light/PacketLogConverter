using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x13, -1, ePacketDirection.ServerToClient, "ML Quest Update")]
	public class StoC_0x13_MLQuestUpdate : Packet
	{
		protected byte unk1;
		protected byte unk2;
		protected byte unk3;
		protected byte unk4;
		protected byte level;
		protected string[] descLines;

		#region public access properties

		public byte Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }
		public byte Unk3 { get { return unk3; } }
		public byte Unk4 { get { return unk4; } }
		public byte Level { get { return level; } }
		public string[] DescLines { get { return descLines; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("level:{0,-3} unk1:{1,-3} unk2:{2,-3} unk3:{3,-3} unk4:{4,-3}", level, unk1, unk2, unk3, unk4);

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

			unk1 = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadByte();
			unk4 = ReadByte();
			level = ReadByte();
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