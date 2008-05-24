using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xC9, -1, ePacketDirection.ServerToClient, "GS_F_PLAY_SOUND?")]
	public class StoC_0xC9_PlaySound : Packet
	{
		protected ushort soundId;//from sounds\cc_sounds.csv
		protected ushort zoneId;
		protected ushort x;
		protected ushort y;
		protected ushort z;
		protected ushort radius;

		public enum eSoundType: ushort
		{
			Relic_PickUp = 15,
			Relic_PutDown = 17,
			Relic_Lose = 48,
			Relic_PulseStrength_Low = 24,
			Relic_PulseStrength_Medium = 33,
			Relic_PulseStrength_High = 43,
			Relic_Hunger = 44,
			ObeliskTeleporter_Idle = 45,
			ObeliskTeleporter_Teleport = 46,
		}

		#region public access properties

		public ushort SoundId { get { return soundId; } }
		public ushort ZoneId { get { return zoneId; } }
		public ushort X { get { return x; } }
		public ushort Y { get { return y; } }
		public ushort Z { get { return z; } }
		public ushort Radius { get { return radius; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("soundId:0x{0:X4} zoneId:{1,-3} x:{2,-5} y:{3,-5} z:{4,-5} radius:{5}",
				soundId, zoneId, x, y, z, radius);
			if (flagsDescription && Enum.IsDefined(typeof(eSoundType), soundId))
				text.Write(" ({0})", (eSoundType)soundId);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			soundId = ReadShort();
			zoneId = ReadShort();
			x = ReadShort();
			y = ReadShort();
			z = ReadShort();
			radius = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xC9_PlaySound(int capacity) : base(capacity)
		{
		}
	}
}