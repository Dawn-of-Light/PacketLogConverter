using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x75, -1, ePacketDirection.ServerToClient, "Set concentration list")]
	public class StoC_0x75_SetConcentrationList : Packet
	{
		protected byte effectsCount;
		protected byte unk1;
		protected byte unk2;
		protected byte unk3;
		protected ConcentrationEffect[] effects;

		#region public access properties

		public byte EffectsCount { get { return effectsCount; } }
		public byte Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }
		public byte Unk3 { get { return unk3; } }
		public ConcentrationEffect[] Effects { get { return effects; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("count:{0,-2} unk1:{1} unk2:{2} unk3:{3}", effectsCount, unk1, unk2, unk3);
			for (int i = 0; i < effectsCount; i++)
			{
				ConcentrationEffect effect = (ConcentrationEffect)effects[i];
				str.AppendFormat("\n\tindex:{0,-2} conc:{1,-2} icon:0x{2:X4} unk1:{3} ownerName:\"{4}\" effectName:\"{5}\"", effect.index, effect.concentration, effect.icon, effect.unk1, effect.ownerName, effect.effectName);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			effectsCount = ReadByte();
			unk1 = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadByte();
			effects = new ConcentrationEffect[effectsCount];
			for (int i = 0; i < effectsCount; i++)
			{
				ConcentrationEffect effect = new ConcentrationEffect();

				effect.index = ReadByte();
				effect.unk1 = ReadByte();
				effect.concentration = ReadByte();
				effect.icon = ReadShort();
				effect.effectName = ReadPascalString();
				effect.ownerName = ReadPascalString();

				effects[i] = effect;
			}
		}

		public struct ConcentrationEffect
		{
			public byte index;
			public byte unk1;
			public byte concentration;
			public ushort icon;
			public string effectName;
			public string ownerName;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x75_SetConcentrationList(int capacity) : base(capacity)
		{
		}
	}
}