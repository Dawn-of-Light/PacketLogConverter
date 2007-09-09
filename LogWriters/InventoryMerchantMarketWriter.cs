using System.Collections;
using System.IO;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("Merchant/Market items writer", "*.txt")]
	public class MerchantMarketItemsWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			Hashtable items = new Hashtable();
			using (StreamWriter s = new StreamWriter(stream))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					Packet pak = log[i];
					if (pak is StoC_0x1F_MarketMerchant)
					{
						StoC_0x1F_MarketMerchant market = pak as StoC_0x1F_MarketMerchant;
						for (int j = 0; j < market.ItemCount; j++)
						{
							StoC_0x1F_MarketMerchant.MerchantItem markeditem = market.Items[j];
							markeditem.name = markeditem.name.Substring(6);
							if (!items.Contains(markeditem.name))
							{
								Item item = new Item();
								item.level = markeditem.level;
								item.value1 = markeditem.value1;
								item.value2 = markeditem.value2;
								item.hand = markeditem.hand;
								item.damageAndObjectType = markeditem.damageAndObjectType;
								item.weight = markeditem.weight;
								item.condition = markeditem.condition;
								item.durability = markeditem.durability;
								item.quality = markeditem.quality;
								item.bonus = markeditem.bonus;
								item.model = markeditem.model;
								item.color = markeditem.color;
								item.effect = markeditem.effect;
								item.name = markeditem.name;
								items.Add(item.name, item);
							}
						}
					}
					else if (pak is StoC_0x17_MerchantWindow)
					{
						StoC_0x17_MerchantWindow merchant = pak as StoC_0x17_MerchantWindow;
						for (int j = 0; j < merchant.ItemCount; j++)
						{
							StoC_0x17_MerchantWindow.MerchantItem merchantitem = merchant.Items[j];
							if (!items.Contains(merchantitem.name))
							{
								Item item = new Item();
								item.level = merchantitem.level;
								item.value1 = merchantitem.value1;
								item.value2 = merchantitem.value2;
								item.hand = merchantitem.hand;
								item.damageAndObjectType = merchantitem.damageAndObjectType;
								item.weight = merchantitem.weight;
//								item.condition = merchantitem.condition;
//								item.durability = merchantitem.durability;
//								item.quality = merchantitem.quality;
//								item.bonus = merchantitem.bonus;
								item.model = merchantitem.model;
//								item.color = merchantitem.color;
//								item.effect = merchantitem.effect;
								item.name = merchantitem.name;
								items.Add(item.name, item);
							}
						}
					}
					else if (pak is StoC_0xC4_CustomTextWindow)
					{
						StoC_0xC4_CustomTextWindow custom = pak as StoC_0xC4_CustomTextWindow;
						string name = custom.Caption;
						if (name[4] == ':')
							name = name.Substring(6);
						if (items.Contains(name))
						{
							Item item = items[name] as Item;
							StringBuilder str = new StringBuilder();
							str.AppendFormat("\tcaption: \"{0}\"", name);
							for (int j = 0; j < custom.Lines.Length; j++)
							{
								StoC_0xC4_CustomTextWindow.LineEntry line = custom.Lines[j];
								str.AppendFormat("\n\t{0,2}: \"{1}\"", line.number, line.text);
							}
							str.Append('\n');
							item.description = str.ToString();
							items[name] = item;
						}
					}
				}
				foreach (DictionaryEntry entry in items)
				{
					Item item = (Item)entry.Value;
					if (item.description == null || item.description == "")
					{
						s.WriteLine(string.Format("level:{0,-2} value1:{1,-3} value2:{2,-3} hand:0x{3:X2} damageType:{4,-1} objectType:0x{5:X2} weigh:{6,-4} condition:{7,3} durability:{8,3} quality:{9,3} bonus:{10,2} model:0x{11:X4} color:0x{12:X4} effect:0x{13:X4} name:\"{14}\"",
							item.level,
							item.value1,
							item.value2,
							item.hand,
							item.damageAndObjectType>> 6,
							item.damageAndObjectType & 0x3F,
							item.weight/10f,
							item.condition,
							item.durability,
							item.quality,
							item.bonus,
							item.model,
							item.color,
							item.effect,
							item.name));
					}
				}
				foreach (DictionaryEntry entry in items)
				{
					Item item = (Item)entry.Value;
					if (item.description != null && item.description != "")
					{
						s.WriteLine(string.Format("level:{0,-2} value1:{1,-3} value2:{2,-3} hand:0x{3:X2} damageType:{4,-1} objectType:0x{5:X2} weigh:{6,-4} condition:{7,3} durability:{8,3} quality:{9,3} bonus:{10,2} model:0x{11:X4} color:0x{12:X4} effect:0x{13:X4} name:\"{14}\"",
							item.level,
							item.value1,
							item.value2,
							item.hand,
							item.damageAndObjectType>> 6,
							item.damageAndObjectType & 0x3F,
							item.weight/10f,
							item.condition,
							item.durability,
							item.quality,
							item.bonus,
							item.model,
							item.color,
							item.effect,
							item.name));
						s.WriteLine(item.description);
					}
				}
			}
		}

		public class Item
		{
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte damageAndObjectType;
			public ushort weight;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public ushort color;
			public ushort effect;
			public string name;
			public string description;
		}
	}
}
