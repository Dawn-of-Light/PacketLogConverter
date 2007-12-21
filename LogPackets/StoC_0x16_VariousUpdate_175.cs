using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, 175, ePacketDirection.ServerToClient, "Various update v175")]
	public class StoC_0x16_VariousUpdate_175 : StoC_0x16_VariousUpdate_169
	{
		protected override void InitPlayerUpdate()
		{
			subData = new PlayerUpdate_175();
			subData.Init(this);
		}

		public class PlayerUpdate_175 : PlayerUpdate
		{
			public byte unk6;
			public string newTitle;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				base.Init(pak);

			// new in 1.75
				unk6 = pak.ReadByte();
				newTitle = pak.ReadPascalString();
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				base.MakeString(str, flagsDescription);
				str.AppendFormat("\n\tnew in 1.75: newTitle:\"{0}\" unk6:{1}", newTitle, unk6);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x16_VariousUpdate_175(int capacity) : base(capacity)
		{
		}
	}
}