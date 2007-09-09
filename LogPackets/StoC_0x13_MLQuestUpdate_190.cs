using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x13, 190, ePacketDirection.ServerToClient, "ML Quest Update 190")]
	public class StoC_0x13_MLQuestUpdate_190 : StoC_0x13_MLQuestUpdate
	{
		protected ushort unk5;
		protected ushort unk6;

		#region public access properties

		public ushort Unk5 { get { return unk5; } }
		public ushort Unk6 { get { return unk6; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("level:{0,-3} unk1:{1,-3} unk2:{2,-3} unk3:{3,-3} unk4:{4,-3} unk5:0x:{5:X4} unk6:0x{6:X4}",
				level, unk1, unk2, unk3, unk4, unk5, unk6);

			for (int i = 0; i < descLines.Length; i++)
			{
				string desc = descLines[i];
				str.AppendFormat("\n\tdesc: \"{0}\"", desc);
			}

			return str.ToString();
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
			level = ReadByte();
			unk3 = ReadByte();
			unk5 = ReadShortLowEndian();
			unk6 = ReadShortLowEndian();
			unk4 = ReadByte();
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
		public StoC_0x13_MLQuestUpdate_190(int capacity) : base(capacity)
		{
		}
	}
}