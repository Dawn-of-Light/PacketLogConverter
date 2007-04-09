using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xB0, -1, ePacketDirection.ClientToServer, "Target change")]
	public class CtoS_0xB0_TargetChange : Packet, IObjectIdPacket
	{
		protected ushort oid;
		protected ushort flags;

		/// <summary>
		/// Gets the object ids of the packet.
		/// </summary>
		/// <value>The object ids.</value>
		public ushort[] ObjectIds
		{
			get { return new ushort[] { oid }; }
		}

		#region public access properties

		public ushort Oid { get { return oid; } }
		public ushort Flags { get { return flags; } }

		#endregion

		public override string GetPacketDataString(bool flagsDescription)
		{
			StringBuilder str = new StringBuilder();

			str.AppendFormat("oid:0x{0:X4} flags:0x{1:X4}", oid, flags);
			if (flagsDescription)
			{
				string flag = "";
				if ((flags & 0x0001) == 0x0001)
					flag += ",InCombat";
				if ((flags & 0x0002) == 0x0002)
					flag += ",UNKx0002";
				if ((flags & 0x0004) == 0x0004)
					flag += ",UNKx0004";
				if ((flags & 0x0008) == 0x0008)
					flag += ",UNKx0008";
				if ((flags & 0x0010) == 0x0010)
					flag += ",UNKx0010";
				if ((flags & 0x0020) == 0x0020)
					flag += ",UNKx0020";
				if ((flags & 0x0040) == 0x0040)
					flag += ",UNKx0040";
				if ((flags & 0x0080) == 0x0080)
					flag += ",UNKx0080";
				if ((flags & 0x0100) == 0x0100)
					flag += ",UNKx0100";
				if ((flags & 0x0200) == 0x0200)
					flag += ",UNKx0200";
				if ((flags & 0x0400) == 0x0400)
					flag += ",UNKx0400";
				if ((flags & 0x0800) == 0x0800) // ControledPet in view
					flag += ",PetInView";
				if ((flags & 0x1000) == 0x1000) // GT
					flag += ",GTinView";
				if ((flags & 0x2000) == 0x2000) // LOS
					flag += ",CheckTargetInView";
				if ((flags & 0x4000) == 0x4000) // LOS
					flag += ",TargetInView";
				if ((flags & 0x8000) == 0x8000) // click mouse
					flag += ",MouseClick";
				if (flag.Length > 0)
					str.AppendFormat(" ({0})", flag);
			}

			return str.ToString();
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;
			oid = ReadShort();
			flags = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xB0_TargetChange(int capacity) : base(capacity)
		{
		}
	}
}