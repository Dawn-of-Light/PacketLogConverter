using System.Collections;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x88, 181, ePacketDirection.ServerToClient, "Pet window update 181")]
	public class StoC_0x88_PetWindowUpdate_181 : StoC_0x88_PetWindowUpdate
	{

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			petId = ReadShort();
#if !SKIPUNUSEDINPACKET
			unused1 = ReadShort();
#else
			Skip(2);
#endif
			windowAction = ReadByte(); // 0-close window, 1-update window, 2-create window
			aggroLevel = ReadByte(); //1-aggressive, 2-defensive, 3-passive
			walkState = ReadByte(); //1-follow, 2-stay, 3-goto, 4-here
#if !SKIPUNUSEDINPACKET
			unused1 = ReadByte();
#else
			Skip(1);
#endif
			ArrayList effects = new ArrayList(8);
			int effectsCount = ReadByte();

			for (int i = 0; i < effectsCount; i++)
			{
				effects.Add(ReadShort());
			}
			petEffects = (ushort[])effects.ToArray(typeof (ushort));
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x88_PetWindowUpdate_181(int capacity) : base(capacity)
		{
		}
	}
}