using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEA, 182, ePacketDirection.ServerToClient, "Trade window v182")]
	public class StoC_0xEA_TradeWindow_182 : StoC_0xEA_TradeWindow
	{
		protected override void WriteItemInfo(int i, StringBuilder str)
		{
			RecieveItem item = items[i];
			str.AppendFormat("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} extension:{13} color:0x{14:X4} effect:0x{15:X2} flag:0x{16:X2} \"{17}\"",
				item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.extension, item.color, item.effect, item.flag, item.name);
				if ((item.flag & 0x08) == 0x08)
					str.AppendFormat("\n\t\teffectIcon:0x{0:X4}  effectName:\"{1}\"",
					item.effectIcon, item.effectName);
				if ((item.flag & 0x10) == 0x10)
					str.AppendFormat("\n\t\teffectIcon2:0x{0:X4}  effectName2:\"{1}\"",
					item.effectIcon2, item.effectName2);
		}

		protected override void ReadItem(int index)
		{
			RecieveItem item = new RecieveItem();

			item.slot = ReadByte();
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

			items[index] = item;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xEA_TradeWindow_182(int capacity) : base(capacity)
		{
		}
	}
}