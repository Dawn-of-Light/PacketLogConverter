using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD4, -1, ePacketDirection.ClientToServer, "World init request")]
	public class CtoS_0xD4_WorldInitRequest : Packet
	{
		protected uint unk1;
		protected uint unk2;
		protected uint unk3;
		protected ushort model;
		protected byte slot;//alb:0-9, mid:10-19, hib:20-29
		protected byte unk4;
		protected ushort unk5; // releated with CtoS_0xE8.Unk2 ?
		protected ushort unk6; // releated with CtoS_0xE8.Unk1 ?
		protected short regionId;

		#region public access properties

		public short RegionId { get { return regionId; } }
		public ushort Model { get { return model; } }
		public byte Slot { get { return slot; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("unk1:0x{0:X8} unk2:0x{1:X8} regionId:{2,-3} unk3:0x{3:X8} playerModel:0x{4:X4} unk4:0x{5:X2} dBslot:{6,-2} unk5:0x{7:X4}{8:X4}", unk1, unk2, regionId, unk3, model, unk4, slot, unk5, unk6);
			if (flagsDescription)
				text.Write("\n\t(model:0x{0:X4} face?:{1} size:{2})", model & 0x7FF, model >> 13, (model >> 11) & 3);

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadInt();
			unk2 = ReadInt();
			regionId = (short)ReadShort();// 0x08
//			regionId = (byte)ReadByte();
//			Skip(1);
			unk3 = ReadInt();
			model = ReadShort();
			unk4 = ReadByte();
			slot = ReadByte();
			unk5 = ReadShortLowEndian();
			unk6 = ReadShortLowEndian();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD4_WorldInitRequest(int capacity) : base(capacity)
		{
		}
	}
}