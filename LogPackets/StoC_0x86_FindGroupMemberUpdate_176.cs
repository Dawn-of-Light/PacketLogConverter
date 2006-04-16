namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x86, 176, ePacketDirection.ServerToClient, "Find group member window update 176")]
	public class StoC_0x86_FindGroupMemberUpdate_176 : StoC_0x86_FindGroupMemberUpdate
	{

		protected override void ReadPlayerInfo(int index)
		{

			Player player = new Player();
			player.index = ReadByte();
			player.level = ReadByte();
			player.name = ReadPascalString();
			player.className = ReadString(4);
			player.zone = ReadShort();
			player.duration = ReadByte();
			player.objective = ReadByte();
			player.unk1 = ReadByte();
			player.unk2 = ReadByte();
			player.flagGrp = ReadByte();
			player.unk3 = ReadByte();

			players[index] = player;
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x86_FindGroupMemberUpdate_176(int capacity) : base(capacity)
		{
		}
	}
}