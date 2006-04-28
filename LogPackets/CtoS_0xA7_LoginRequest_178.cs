using System.Collections;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 178, ePacketDirection.ClientToServer, "Login request v178")]
	public class CtoS_0xA7_LoginRequest_178 : CtoS_0xA7_LoginRequest_174
	{

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			clientType = ReadShort();
			clientVersionMajor = ReadByte();
			clientVersionMinor = ReadByte();
			clientVersionBuild = ReadByte();
			clientAccountPassword = ReadString(19);
			ArrayList arr = new ArrayList(8);
			unk1 = ReadInt();
			unk2 = ReadInt();
			for(byte i = 0; i < 4; i++)
			{
				arr.Add(ReadInt());
			}
			Aunk1 = (uint[])arr.ToArray(typeof (uint));
			unk3 = ReadInt();
//			Skip(32); cryptKeyRequests; Skip(18);
			arr.Clear();
			for(byte i = 0; i < 8; i++)
				arr.Add(ReadByte());
			AunkB = (byte[])arr.ToArray(typeof (byte));
			cryptKeyRequests = AunkB[4];
			arr.Clear();
			for(byte i = 0; i < 3; i++)
			{
				arr.Add(ReadInt());
			}
			Aunk2 = (uint[])arr.ToArray(typeof (uint));
			unkB1 = ReadByte();
			unkS1 = ReadShort();
			clientAccountName = ReadString(20);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_178(int capacity) : base(capacity)
		{
		}
	}
}