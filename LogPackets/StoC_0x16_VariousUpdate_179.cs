using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x16, 179, ePacketDirection.ServerToClient, "Various update v179")]
	public class StoC_0x16_VariousUpdate_179 : StoC_0x16_VariousUpdate_175
	{
		protected override void InitPlayerUpdate()
		{
			subData = new PlayerUpdate_179();
			subData.Init(this);
		}

		public class PlayerUpdate_179 : PlayerUpdate_175
		{
			public byte championLevel;
			public string championTitle;

			public override void Init(StoC_0x16_VariousUpdate pak)
			{
				base.Init(pak);

			// new in 1.79
				championLevel = pak.ReadByte();
				championTitle = pak.ReadPascalString();
			}

			public override void MakeString(StringBuilder str, bool flagsDescription)
			{
				base.MakeString(str, flagsDescription);
				str.AppendFormat(" championTitle:\"{0}\" championLevel:{1}", championTitle, championLevel);
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x16_VariousUpdate_179(int capacity) : base(capacity)
		{
		}
	}
}