using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7F, -1, ePacketDirection.ServerToClient, "Update icons")]
	public class StoC_0x7F_UpdateIcons : Packet
	{
		protected byte effectsCount;
		protected byte unk1;
		protected byte unk2;
		protected byte unk3;
		protected Effect[] effects;

		#region public access properties

		public byte EffectsCount { get { return effectsCount; } }
		public byte Unk1 { get { return unk1; } }
		public byte Unk2 { get { return unk2; } }
		public byte Unk3 { get { return unk3; } }
		public Effect[] Effects { get { return effects; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("count:{0,-2} unk1:{1} unk2:{2} unk3:{3}", effectsCount, unk1, unk2, unk3);
			for (int i = 0; i < effectsCount; i++)
			{
				WriteEffectInfo(i, str);
			}

			return str.ToString();
		}

		protected virtual void WriteEffectInfo(int i, StringBuilder str)
		{
			Effect effect = effects[i];
			str.AppendFormat("\n\ticonIndex:{0,-2} unk1:0x{1:X2} icon:0x{2:X4} remainingTime:{3,-4} internalId:{4,-5} name:\"{5}\"",
			                 effect.iconIndex, effect.unk1, effect.icon, effect.remainingTime, effect.internalId, effect.name);
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

			effects = new Effect[effectsCount];

			for (int i = 0; i < effectsCount; i++)
			{
				ReadEffect(i);
			}
		}

		protected virtual void ReadEffect(int index)
		{
			Effect effect = new Effect();

			effect.iconIndex = ReadByte();
			effect.unk1 = ReadByte();
			effect.icon = ReadShort();
			effect.remainingTime = ReadShort();
			effect.internalId = ReadShort();
			effect.name = ReadPascalString();

			effects[index] = effect;
		}

		public struct Effect
		{
			public byte iconIndex;
			public byte unk1;
			public ushort icon;
			public ushort remainingTime;
			public ushort internalId;
			public string name;
			public byte immunity; // new in 1.73
			public byte negative; // new in 1.90c
			public byte protectedByCount; // unknow from what version
			public byte[] protectedByIndex; // unknow from what version
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7F_UpdateIcons(int capacity) : base(capacity)
		{
		}
	}
}