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
			private byte unk7_175;
			private string newTitle;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				base.Init(pak);

			// new in 1.75
				unk7_175 = pak.ReadByte();
				newTitle = pak.ReadPascalString();
			}

			public override void MakeString(StringBuilder str)
			{
				base.MakeString(str);
				str.AppendFormat("\n\tnew in 1.75: newTitle:\"{0}\" unk7_175:{1}", newTitle, unk7_175);
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