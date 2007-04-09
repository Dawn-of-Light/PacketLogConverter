using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x7A, -1, ePacketDirection.ClientToServer, "Object interact request")]
	public class CtoS_0x7A_ObjectInteractRequest : Packet, IObjectIdPacket
	{
		protected uint playerX;
		protected uint playerY;
		protected ushort sessionId;
		protected ushort objectOid;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { objectOid }; }
		}

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public ushort SessionId { get { return sessionId; } }
		public ushort MobOid { get { return objectOid; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("objectOid:0x{0:X4} sessionId:0x{1:X4} playerX:{2,-6} playerY:{3,-6}", objectOid, sessionId, playerX, playerY);

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			playerX = ReadInt();
			playerY = ReadInt();
			sessionId = ReadShort();
			objectOid = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x7A_ObjectInteractRequest(int capacity) : base(capacity)
		{
		}
	}
}