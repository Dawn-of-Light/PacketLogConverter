using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF5, -1, ePacketDirection.ServerToClient, "Siege weapon interact")]
	public class StoC_0xF5_SiegeWeaponInteract: Packet, IObjectIdPacket
	{
		protected byte siegeMenu;
		protected byte canMove;
		protected ushort unk2;
		protected byte timer; // in sec/10
		protected byte ammoCount;
		protected byte action;
		protected byte unk4;
		protected ushort effect;
		protected ushort unk6;
		protected ushort unk7;
		protected ushort oid;
		protected string name;
		protected Item[] m_items;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public byte SiegeMenu { get { return siegeMenu; } }
		public byte CanMove { get { return canMove ; } }
		public ushort Unk2 { get { return unk2; } }
		public byte Timer { get { return timer; } }
		public byte AmmoCount { get { return ammoCount; } }
		public byte Action { get { return action; } }
		public byte Unk4 { get { return unk4; } }
		public ushort Effect { get { return effect; } }
		public ushort Unk6 { get { return unk6; } }
		public ushort Unk7 { get { return unk7; } }
		public string Name { get { return name; } }
		public Item[] Items { get { return m_items; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string actionType;
			switch (action)
			{
				case 0:
					actionType = "opening";
					if (unk2 == 1)
						actionType = "closing";
					break;
				case 1:
					actionType = "aiming ";
					break;
				case 2:
					actionType = "arming ";
					break;
				case 3:
					actionType = "loading";
					break;
				default:
					actionType = "unknown";
					break;
			}
			str.AppendFormat("menuButtons:0x{0:X2} canMove:0x{1} unk2:0x{2:X4} timer:{3,-3} externalAmmoCount:{4} action:{5}({6}) currentAmmo?:0x{7:X2} effect:0x{8:X4} unk6:0x{9:X4} unk7:0x{10:X4} oid:0x{11:X4} name:\"{12}\"",
				siegeMenu, canMove, unk2, timer , ammoCount, action, actionType, unk4, effect, unk6, unk7, oid, name);

			for (int i = 0; i < ammoCount; i++)
			{
				Item item = (Item)Items[i];
				str.AppendFormat("\n\tindex:{0,-2} level:{1,-2} value1:0x{2:X2} value2:0x{3:X2} unk1:0x{4:X2} objectType:0x{5:X2} unk2:0x{6:X2} count:{7,-2} condition:{8,-3} durability?:{9,-3} quality?:{10,-3} bonus?:{11,-2} model:0x{12:X4} extension?:{13} effect?:0x{14:X4} color?:0x{15:X4} name:\"{16}\"",
	                 item.index, item.level, item.value1, item.value2, item.unk1, item.objectType, item.unk2, item.count, item.condition, item.durability, item.quality, item.bonus, item.model, item.extension, item.unk3, item.unk4, item.name);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			siegeMenu = ReadByte();
			canMove = ReadByte();
			unk2 = ReadShort();
			timer = ReadByte();
			ammoCount = ReadByte();
			action = ReadByte();
			unk4 = ReadByte();
			effect = ReadShort();
			unk6 = ReadShort();
			unk7 = ReadShort();
			oid = ReadShort();
			m_items = new Item[ammoCount];
			for (int i = 0; i < ammoCount; i++)
			{
				Item item = new Item();

				item.index = ReadByte();
				item.level = ReadByte();
				item.value1 = ReadByte();
				item.value2 = ReadByte();
				item.unk1 = ReadByte();
				item.objectType = ReadByte();
				item.unk2 = ReadByte();
				item.count = ReadByte();
				item.condition = ReadByte();
				item.durability = ReadByte();
				item.quality = ReadByte();
				item.bonus = ReadByte();
				item.model = ReadShort();
				item.extension = ReadByte();
				item.unk3 = ReadShort();
				item.unk4 = ReadShort();
				item.name = ReadPascalString();

				m_items[i] = item;
			}

			name = ReadPascalString();
		}

		public struct Item
		{
			public byte index;
			public byte level;
			public byte value1;
			public byte value2;
			public byte unk1;
			public byte objectType;
			public byte unk2;
			public byte count;
			public byte condition;
			public byte durability;
			public byte quality;
			public byte bonus;
			public ushort model;
			public byte extension;
			public ushort unk3;
			public ushort unk4;
			public string name;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xF5_SiegeWeaponInteract(int capacity) : base(capacity)
		{
		}
	}
}
