using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEA, 01, ePacketDirection.ServerToClient, "Trade window")]
	public class StoC_0xEA_TradeWindow : Packet
	{
		protected byte[] slots;
		protected ushort unk1;
		protected ushort mithrilPlayer;
		protected ushort platinumPlayer;
		protected ushort goldPlayer;
		protected ushort silverPlayer;
		protected ushort copperPlayer;
		protected ushort unk2;
		protected ushort mithrilPartner;
		protected ushort platinumPartner;
		protected ushort goldPartner;
		protected ushort silverPartner;
		protected ushort copperPartner;
		protected ushort unk3;
		protected byte itemCount;
		protected byte tradeCode;
		protected byte repair;
		protected byte combine;
		protected RecieveItem[] items;
		protected string tradeDescription;

		#region public access properties

		public byte[] Slots { get { return slots; } }
		public ushort Unk1 { get { return unk1; } }
		public ushort MithrilPlayer { get { return mithrilPlayer; } }
		public ushort PlatinumPlayer { get { return platinumPlayer; } }
		public ushort GoldPlayer { get { return goldPlayer; } }
		public ushort SilverPlayer { get { return silverPlayer; } }
		public ushort CopperPlayer { get { return copperPlayer; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort MithrilPartner { get { return mithrilPartner; } }
		public ushort PlatinumPartner { get { return platinumPartner; } }
		public ushort GoldPartner { get { return goldPartner; } }
		public ushort SilverPartner { get { return silverPartner; } }
		public ushort CopperPartner { get { return copperPartner; } }
		public ushort Unk3 { get { return unk3; } }
		public byte ItemCount { get { return itemCount; } }
		public byte TradeCode { get { return tradeCode; } }
		public byte Repair { get { return repair; } }
		public byte Combine { get { return combine; } }
		public RecieveItem[] Items { get { return items; } }
		public string TradeDescription { get { return tradeDescription; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("code:{0} recieveItems:{1} repair:{2} combine:{3} unk1:{4} unk2:{5} unk3:{6} description:\"{7}\"",
				tradeCode, itemCount, repair, combine, unk1, unk2, unk3, tradeDescription);
			str.AppendFormat("\n\tgive money (copper:{0,-2} silver:{1,-2} gold:{2,-3} platinum:{3} mithril:{4,-3})",
				copperPlayer, silverPlayer, goldPlayer, platinumPlayer, mithrilPlayer);
			str.AppendFormat("\n\ttake money (copper:{0,-2} silver:{1,-2} gold:{2,-3} platinum:{3} mithril:{4,-3})",
				copperPartner, silverPartner, goldPartner, platinumPartner, mithrilPartner);

			str.Append("\n\tgive slots:(");
			for (byte i = 0; i < slots.Length ; i++)
			{
				if (i > 0)
					str.Append(',');
				str.AppendFormat("{0,-3}", slots[i]);
			}
			str.Append(")");

			for (int i = 0; i < itemCount; i++)
			{
				WriteItemInfo(i, str);
			}

			return str.ToString();
		}

		protected virtual void WriteItemInfo(int i, StringBuilder str)
		{
			RecieveItem item = items[i];
			str.AppendFormat("\n\tslot:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"",
				item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			ArrayList tmp = new ArrayList(10);
			for (byte i = 0; i < 10; i++)
				tmp.Add(ReadByte());
			slots = (byte[])tmp.ToArray(typeof (byte));
			unk1 = ReadShort();
			mithrilPlayer = ReadShort();
			platinumPlayer = ReadShort();
			goldPlayer = ReadShort();
			silverPlayer = ReadShort();
			copperPlayer = ReadShort();
			unk2 = ReadShort();
			mithrilPartner = ReadShort();
			platinumPartner = ReadShort();
			goldPartner = ReadShort();
			silverPartner = ReadShort();
			copperPartner = ReadShort();
			unk3 = ReadShort();
			itemCount = ReadByte();
			tradeCode = ReadByte();
			repair = ReadByte();
			combine = ReadByte();
			items = new RecieveItem[itemCount];

			for (int i = 0; i < itemCount; i++)
			{
				ReadItem(i);
			}
			if (tradeCode != 3 && tradeCode != 0) // code = 3 is Close tradewindow
				tradeDescription = ReadPascalString();
		}

		protected virtual void ReadItem(int index)
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
			item.color = ReadShort();
			item.effect = ReadShort();
			item.name = ReadPascalString();

			items[index] = item;
		}

		public struct RecieveItem
		{
			public byte slot;
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte damageType;
			public byte objectType;
			public ushort weight;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public byte extension; // new in 1.72
			public ushort color;
			public ushort effect;
			public byte flag;
			public string name;
			public ushort effectIcon; // new 1.82
			public string effectName; // new 1.82
			public ushort effectIcon2; // new 1.82
			public string effectName2; // new 1.82
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xEA_TradeWindow(int capacity) : base(capacity)
		{
		}
	}
}