using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x88, -1, ePacketDirection.ServerToClient, "Pet window update ")]
	public class StoC_0x88_PetWindowUpdate : Packet, IObjectIdPacket
	{
		protected ushort petId;
		protected int unused1;
		protected byte windowAction;
		protected byte aggroLevel;
		protected byte walkState;
		protected byte unused2;
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
		public int Unused1 { get { return unused1; } }
		public byte WindowAction { get { return windowAction; } }
		public byte AggroLevel { get { return aggroLevel; } }
		public byte WalkState { get { return walkState; } }
		public byte Unused2 { get { return unused2; } }
		public ushort[] PetEffects { get { return petEffects; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{

			text.Write("pet:0x{0:X4} ", petId);
			switch (windowAction)
			{
				case 0:
					text.Write(" (close)");
					break;
				case 1:
					text.Write("(update)");
					break;
				case 2:
					text.Write("  (open)");
					break;
				default:
					text.Write("unk {0:X2}", windowAction);
					break;
			}
			text.Write(" aggro:");
			switch (aggroLevel)
			{
				case 1:
					text.Write("aggr");
					break;
				case 2:
					text.Write("def ");
					break;
				case 3:
					text.Write("pass");
					break;
				default:
					text.Write("? {0:X2}", aggroLevel);
					break;
			}
			text.Write(" walk:");
			switch (walkState)
			{
				case 1:
					text.Write("follow");
					break;
				case 2:
					text.Write("stay  ");
					break;
				case 3:
					text.Write("goto  ");
					break;
				case 4:
					text.Write("here  ");
					break;
				default:
					text.Write("unk {0:X2}", walkState);
					break;
			}

			text.Write("  unused1:0 unused2:1", unused1, unused2);

			if (petEffects.Length > 0)
			{
				text.Write(" pet effects:(");
				for (int i = 0; i < petEffects.Length; i++)
				{
					if (i > 0)
						text.Write(',');
					text.Write("0x{0:X4}", petEffects[i]);
				}
				text.Write(")");
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			petId = ReadShort();
			unused1 = ReadShort();
			windowAction = ReadByte(); //0-released, 1-normal, 2-just charmed? | Roach: 0-close window, 1-update window, 2-create window
			aggroLevel = ReadByte(); //1-aggressive, 2-defensive, 3-passive
			walkState = ReadByte(); //1-follow, 2-stay, 3-goto, 4-here
			unused2 = ReadByte();

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