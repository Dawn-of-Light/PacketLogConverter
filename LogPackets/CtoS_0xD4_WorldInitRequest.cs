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
		protected uint unk5;
		protected short regionId;

		#region public access properties

		public short RegionId { get { return regionId; } }
		public byte Slot { get { return slot; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("unk1:0x{0:X8} unk2:0x{1:X8} regionId:{2,-3} unk3:0x{3:X8} playerModel:0x{4:X4} unk4:0x{5:X2} dBslot:{6,-2} unk5:0x{7:X8}", unk1, unk2, regionId, unk3, model, unk4, slot, unk5);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadInt();
			unk2 = ReadInt();
			regionId = (byte)ReadByte();
			Skip(1);
			unk3 = ReadInt();
			model = ReadShort();
			unk4 = ReadByte();
			slot = ReadByte();
			unk5 = ReadInt();
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