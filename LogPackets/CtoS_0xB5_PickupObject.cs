using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB5, -1, ePacketDirection.ClientToServer, "Pick up object")]
	public class CtoS_0xB5_PickupObject: Packet, IObjectIdPacket
	{
		protected uint playerX;
		protected uint playerY;
		protected ushort sessionId;
		protected ushort unk1;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { sessionId }; }
		}

		#region public access properties

		public uint PlayerX { get { return playerX; } }
		public uint PlayerY { get { return playerY; } }
		public ushort SessionId { get { return sessionId; } }
		public ushort Unk1 { get { return unk1; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("sessionId:0x{0:X4} playerX:{1,-6} playerY:{2,-6} unk1:{3:X4}",
					sessionId, playerX, playerY, unk1);

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
			unk1 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xB5_PickupObject(int capacity) : base(capacity)
		{
		}
	}
}