using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7F, 190.1f, ePacketDirection.ServerToClient, "Update icons v190c")]
	public class StoC_0x7F_UpdateIcons_190c : StoC_0x7F_UpdateIcons
	{
		protected override void WriteEffectInfo(int i, StringBuilder str)
		{
			Effect effect = effects[i];
			str.AppendFormat("\n\ticonIndex:0x{0:X2} iconPrevIndex:0x{1:X2} immunity:{2} icon:0x{3:X4} remainingTime:{4,-4} internalId:{5,-5} negative:{7} name:\"{6}\"",
				effect.iconIndex, effect.unk1, effect.immunity, effect.icon, effect.remainingTime, effect.internalId, effect.name, effect.negative);
		}

		protected override void ReadEffect(int index)
		{
			Effect effect = new Effect();

			effect.iconIndex = ReadByte();
			effect.unk1 = ReadByte();
			effect.immunity = ReadByte();
			effect.icon = ReadShort();
			effect.remainingTime = ReadShort();
			effect.internalId = ReadShort();
			effect.negative = ReadByte();
			effect.name = ReadPascalString();

			effects[index] = effect;
		}

//		public class Effect_173 : Effect
//		{
//			public int immunity;
//		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7F_UpdateIcons_190c(int capacity) : base(capacity)
		{
		}
	}
}