using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xD8, 186, ePacketDirection.ClientToServer, "Detail display request v186")]
	public class CtoS_0xD8_DetailDisplayRequest_186 : CtoS_0xD8_DetailDisplayRequest
	{
		protected uint unk1;

		#region public access properties

		public uint Unk1 { get { return unk1; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("objectType:0x{0:X4} objectId:0x{1:X4} unk1:0x{2:X8}", objectType, objectId, unk1);
			if (flagsDescription)
			{
				if (objectType == 19)
					text.Write(" (questId:0x{0:X4} slot:{1})", ((unk1 & 0x0F) << 12) + (objectId >> 4), objectId & 0x0F);
				if (objectType >= 150)
					text.Write(" (CL IdLine:{0} SkillIndex:{1} index:{2})", objectType - 150, objectId >> 8, objectId & 0xFF);
			}
		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			objectType = ReadShort();
			unk1 = ReadInt();
			objectId = ReadShort();
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xD8_DetailDisplayRequest_186(int capacity) : base(capacity)
		{
		}
	}
}