using System;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xA1, -1, ePacketDirection.ServerToClient, "NPC Update")]
	public class StoC_0xA1_NpcUpdate : Packet, IObjectIdPacket
	{
		protected ushort temp;
		protected short speed;
		protected short speedZ;
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
		protected ushort currentZoneId;
		protected ushort targetZoneId;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { npcOID, targetOID }; }
		}

		#region public access properties

		public ushort Temp { get { return temp; } }
		public short Speed { get { return speed; } }
		public short SpeedZ { get { return speedZ; } }
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
		public ushort CurrentZoneId { get { return currentZoneId; } }
		public ushort TargetZoneId { get { return targetZoneId; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("oid:0x{0:X4} speed:{1,-4} speedZ:{2,-4}({15,-4}) heading:0x{3:X4} currentZone({4,-3}): ({5,-6} {6,-6} {7,-5}) walkToZone({8,-3}): ({9,-6} {10,-6} {11,-5}) health:{12,3}% targetOID:0x{13:X4} flags:0x{14:X2}",
				npcOID, speed, speedZ, heading, currentZoneId, currentZoneX, currentZoneY, currentZoneZ, targetZoneId, targetZoneX, targetZoneY, targetZoneZ, healthPercent, targetOID, flags, speedZ * 4);
			if (flagsDescription)
			{
				str.AppendFormat(" (realm:{0}", (flags >> 6) & 3);
				if ((flags & 0x01) == 0x01)
					str.Append(",-DOR");
				if ((flags & 0x02) == 0x02)
					str.Append(",-NON");
				// 0x04 - zone bit 0x100 , 0x08 - targetZone bit 0x100
				if ((flags & 0x10) == 0x10)
					str.Append(",Underwater");
				if ((flags & 0x20) == 0x20)
					str.Append(",Fly");
				str.Append(')');
/* 				if (targetZoneX != 0 || targetZoneY !=0 || targetZoneZ !=0)
				{
					int diffZ = (targetZoneZ - currentZoneZ);
					int diffX = (currentZoneX - targetZoneX);
	                int diffY = (currentZoneY - targetZoneY);
					int range = (int)Math.Sqrt(diffX*diffX + diffY*diffY);
					double zSpeed = speed * diffZ / range;
					if (speed == 0)
						zSpeed = diffZ;
					str.AppendFormat("\n\tcalced zSpeed:{0}({1}) diffX:{2} diffY:{3} diffZ:{4} range:{5} speed:{6}", (int)zSpeed/4, (int)zSpeed, diffX, diffY, diffZ, range, speed);
				}*/
			}
			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			temp = ReadShort();
			speed = (short)(temp & 0x7FF);
			if ((temp & 0x800) == 0x800)
				speed = (short)-speed;
			heading = ReadShort();
			speedZ = (short)(((temp & 0x7000) >> 8) | (heading >> 12)); // ZSpeed in 0xA1 = 1/4 0xDA ZSpeed
			if ((temp & 0x8000) == 0x8000)
				speedZ = (short)-speedZ;
			heading = (ushort)(heading & 0xFFF);
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
			currentZoneId = (ushort)(ReadByte() | ((flags & 0x04) << 6));
			targetZoneId = (ushort)(ReadByte() | ((flags & 0x08) << 5));
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