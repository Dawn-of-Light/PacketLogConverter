using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x15, 189.1f, ePacketDirection.ServerToClient, "Equipment update v189")]// not sure when it changed, but mb after 1.89F
	public class StoC_0x15_EquipmentUpdate_189 : StoC_0x15_EquipmentUpdate_176
	{
		protected byte speed;
		protected byte m_helmAndCloakVisibile;

		#region pulic access properties

		public byte Speed { get { return speed; } }
		public byte HelmAndCloakVisibile { get { return m_helmAndCloakVisibile; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("oid:0x{0:X4} speed:{4,-3} helmAndCloakVisibile:0x{5:X2} hoodUp:{1} visibleWeaponSlots:0x{2:X2} count:{3,-2}", oid, hoodUp, visibleWeaponSlots, count, speed, m_helmAndCloakVisibile);
			if (count > 0)
				text.Write("  items:(");
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
					text.Write(" | ");
				Item item = items[i];
				text.Write("slot:{0,-2} model:0x{1:X4} {2}:0x{3:X4} effect:0x{4:X2} extension:{5}", item.slot, item.model, item.guildBit_176 ? "Guild176Emblem" : "color", item.color, item.effect, item.extension);
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

			oid = ReadShort();                   // 0x00
			visibleWeaponSlots = ReadByte();     // 0x02
			speed = ReadByte();                  // 0x03
			m_helmAndCloakVisibile = ReadByte(); // 0x04
			hoodUp = ReadByte();                 // 0x05
			count = ReadByte();                  // 0x06

			items = new Item[count];

			for (int i = 0; i < count; i++)
			{
				Item item = new Item();

				item.guildBit_176 = false;
				byte slot = ReadByte();          // 0x07
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