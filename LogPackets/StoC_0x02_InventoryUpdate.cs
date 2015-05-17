using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x02, -1, ePacketDirection.ServerToClient, "Inventory update")]
	public class StoC_0x02_InventoryUpdate : Packet
	{
		protected byte m_slotsCount;
		protected byte m_bits;
		protected byte m_visibleSlots;
		protected byte m_preAction;
		protected Item[] m_items;

		#region public access properties

		public byte SlotsCount { get { return m_slotsCount; } }
		public byte Bits { get { return m_bits; } }
		public byte VisibleSlots { get { return m_visibleSlots; } }
		public byte PreAction { get { return m_preAction; } }
		public Item[] Items { get { return m_items; } }

		#endregion

		public enum ePreActionType: byte
		{
			UpdateLastOpened = 0,
			InitPaperdoll = 1,
			InitBackpack = 2,
			InitVaultKeeper = 3,
			InitHouseVault = 4,
			InitOwnConsigmentMerchant = 5, // have SetPrice,Withdrow
			InitConsigmentMerchant = 6,// have Buy
			HorseBags = 7,
			ContinueBackpack = 12,
			ContinueVaultKeeper = 13,
			ContinueHouseVault = 14,
			ContinueConsigmentMerchant = 15,
			ContinueOtherConsigmentMerchant = 16,
		}

		public enum eObjectType: byte
		{
			GenericItem = 0,
			GenericWeapon = 1,

			//Albion weapons
			CrushingWeapon = 2,
			SlashingWeapon = 3,
			ThrustWeapon = 4,
			Fired = 5,
			TwoHandedWeapon = 6,
			PolearmWeapon = 7,
			Staff = 8,
			Longbow = 9,
			Crossbow = 10,
			Flexible = 24,

			//Midgard weapons
			Sword = 11,
			Hammer = 12,
			Axe = 13,
			Spear = 14,
			CompositeBow = 15,
			Thrown = 16,
			LeftAxe = 17,
			HandToHand = 25,

			//Hibernia weapons
			RecurvedBow = 18,
			Blades = 19,
			Blunt = 20,
			Piercing = 21,
			LargeWeapons = 22,
			CelticSpear = 23,
			Scythe = 26,

			//Mauler
			FistWraps = 27,
			MaulerStaff = 28,

			//Armor
			GenericArmor = 31,
			Cloth = 32,
			Leather = 33,
			Studded = 34,
			Chain = 35,
			Plate = 36,
			Reinforced = 37,
			Scale = 38,

			//Misc
			Magical = 41,
			Shield = 42,
			Arrow = 43,
			Bolt = 44,
			Instrument = 45,
			Poison = 46,
			AlchemyTincture = 47,
			SpellcraftGem = 48,

			//housing
			GardenObject = 49,
			HouseWallObject = 50,
			HouseFloorObject = 51,
			HouseCarpetFirst = 52,
			HouseNPC = 53,
			HouseVault = 54,
			HouseInteriorObject = 55, //Lathe, forge, alchemy table
			HouseTentColor = 56,
			HouseExteriorBanner = 57,
			HouseExteriorShield = 58,
			HouseRoofMaterial = 59,
			HouseWallMaterial = 60,
			HouseDoorMaterial = 61,
			HousePorchMaterial = 62,
			HouseWoodMaterial = 63,
			HouseShutterMaterial = 64,
			HouseInteriorBanner = 66,
			HouseInteriorShield = 67,
			HouseBindstone = 68,
			HouseCarpetSecond = 69,
			HouseCarpetThird = 70,
			HouseCarpetFourth = 71,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("slots:{0,-2} bits:0x{1:X2} visibleSlots:0x{2:X2} preAction:0x{3:X2}", SlotsCount, Bits, VisibleSlots, PreAction);
			if (flagsDescription)
				text.Write("({0}{1})", (ePreActionType)PreAction, (PreAction == (byte)ePreActionType.InitHouseVault) ? VisibleSlots.ToString() : "");

			for (int i = 0; i < m_slotsCount; i++)
			{
				Item item = (Item)Items[i];
				text.Write("\n\tslot:{0,-3} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} hand:0x{4:X2} damageType:0x{5:X2} objectType:0x{6:X2} weight:{7,-4} con:{8,-3} dur:{9,-3} qual:{10,-3} bonus:{11,-2} model:0x{12:X4} color:0x{13:X4} effect:0x{14:X2} \"{15}\"",
				                 item.slot, item.level, item.value1, item.value2, item.hand, item.damageType, item.objectType, item.weight, item.condition, item.durability, item.quality, item.bonus, item.model, item.color, item.effect, item.name);
				if (flagsDescription && item.name != null && item.name != "")
				{
					text.Write(" ({0}", (eObjectType)item.objectType);
					if ((eObjectType)item.objectType == eObjectType.GardenObject)
						text.Write(" {0}x{1}", item.value2, item.hand);
					text.Write(')');
				}
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
				item.level = ReadByte();

				item.value1 = ReadByte();
				item.value2 = ReadByte();

				item.hand = ReadByte();
				byte damageAndObjectType = ReadByte(); //WriteByte((byte) ((item.Type_Damage*64) + item.Object_Type));
				item.damageType = (byte)(damageAndObjectType >> 6);
				item.objectType = (byte)(damageAndObjectType & 0x3F);
				item.weight = ReadShort();
				item.condition = ReadByte();
				item.durability = ReadByte();
				item.quality = ReadByte();
				item.bonus = ReadByte();
				item.model = ReadShort();
				item.color = ReadShort();
				item.flag = ReadByte();
				item.effect = ReadByte();
				item.name = ReadPascalString();

				m_items[i] = item;
			}
		}

		public class Item
		{
			public byte slot;
			public byte level;
			public byte value1;
			public byte value2;
			public byte hand;
			public byte damageAndObjectType;
			public byte damageType;
			public byte objectType;
			public ushort weight;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public ushort color;
			public byte effect;
			public byte flag;
			public string name;
			public byte extension; // new in 1.72
			public ushort effectIcon; // new 1.82
			public string effectName; // new 1.82
			public ushort effectIcon2; // new 1.82
			public string effectName2; // new 1.82
			public byte unk1_1112; // new 1.11.2 ?
			public byte unk2_1112; // new 1.11.2 ?
			public ushort unk1_1115; // new 1.11.2 ?
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x02_InventoryUpdate(int capacity) : base(capacity)
		{
		}
	}
}