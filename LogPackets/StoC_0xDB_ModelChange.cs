using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDB, -1, ePacketDirection.ServerToClient, "Model change")]
	public class StoC_0xDB_ModelChange : Packet, IOidPacket
	{
		protected ushort oid;
		protected ushort newModel;
		protected ushort unk1;
		protected ushort unk2;

		public int Oid1 { get { return oid; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort NewModel { get { return newModel; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} newModel:0x{1:X4} unk1:0x{2:X4} unk2:0x{3:X4}", oid, newModel, unk1, unk2);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			newModel = ReadShort();
			unk1 = ReadShort();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xDB_ModelChange(int capacity) : base(capacity)
		{
		}
	}
}