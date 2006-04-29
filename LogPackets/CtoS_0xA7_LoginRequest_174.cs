using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA7, 174, ePacketDirection.ClientToServer, "Login request v174")]
	public class CtoS_0xA7_LoginRequest_174 : CtoS_0xA7_LoginRequest
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
			Aunk1 = ReadIntLowEndian();
			Aunk2 = ReadIntLowEndian();
			Aunk3 = ReadIntLowEndian();
			Aunk4 = ReadIntLowEndian();
			unk3 = ReadIntLowEndian();
//			Skip(28); cryptKeyRequests; Skip(22);
			edi = ReadIntLowEndian();
			AunkB = ReadIntLowEndian();
			cryptKeyRequests = (byte)(AunkB>>24);
			ArrayList arr = new ArrayList(4);
			for(byte i = 0; i < 3; i++)
				arr.Add(ReadIntLowEndian());
			AunkI = (uint[])arr.ToArray(typeof (uint));
			unkB1 = ReadByte();
			unkS1 = ReadShort();
			clientAccountName = ReadString(20);
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xA7_LoginRequest_174(int capacity) : base(capacity)
		{
		}
	}
}