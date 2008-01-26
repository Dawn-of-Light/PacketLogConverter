using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x01, -1, ePacketDirection.ClientToServer, "House modified")]
	public class CtoS_0x01_HouseModified: Packet, ISessionIdPacket
	{
		protected ushort sessionId;
		protected short[] objects;


		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public short[] Objects { get { return objects; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("sessionId:0x{0:X4}", sessionId);
			text.Write(" objects:(");
			for (byte i = 0; i < 10 ; i++)
			{
				if (i > 0)
					text.Write(',');
				text.Write("[{0}]={1}", i, objects[i]);
				if (flagsDescription && objects[i] > 0)
					text.Write("(page:{0}/slot:{1})", (byte)(objects[i] / 30), objects[i] % 30);
			}
			text.Write(")");
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			sessionId = ReadShort();
			ArrayList slots = new ArrayList(10);
			for (byte i=0; i < 10; i++)
					slots.Add((short)ReadShort());
			objects = (short[])slots.ToArray(typeof(short));
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x01_HouseModified(int capacity) : base(capacity)
		{
		}
	}
}