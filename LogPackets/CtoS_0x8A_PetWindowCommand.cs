using System.IO;
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

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			Position = 0;


			if (flagsDescription)
				text.Write("aggroState:{0} walkState:{1} command:{2} unk1:{3} ", aggroState, walkState, command, unk1);

			//
			// Write aggro state
			//
			string strAggro = null;
			switch (aggroState)
			{
				case 0:
					break;
				case 1:
					strAggro = "aggressive";
					break;
				case 2:
					strAggro = "defensive";
					break;
				case 3:
					strAggro = "passive";
					break;
				default:
					strAggro = "unk aggro 0x" + aggroState.ToString("X2");
					break;
			}
			if (strAggro != null)
			{
				text.Write(strAggro);
			}

			//
			// Write walk state
			//
			string strWalkState = null;
			if (walkState != 0)
			{
				if (strAggro != null) text.Write(',');
				switch (walkState)
				{
					case 0:
						break;
					case 1:
						strWalkState = "follow";
						break;
					case 2:
						strWalkState = "stay";
						break;
					case 3:
						strWalkState = "goto";
						break;
					case 4:
						strWalkState = "here";
						break;
					default:
						strWalkState = "unk walk 0x" + walkState.ToString("X2");
						break;
				}
				if (strWalkState != null)
				{
					text.Write(strWalkState);
				}
			}

			//
			// Write command
			//
			if (command != 0)
			{
				if (strAggro != null || strWalkState != null) text.Write(',');
				string strCommand = null;
				switch (command)
				{
					case 0:
						break;
					case 1:
						strCommand = "attack";
						break;
					case 2:
						strCommand = "release";
						break;
					default:
						strCommand = "unk command 0x" + command.ToString("2X");
						break;
				}
				if (strCommand != null)
				{
					text.Write(strCommand);
				}
			}

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