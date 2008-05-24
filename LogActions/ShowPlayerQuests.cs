using System.Collections;
using System.IO;
using System.Text;
using PacketLogConverter.LogPackets;
using PacketLogConverter.LogWriters;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player active quests")]
	public class ShowPlayerQuestAction : AbstractEnabledAction
	{
		#region ILogAction Members

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public override bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			int selectedIndex = selectedPacket.PacketIndex;

			Packet[] quests = new Packet[26];
			for (int i = 0; i < selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x83_QuestUpdate_186)
				{
	 				if ((pak as StoC_0x83_QuestUpdate_186).InNewQuestUpdate != null)
					{
						quests[((pak as StoC_0x83_QuestUpdate_186).InNewQuestUpdate).index] = pak;
					}
	 				else if ((pak as StoC_0x83_QuestUpdate).InQuestUpdate != null)
					{
						int index = ((pak as StoC_0x83_QuestUpdate).InQuestUpdate).index;
						if (((pak as StoC_0x83_QuestUpdate).InQuestUpdate).lenName == 0 && ((pak as StoC_0x83_QuestUpdate).InQuestUpdate).lenDesc == 0)
						{
							quests[index] = null;
						}
						else
							quests[index] = pak;
					}
				}
				else if (pak is StoC_0x83_QuestUpdate)
				{
					int index = ((pak as StoC_0x83_QuestUpdate).InQuestUpdate).index;
					if (((pak as StoC_0x83_QuestUpdate).InQuestUpdate).lenName == 0 && ((pak as StoC_0x83_QuestUpdate).InQuestUpdate).lenDesc == 0)
					{
						quests[index] = null;
					}
					else
						quests[index] = pak;
				}
			}

			StringBuilder str = new StringBuilder();
			for (int i = 0; i < quests.Length; i++)
			{
				Packet pak = quests[i];
				if (pak != null)
				{
					str.Append(pak.GetPacketDataString(true));
					str.Append('\n');
				}
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player quest info (right click to close)";
			infoWindow.Width = 820;
			infoWindow.Height = 320;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion
	}
}
