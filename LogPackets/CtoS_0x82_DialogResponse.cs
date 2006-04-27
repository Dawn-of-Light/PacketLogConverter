using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x82, -1, ePacketDirection.ClientToServer, "Dialog response")]
	public class CtoS_0x82_DialogResponse : Packet, IOidPacket
	{
		protected ushort data1;
		protected ushort data2;
		protected ushort data3;
		protected byte messageType;
		protected byte response;

		// TODO when resopnse type is parsed
		public int Oid1 { get { return int.MinValue; } }
		public int Oid2 { get { return int.MinValue; } }

		#region public access properties

		public ushort Data1 { get { return data1; } }
		public ushort Data2 { get { return data2; } }
		public ushort Data3 { get { return data3; } }
		public byte MessageType { get { return messageType; } }
		public byte Response { get { return response; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("data1:0x{0:X4} data2:0x{1:X4} data3:0x{2:X4} messageType:0x{3:X4} response:{4}",
				data1, data2, data3, messageType, response);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			data1 = ReadShort();
			data2 = ReadShort();
			data3 = ReadShort();
			messageType = ReadByte();
			response = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x82_DialogResponse(int capacity) : base(capacity)
		{
		}
	}
}