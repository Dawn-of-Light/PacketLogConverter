using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA1, -1, ePacketDirection.ServerToClient, "NPC Update")]
	public class StoC_0xA1_NpcUpdate : Packet, IOidPacket
	{
		protected ushort temp;
		protected ushort speed;
		protected ushort heading;
		protected ushort currentZoneX;
		protected ushort targetZoneX;
		protected ushort currentZoneY;
		protected ushort targetZoneY;
		protected ushort currentZoneZ;
		protected ushort targetZoneZ;
		protected ushort npcOID;
		protected ushort targetOID;
		protected byte healthPercent;
		protected byte flags;
		protected byte currentZoneId;
		protected byte targetZoneId;

		public int Oid1 { get { return npcOID; } }
		public int Oid2 { get { return targetOID; } }

		#region public access properties

		public ushort Temp { get { return temp; } }
		public ushort Speed { get { return speed; } }
		public ushort Heading { get { return heading; } }
		public ushort CurrentZoneX { get { return currentZoneX; } }
		public ushort TargetZoneX { get { return targetZoneX; } }
		public ushort CurrentZoneY { get { return currentZoneY; } }
		public ushort TargetZoneY { get { return targetZoneY; } }
		public ushort CurrentZoneZ { get { return currentZoneZ; } }
		public ushort TargetZoneZ { get { return targetZoneZ; } }
		public ushort NpcOid { get { return npcOID; } }
		public ushort TargetOid { get { return targetOID; } }
		public byte HealthPercent { get { return healthPercent; } }
		public byte Flags { get { return flags; } }
		public byte CurrentZoneId { get { return currentZoneId; } }
		public byte TargetZoneId { get { return targetZoneId; } }

		#endregion

		public override string GetPacketDataString()
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} speed:0x{1:X4} heading:0x{2:X4} currentZone({3,-3}): ({4,-6} {5,-6} {6,-5}) walkToZone({7,-3}): ({8,-6} {9,-6} {10,-5}) health:{11,3}% targetOID:0x{12:X4} flags:0x{13:X2}(realm:{14},0x{15:X2})",
			                 npcOID, speed, heading, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, targetZoneId, targetZoneX, targetZoneY, targetZoneZ, healthPercent, targetOID, flags, flags>>6, flags & 0x3F);

			Convert.ToString(44, 2);
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			temp = ReadShort();
			speed = temp;
//			speed = (temp & 0x7FFF); // TODO
//			if ((temp & 0x8000) != 0) speed = -speed;
			heading = ReadShort();
			currentZoneX = ReadShort();
			targetZoneX = ReadShort();
			currentZoneY = ReadShort();
			targetZoneY = ReadShort();
			currentZoneZ = ReadShort();
			targetZoneZ = ReadShort();
			npcOID = ReadShort();
			targetOID = ReadShort();
			healthPercent = ReadByte();
			flags = ReadByte();
			currentZoneId = ReadByte();
			targetZoneId = ReadByte();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0xA1_NpcUpdate(int capacity) : base(capacity)
		{
		}
	}
}