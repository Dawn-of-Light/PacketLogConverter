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
			unk1 = ReadIntLowEndian();
			unk2 = ReadIntLowEndian();
			aunk1 = ReadIntLowEndian();
			aunk2 = ReadIntLowEndian();
			aunk3 = ReadIntLowEndian();
			aunk4 = ReadIntLowEndian();
			unk3 = ReadIntLowEndian();
//			Skip(32); cryptKeyRequests; Skip(18);
			edi = ReadIntLowEndian();
			AunkB = ReadIntLowEndian();
			cryptKeyRequests = (byte)(AunkB);
			ArrayList arr = new ArrayList(4);
			for(byte i = 0; i < 3; i++)
			{
				arr.Add(ReadIntLowEndian());
			}
			AunkI = (uint[])arr.ToArray(typeof (uint));
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