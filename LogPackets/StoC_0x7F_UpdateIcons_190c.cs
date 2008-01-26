using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7F, 190.1f, ePacketDirection.ServerToClient, "Update icons v190c")]
	public class StoC_0x7F_UpdateIcons_190c : StoC_0x7F_UpdateIcons_173
	{
		protected override void WriteEffectInfo(int i, TextWriter text)
		{
			Effect effect = effects[i];
			text.Write("\n\ticonIndex:0x{0:X2} iconPrevIndex:0x{1:X2} immunity:{2} icon:0x{3:X4} remainingTime:{4,-4} internalId:{5,-5} negative:{6}",
				effect.iconIndex, effect.unk1, effect.immunity, effect.icon, effect.remainingTime, effect.internalId, effect.negative);
			if (effect.protectedByCount == 0)
			{
				text.Write(" name:\"{0}\"", effect.name);
			}
			else
			{
				text.Write(" countProtectedBy:{0} ListProtectedBy:", effect.protectedByCount);
				for (int j = 0; j < effect.protectedByCount; j++)
					text.Write(" {0}", effect.protectedByIndex[j]);
			}
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
			if (effect.immunity == 2)
			{
				effect.protectedByCount = ReadByte();
				effect.protectedByIndex = new byte[effect.protectedByCount];
				for (int i = 0; i < effect.protectedByCount; i++)
					effect.protectedByIndex[i] = ReadByte();
			}
			else
				effect.name = ReadPascalString();

			effects[index] = effect;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x7F_UpdateIcons_190c(int capacity) : base(capacity)
		{
		}
	}
}