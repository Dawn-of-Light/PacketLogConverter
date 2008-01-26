using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xDC, 171, ePacketDirection.ClientToServer, "System message filter v171")]
	public class CtoS_0xDC_MessageFilter_171: CtoS_0xDC_MessageFilter
	{
		protected byte spellEffects;
		protected byte unk1;
		protected ushort unk2;

		#region public access properties

		public byte SpellEffects{ get { return spellEffects; } }
		public byte Unk1 { get { return unk1; } }
		public ushort Unk2 { get { return unk2; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);

			string effects;
			switch (spellEffects)
			{
				case 0:
					effects = "ALL";
					break;
				case 1:
					effects = "SELF";
					break;
				case 2:
					effects = "NONE";
					break;
				case 3:
					effects = "GROUP";
					break;
				default:
					effects = "UNKNOWN";
					break;
			}
			text.Write("\n\tspellEffects:0x{0:X2}({1}) unk1:0x{2:X2} unk2:0x{3:X4}", spellEffects, effects, unk1, unk2);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			base.Init();
			spellEffects = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xDC_MessageFilter_171(int capacity) : base(capacity)
		{
		}
	}
}