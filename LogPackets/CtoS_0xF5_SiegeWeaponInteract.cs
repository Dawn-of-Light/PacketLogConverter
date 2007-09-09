using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xF5, -1, ePacketDirection.ClientToServer, "Siege weapon interact")]
	public class CtoS_0xF5_SiegeWeaponInteract: Packet
	{
		protected ushort unk1;
		protected byte action;
		protected byte ammo;
		protected ushort unk2;
		protected ushort unk3;
		protected ushort unk4;
		protected ushort unk5;
		protected ushort unk6;
		protected ushort unk7;

		#region public access properties

		public ushort Unk1 { get { return unk1; } }
		public byte Action { get { return action; } }
		public byte Ammo { get { return ammo; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort Unk3 { get { return unk3; } }
		public ushort Unk4 { get { return unk4; } }
		public ushort Unk5 { get { return unk5; } }
		public ushort Unk6 { get { return unk6; } }
		public ushort Unk7 { get { return unk7; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			string actionType;
			switch (action)
			{
				case 1:
					actionType = "putAmmo";
					break;
				case 2:
					actionType = "arm    ";
					break;
				case 3:
					actionType = "aim    ";
					break;
				case 4:
					actionType = "fire   ";
					break;
				case 5:
					actionType = "move   ";
					break;
				case 6:
					actionType = "repair ";
					break;
				case 7:
					actionType = "salvage";
					break;
				case 8:
					actionType = "release";
					break;
				case 9:
					actionType = "stop   ";
					break;
				case 10:
					actionType = "swing  ";
					break;
				default:
					actionType = "unknown";
					break;
			}
			str.AppendFormat("unk1:0x{0:X4} action:{1}({2}) ammo?:{3, -2} unk2:0x{4:X4} unk3:0x{5:X4} unk4:0x{6:X4} unk5:0x{7:X4} unk6:0x{8:X4} unk7:0x{9:X4}",
				unk1, action, actionType, ammo, unk2, unk3, unk4, unk5, unk6, unk7);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			unk1 = ReadShort();
			action = ReadByte();
			ammo = ReadByte();
			unk2 = ReadShort();
			unk3 = ReadShort();
			unk4 = ReadShort();
			unk5 = ReadShort();
			unk6 = ReadShort();
			unk7 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xF5_SiegeWeaponInteract(int capacity) : base(capacity)
		{
		}
	}
}