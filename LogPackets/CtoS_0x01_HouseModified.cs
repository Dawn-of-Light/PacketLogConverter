using System.Collections;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x01, -1, ePacketDirection.ClientToServer, "House modified")]
	public class CtoS_0x01_HouseModified: Packet
	{
		protected ushort sessionId;
		protected short[] objects;

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public short[] Objects { get { return objects; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4}", sessionId);
			str.Append(" objects:(");
			for (byte i = 0; i < 10 ; i++)
			{
				if (i > 0)
					str.Append(',');
				str.AppendFormat("[{0}]={1}", i, objects[i]);
			}
			str.Append(")");
			return str.ToString();
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