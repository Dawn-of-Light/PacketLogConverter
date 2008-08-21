using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x91, 190, ePacketDirection.ServerToClient, "Update points 190")]
	public class StoC_0x91_UpdatePoints_190 : StoC_0x91_UpdatePoints_179
	{
		protected ulong experience;
		protected ulong expNextLevel;
		protected ulong champExp;
		protected ulong champExpNextLevel;

		#region public access properties

		public ulong Experience { get { return experience; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			base.GetPacketDataString(text, flagsDescription);
			text.Write(" exp:{0, -12} expNextLevel:{1, -12} champExp:{2,-8} champExpNextLevel:{3}", experience, expNextLevel, champExp, champExpNextLevel);
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			realmPoints = ReadInt();                 // 0x00
			levelPermill = ReadShort();              // 0x04
			skillSpecPoints = ReadShort();           // 0x06
			bountyPoints = ReadInt();                // 0x08
			realmSpecPoints = ReadShort();           // 0x0C
			champLevelPermill = ReadShort();         // 0x0E
			experience = ReadLongLowEndian();        // 0x10
			expNextLevel = ReadLongLowEndian();      // 0x18
			champExp = ReadLongLowEndian();          // 0x20
			champExpNextLevel = ReadLongLowEndian(); // 0x28
		}
				
		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public StoC_0x91_UpdatePoints_190(int capacity) : base(capacity)
		{
		}
	}
}