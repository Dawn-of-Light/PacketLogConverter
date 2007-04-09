using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x01, -1, ePacketDirection.ClientToServer, "House modified")]
	public class CtoS_0x01_HouseModified: Packet, IObjectIdPacket
	{
		protected ushort sessionId;
		protected short[] objects;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { sessionId }; }
		}

		#region public access properties

		public ushort SessionId { get { return sessionId; } }
		public short[] Objects { get { return objects; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4}", sessionId);
			str.Append(" objects:(");
			for (byte i = 0; i < 10 ; i++)
			{
				if (i > 0)
					str.Append(',');
				str.AppendFormat("[{0}]={1}", i, objects[i]);
				if (flagsDescription && objects[i] > 0)
					str.AppendFormat("(page:{0}/slot:{1})", (byte)(objects[i]/30), objects[i]%30);
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