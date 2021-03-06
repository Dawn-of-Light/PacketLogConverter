using System;
using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x84, -1, ePacketDirection.ClientToServer, "Looking for group")]
	public class CtoS_0x84_LookingForGroupFilter: Packet
	{
		protected byte lfgFlag;
		protected byte unk1; // Privious LFG ?
		protected byte[] filter;
		protected byte lvlMin;
		protected byte lvlMax;
		protected byte unk2;
		protected ushort unk3;

		#region public access properties

		public byte LfgFlag { get { return lfgFlag; } }
		public byte Unk1 { get { return unk1; } }
		public byte LvlMin { get { return lvlMin; } }
		public byte[] Filter { get {return filter; } }
		public byte LvlMax { get { return lvlMax; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("lfg:{0} code:{1} lvlMin:{2} lvlMax:{3})",
				lfgFlag, unk1, lvlMin, lvlMax);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			lfgFlag = ReadByte();
			unk1 = ReadByte();
			ArrayList tmp = new ArrayList(10);
			for (byte i = 0; i < 117; i++)
				tmp.Add(ReadByte());
			filter = (byte[])tmp.ToArray(typeof (byte));
			lvlMin = ReadByte();
			lvlMax = ReadByte();
			unk2 = ReadByte();
			unk3 = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x84_LookingForGroupFilter(int capacity) : base(capacity)
		{
		}
	}
}