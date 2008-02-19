using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x02, 182, ePacketDirection.ServerToClient, "Inventory update v182")]
	public class StoC_0x02_InventoryUpdate_182 : StoC_0x02_InventoryUpdate
	{
		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("slots:{0} bits:0x{1:X2} visibleSlots:0x{2:X2} preAction:0x{3:X2}", SlotsCount, Bits, VisibleSlots, PreAction);
			if (flagsDescription)
				text.Write("({0}{1})", (ePreActionType)PreAction, (PreAction == (byte)ePreActionType.InitHouseVault) ? VisibleSlots.ToString() : "");

			for (int i = 0; i < m_slotsCount; i++)
			{
				Item item = Items[i];

				text.Write("\n\tslot:{0,-3} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} flag:0x{15:X2} extension:{16} \"{17}\"",
					item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.flag, item.extension, item.name);
				if (flagsDescription && item.name != null && item.name != "")
					text.Write(" ({0})", (eObjectType)item.objectType);
				if ((item.flag & 0x08) == 0x08)
					text.Write("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
					item.effectIcon, item.effectName);
				if ((item.flag & 0x10) == 0x10)
					text.Write("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
					item.effectIcon2, item.effectName2);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			m_slotsCount = ReadByte();
			m_bits = ReadByte();
			m_visibleSlots = ReadByte();
			m_preAction = ReadByte();

			m_items = new Item[m_slotsCount];

			for (int i = 0; i < m_slotsCount; i++)
			{
				Item item = new Item();

				item.slot = ReadByte();
				if (Position < Length)
				{
					item.level = ReadByte();

					item.value1 = ReadByte();
					item.value2 = ReadByte();

					item.hand = ReadByte();
					byte temp = ReadByte(); //WriteByte((byte) ((item.Type_Damage*64) + item.Object_Type));
					item.damageType = (byte)(temp >> 6);
					item.objectType = (byte)(temp & 0x3F);
					item.weight = ReadShort();
					item.condition = ReadByte();
					item.durability = ReadByte();
					item.quality = ReadByte();
					item.bonus = ReadByte();
					item.model = ReadShort();
					item.extension = ReadByte();
					item.color = ReadShort();
					item.flag = ReadByte();
					if ((item.flag & 0x08) == 0x08)
					{
						item.effectIcon = ReadShort();
						item.effectName = ReadPascalString();
					}
					if ((item.flag & 0x10) == 0x10)
					{
						item.effectIcon2 = ReadShort();
						item.effectName2 = ReadPascalString();
					}
					item.effect = ReadByte();
					item.name = ReadPascalString();
					m_items[i] = item;
				}
				else
				{
					m_items[i] = item;
					for (int j = (i + 1); j < m_slotsCount; j++)//Test
					{
						Item itm = new Item();
						itm.slot = (byte)(item.slot + j);
						m_items[j] = itm;
						i = j;
					}
				}
			}
		}

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			// Partialy detectable only from serverside (this subversion not detectable from client)
			if (!log.IgnoreVersionChanges && log.Version >= 189 && log.Version < 189.1f)
			{
				if (Length == 6)// detectable only if server send empty inventory update
				{
					log.Version = 189.1f;
					log.SubversionReinit = true;
//					log.IgnoreVersionChanges = true;
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x02_InventoryUpdate_182(int capacity) : base(capacity)
		{
		}
	}
}