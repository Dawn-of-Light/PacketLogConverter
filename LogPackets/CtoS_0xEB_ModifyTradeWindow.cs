using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xEB, -1, ePacketDirection.ClientToServer, "Modify trade window")]
	public class CtoS_0xEB_ModifyTradeWindow : Packet
	{
		protected byte tradeCode;
		protected byte repair;
		protected byte combine;
		protected byte unk1;
		protected byte[] slots;
		protected ushort unk2;
		protected ushort mithrilPlayer;
		protected ushort platinumPlayer;
		protected ushort goldPlayer;
		protected ushort silverPlayer;
		protected ushort copperPlayer;
		protected ushort unk3;

		#region public access properties

		public byte TradeCode { get { return tradeCode; } }
		public byte Repair { get { return repair; } }
		public byte Combine { get { return combine; } }
		public byte Unk1 { get { return unk1; } }
		public byte[] Slots { get { return slots; } }
		public ushort Unk2 { get { return unk2; } }
		public ushort MithrilPlayer { get { return mithrilPlayer; } }
		public ushort PlatinumPlayer { get { return platinumPlayer; } }
		public ushort GoldPlayer { get { return goldPlayer; } }
		public ushort SilverPlayer { get { return silverPlayer; } }
		public ushort CopperPlayer { get { return copperPlayer; } }
		public ushort Unk3 { get { return unk3; } }

		#endregion

		public enum tradeCommand : byte
		{
			close = 0,
			change = 1,
			accept = 2,
		};

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("\n\tcode:{0}({1}) repair:{2} combine:{3} unk1:0x{4:X2} unk2:0x{5:X4} unk3:0x{6:X4}",
				tradeCode, (tradeCommand)tradeCode, repair, combine, unk1, unk2, unk3);
			str.AppendFormat("\n\tmoney (copper:{0,-2} silver:{1,-2} gold:{2,-3} platinum:{3} mithril:{4,-3})",
				copperPlayer, silverPlayer, goldPlayer, platinumPlayer, mithrilPlayer);
			str.Append("\n\tslots:(");
			for (byte i = 0; i < slots.Length ; i++)
			{
				if (i > 0)
					str.Append(',');
				str.AppendFormat("{0,-3}", slots[i]);
			}
			str.Append(")");

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			tradeCode = ReadByte();
			repair = ReadByte();
			combine = ReadByte();
			unk1 = ReadByte();
			ArrayList tmp = new ArrayList(10);
			for (byte i = 0; i < 10; i++)
				tmp.Add(ReadByte());
			slots = (byte[])tmp.ToArray(typeof (byte));
			unk2 = ReadShort();
			mithrilPlayer = ReadShort();
			platinumPlayer = ReadShort();
			goldPlayer = ReadShort();
			silverPlayer = ReadShort();
			copperPlayer = ReadShort();
			unk3 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xEB_ModifyTradeWindow(int capacity) : base(capacity)
		{
		}
	}
}