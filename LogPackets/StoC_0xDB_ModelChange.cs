using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDB, -1, ePacketDirection.ServerToClient, "Model change")]
	public class StoC_0xDB_ModelChange : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected ushort newModel;
		protected byte size;
		protected byte torch; // npc torchMode ?
		protected ushort unk1; // unused

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort NewModel { get { return newModel; } }
		public byte Size { get { return size; } }
		public byte Torch { get { return torch ; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("oid:0x{0:X4} newModel:0x{1:X4} size:{2,-2}({4,-3}) torch:0x{3:X2}", oid, newModel, size, torch, size * 2);
			if (flagsDescription)
				text.Write(" unk1:0x{0:X4}", unk1);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();     // 0x00
			newModel = ReadShort();// 0x02
			size = ReadByte();     // 0x04
			torch = ReadByte();     // 0x05
			unk1 = ReadShort();    // 0x06
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