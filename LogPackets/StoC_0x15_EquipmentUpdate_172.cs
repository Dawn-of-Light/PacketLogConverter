using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x15, 172, ePacketDirection.ServerToClient, "Equipment update v172")]
	public class StoC_0x15_EquipmentUpdate_172 : StoC_0x15_EquipmentUpdate
	{
		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} hoodUp:{1} visibleWeaponSlots:0x{2:X2} count:{3,-2}", oid, hoodUp, visibleWeaponSlots, count);
			if (count > 0)
				str.Append("  items:(");
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
					str.Append(" | ");
				Item item = items[i];
				str.AppendFormat("slot:{0,-2} model:0x{1:X4} {2}:0x{3:X4} effect:0x{4:X2} extension:{5}", item.slot, item.model, item.guildBit_176 ? "Guild176Emblem" : "color", item.color, item.effect, item.extension);
			}
			if (count > 0)
				str.Append(")");

			return str.ToString();
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

				item.guildBit_176 = false;
				item.slot = ReadByte();

				int modelAndBits = ReadShort();
				item.model = (ushort)(modelAndBits & 0x1FFF);

				if (item.slot > 13 || item.slot < 10)
					item.extension = ReadByte();

				if ((modelAndBits & 0x8000) != 0)
					item.color = ReadShort();
				else if ((modelAndBits & 0x4000) != 0)
					item.color = ReadByte();

				if ((modelAndBits & 0x2000) != 0)
					item.effect = ReadShort();

				items[i] = item;
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x15_EquipmentUpdate_172(int capacity) : base(capacity)
		{
		}
	}
}