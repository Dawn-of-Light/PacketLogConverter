using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x8A, -1, ePacketDirection.ClientToServer, "Pet window command")]
	public class CtoS_0x8A_PetWindowCommand : Packet
	{
		protected byte aggroState;
		protected byte walkState;
		protected byte command;
		protected byte unk1;

		#region public access properties

		public byte AggroState { get { return aggroState; } }
		public byte WalkState { get { return walkState; } }
		public byte Command { get { return command; } }
		public byte Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString()
		{
			Position = 0;

			StringBuilder str = new StringBuilder();

			int baseLen = str.Length;
			switch (aggroState)
			{
				case 0:
					break;
				case 1:
					str.Append("aggressive");
					break;
				case 2:
					str.Append("defensive");
					break;
				case 3:
					str.Append("passive");
					break;
				default:
					str.AppendFormat("unk aggro 0x{0:X2}", aggroState);
					break;
			}
			if (walkState != 0)
			{
				if (str.Length != baseLen) str.Append(',');
				switch (walkState)
				{
					case 0:
						break;
					case 1:
						str.Append("follow");
						break;
					case 2:
						str.Append("stay");
						break;
					case 3:
						str.Append("goto");
						break;
					case 4:
						str.Append("here");
						break;
					default:
						str.AppendFormat("unk walk 0x{0:2X}", walkState);
						break;
				}
			}
			if (command != 0)
			{
				if (str.Length != baseLen) str.Append(',');
				switch (command)
				{
					case 0:
						break;
					case 1:
						str.Append("attack");
						break;
					case 2:
						str.Append("release");
						break;
					default:
						str.AppendFormat("unk command 0x{0:2X}", command);
						break;
				}
			}

			return str.ToString();
		}

		public override void Init()
		{
			Position = 0;
			aggroState = ReadByte(); // 1-Aggressive, 2-Deffensive, 3-Passive
			walkState = ReadByte(); // 1-Follow, 2-Stay, 3-GoTarg, 4-Here
			command = ReadByte(); // 1-Attack, 2-Release
			unk1 = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x8A_PetWindowCommand(int capacity) : base(capacity)
		{
		}
	}
}