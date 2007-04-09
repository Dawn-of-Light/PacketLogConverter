using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xE4, -1, ePacketDirection.ClientToServer, "Ship hookpoint interact")]
	public class CtoS_0xE4_ShipHookpointInteract: Packet, IObjectIdPacket
	{
		protected ushort unk1;
		protected ushort objectOid;
		protected ushort unk2;
		protected byte slot;
		protected byte flag;
		protected byte currency;
		protected byte unk3;
		protected ushort unk4;
		protected byte type; // 00 - buy item from store, 01-choose shippoint, 02-choose shopShipPoint
		protected byte unk5;
		protected ushort unk6;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { objectOid }; }
		}

		#region public access properties

		public ushort ObjectOid { get { return objectOid; } }
		public byte Slot { get { return slot; } }
		public byte Flag { get { return flag; } }
		public byte Currency { get { return currency; } }
		public byte Type { get { return type; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("unk1:0x{0:X4} objectOid:0x{1:X4} unk2:0x{2:X4} slot:{3,-2} flag:{4} currency:{5} unk3:0x{6:X2} unk4:0x{7:X4} type:{8} unk5:0x{9:X2} unk6:0x{10:X4}",
				unk1, objectOid, unk2, slot, flag, currency, unk3, unk4, type, unk5, unk6);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadShort();
			objectOid = ReadShort();
			unk2 = ReadShort();
			slot = ReadByte();
			flag = ReadByte();
			currency = ReadByte();
			unk3 = ReadByte();
			unk4 = ReadShort();
			type = ReadByte();
			unk5 = ReadByte();
			unk6 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xE4_ShipHookpointInteract(int capacity) : base(capacity)
		{
		}
	}
}