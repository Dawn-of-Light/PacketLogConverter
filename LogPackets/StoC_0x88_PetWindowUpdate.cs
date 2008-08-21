using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x88, -1, ePacketDirection.ServerToClient, "Pet window update ")]
	public class StoC_0x88_PetWindowUpdate : Packet, IObjectIdPacket
	{
		protected ushort petId;
		protected byte windowAction;
		protected byte aggroLevel;
		protected byte walkState;
#if !SKIPUNUSEDINPACKET
		protected ushort unused1;
		protected byte unused2;
#endif
		protected ushort[] petEffects;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { petId }; }
		}

		#region public access properties

		public ushort PetId { get { return petId; } }
		public byte WindowAction { get { return windowAction; } }
		public byte AggroLevel { get { return aggroLevel; } }
		public byte WalkState { get { return walkState; } }
		public ushort[] PetEffects { get { return petEffects; } }
#if !SKIPUNUSEDINPACKET
		public ushort Unused1 { get { return unused1; } }
		public byte Unused2 { get { return unused2; } }
#endif

		#endregion

		public enum eWindowAction: byte
		{
			close = 0,
			update = 1,
			open = 2,
		}

		public enum eAggroMode: byte
		{
			Aggressive = 1,
			Defencive = 2,
			Passive = 3,
		}

		public enum eWalkMode: byte
		{
			Follow = 1,
			Stay = 2,
			Goto = 3,
			Here = 4,
		}

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("pet:0x{0:X4}", petId);
			text.Write(" windowAction:{0}({1,-6})", windowAction, (eWindowAction)windowAction);
			text.Write(" aggro:{0}({1,-10})", aggroLevel, (eAggroMode)aggroLevel);
			text.Write(" walk:{0}({1})", walkState, (eWalkMode)walkState);
#if !SKIPUNUSEDINPACKET
			if (flagsDescription)
				text.Write(" unk1:0x{0:X4} unk2:0x{1:X2}", unused1, unused2);
#endif
			if (petEffects.Length > 0)
			{
				text.Write(")");
				text.Write("\n\tpet effects:(");
				for (int i = 0; i < petEffects.Length; i++)
				{
					if (i > 0)
						text.Write(',');
					text.Write("0x{0:X4}", petEffects[i]);
				}
			}

		}

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
			aggroLevel = ReadByte(); // 1-aggressive, 2-defensive, 3-passive
			walkState = ReadByte(); // 1-follow, 2-stay, 3-goto, 4-here
#if !SKIPUNUSEDINPACKET
			unused2 = ReadByte();
#else
			Skip(1);
#endif
			ArrayList effects = new ArrayList(8);

			int stop = ReadByte();
			while (Position < Length)
			{
				Seek(-1, SeekOrigin.Current);
				effects.Add(ReadShort());
				stop = ReadByte();

//				petEffects.Add(effect);
			}

			petEffects = (ushort[])effects.ToArray(typeof (ushort));
		}

//		public class PetEffect
//		{
//			public int id;
//		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x88_PetWindowUpdate(int capacity) : base(capacity)
		{
		}
	}
}