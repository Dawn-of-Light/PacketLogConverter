//#define SHOW_PACKETS
using System.IO;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("Emotion writer", "*.txt")]
	public class EmotionWriter : ILogWriter
	{
		public void WriteLog(PacketLog log, Stream stream, ProgressCallback callback)
		{
			Packet prevpack = null;
			int playerOid = -1;
			ushort targetOid = 0xFFFF;
			using (StreamWriter s = new StreamWriter(stream))
			{
				for (int i = 0; i < log.Count; i++)
				{
					if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
						callback(i, log.Count-1);

					Packet pak = log[i];
					if (pak is StoC_0x20_PlayerPositionAndObjectID)
					{
						StoC_0x20_PlayerPositionAndObjectID plr = (StoC_0x20_PlayerPositionAndObjectID)pak;
						playerOid = plr.PlayerOid;
#if SHOW_PACKETS
						s.WriteLine("playerOid:0x{0:X4}", playerOid);
#endif
					}
					else if (pak is CtoS_0xB0_TargetChange)
					{
						CtoS_0xB0_TargetChange target = (CtoS_0xB0_TargetChange)pak;
						targetOid = target.Oid;
					}
					else if (pak is StoC_0xF9_EmoteAnimation)
					{
#if SHOW_PACKETS
						StoC_0xF9_EmoteAnimation emote = (StoC_0xF9_EmoteAnimation)pak;
						if (emote.Oid == playerOid)
						{
							s.WriteLine("{0, -16} oid:0x{1:X4} target:0x{4:X4} emote:{2}({3})",  pak.Time.ToString(), emote.Oid, emote.Emote, (StoC_0xF9_EmoteAnimation.eEmote)emote.Emote, targetOid);
						}
						else
						{
							s.WriteLine("{0, -16} oid:0x{1:X4}               emote:{2}({3})", pak.Time.ToString(), emote.Oid, emote.Emote, (StoC_0xF9_EmoteAnimation.eEmote)emote.Emote);
						}
#endif
					}
					else if (pak is StoC_0xAF_Message)
					{
						StoC_0xAF_Message msg = (StoC_0xAF_Message)pak;
						if(msg.Type == 6) // emote messages
						{
#if SHOW_PACKETS
							s.WriteLine("{0, -16} 0xAF 0x{1:X2} {2}", pak.Time.ToString(), msg.Type, msg.Text);
#endif
							if (prevpack != null && prevpack is StoC_0xF9_EmoteAnimation)
							{
								string targetStr = "";
								ushort target = 0xFFFF;
								StoC_0xF9_EmoteAnimation emote = (StoC_0xF9_EmoteAnimation)prevpack;
								if (emote.Oid == playerOid)
									target = targetOid;
								if (target == 0xFFFF)
									targetStr = "unknown";
								else if (target == 0)
									targetStr = "none";
								else if (target == playerOid)
									targetStr = "self";
								else
									targetStr = string.Format("0x{0:X4}", target);
								s.WriteLine("codeEmote:0x{0:X2}({1,-2}) emote:{2, -15} target:{4,-10} \"{3}\"", emote.Emote, emote.Emote, (StoC_0xF9_EmoteAnimation.eEmote)emote.Emote, msg.Text, targetStr);
							}
						}
					}
					prevpack = pak;
				}
			}
		}
	}
}
