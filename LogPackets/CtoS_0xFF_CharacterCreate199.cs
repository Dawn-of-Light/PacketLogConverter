using System.Collections;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0xFF, 199, ePacketDirection.ClientToServer, "Create Character v199")]
	public class CtoS_0xFF_CreateCharacter_199 : CtoS_0xFF_CreateCharacter_189
	{

		/// <summary>
		/// Set all log variables from the packet here
		/// </summary>
		/// <param name="log"></param>
		public override void InitLog(PacketLog log)
		{
			// Reinit only on for 190 version and subversion lower 190.1
			if (!log.IgnoreVersionChanges && log.Version >= 199 && log.Version < 199.1f)
			{
				if (Length == 1904)
				{
					log.Version = 199.1f;
					log.SubversionReinit = true;
//					log.IgnoreVersionChanges = true;
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0xFF_CreateCharacter_199(int capacity) : base(capacity)
		{
		}
	}
}