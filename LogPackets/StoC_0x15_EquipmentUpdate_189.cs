using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x15, 189, ePacketDirection.ServerToClient, "Equipment update v189")]
	public class StoC_0x15_EquipmentUpdate_189 : StoC_0x15_EquipmentUpdate_176
	{
		protected byte unk1;
		protected byte m_helmAndCloakVisibile;

		#region pulic access properties

		public byte Unk1 { get { return unk1; } }
		public byte HelmAndCloakVisibile { get { return m_helmAndCloakVisibile; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} unk1:0x{4:X2} helmAndCloakVisibile:0x{5:X2} hoodUp:{1} visibleWeaponSlots:0x{2:X2} count:{3,-2}", oid, hoodUp, visibleWeaponSlots, count, unk1, m_helmAndCloakVisibile);
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
			visibleWeaponSlots = ReadByte();
			unk1 = ReadByte();
			m_helmAndCloakVisibile = ReadByte();
			hoodUp = ReadByte();
			count = ReadByte();

			items = new Item[count];

			for (int i = 0; i < count; i++)
			{
				Item item = new Item();

				item.guildBit_176 = false;
				byte slot = ReadByte();
				item.slot = (byte)(slot & 0x7F);
				if (item.slot != slot)
					item.guildBit_176 = true;

				int modelAndBits = ReadShort();
				item.model = (ushort)(modelAndBits & 0x1FFF);

				if (item.slot > 13 || item.slot < 10)
					item.extension = ReadByte();

				if ((modelAndBits & 0x8000) != 0)
					item.color = ReadShort();
				else if ((modelAndBits & 0x4000) != 0)
					item.color = ReadByte();

				if ((modelAndBits & 0x2000) != 0)
					item.effect = ReadByte();

				items[i] = item;
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x15_EquipmentUpdate_189(int capacity) : base(capacity)
		{
		}
	}
}