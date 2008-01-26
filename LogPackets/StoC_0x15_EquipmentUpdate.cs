using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x15, -1, ePacketDirection.ServerToClient, "Equipment update")]
	public class StoC_0x15_EquipmentUpdate : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected byte hoodUp;
		protected byte visibleWeaponSlots;
		protected byte count;
		protected Item[] items;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region pulic access properties

		public ushort Oid { get { return oid; } }
		public byte HoodUp { get { return hoodUp; } }
		public byte VisibleWeaponSlots { get { return visibleWeaponSlots; } }
		public byte Count { get { return count; } }
		public Item[] Items { get { return items; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} hoodUp:{1} visibleWeaponSlots:0x{2:X2} count:{3,-2}", oid, hoodUp, visibleWeaponSlots, count);
			if (count > 0)
				text.Write("  items:(");
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
					text.Write(" | ");
				Item item = (Item)items[i];
				text.Write("slot:{0,-2} model:0x{1:X4} color:0x{2:X4} effect:0x{3:X2}", item.slot, item.model, item.color, item.effect);
			}
			if (count > 0)
				text.Write(")");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		/// <returns>True if initialized successfully</returns>
		public override void Init()
		{
			Position = 0;

			oid = ReadShort();
			hoodUp = ReadByte();
			visibleWeaponSlots = ReadByte();
			count = ReadByte();

			items = new Item[count];

			for (int i = 0; i < count; i++)
			{
				Item item = new Item();

				item.slot = ReadByte();

				ushort modelAndBits = ReadShort();
				item.model = (ushort)(modelAndBits & 0x1FFF);

				if ((modelAndBits & 0x8000) != 0)
					item.color = ReadShort();
				else if ((modelAndBits & 0x4000) != 0)
					item.color = ReadByte();

				if ((modelAndBits & 0x2000) != 0)
					item.effect = ReadShort();

				items[i] = item;
			}
		}

		public struct Item
		{
			public byte slot;
			public ushort model;
			public ushort color;
			public ushort effect;
			public byte extension; // new in 1.72
			public bool guildBit_176; // new in 1.76
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x15_EquipmentUpdate(int capacity) : base(capacity)
		{
		}
	}
}