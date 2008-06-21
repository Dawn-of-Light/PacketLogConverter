using System.Collections;
using System.IO;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player group")]
	public class ShowPlayerGroupAction : AbstractEnabledAction
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
			string charName = "";

			GroupMemberInfo[] groupMembers = new GroupMemberInfo[8];
			for (int i = 0 ; i <= selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is CtoS_0x10_CharacterSelectRequest)
				{
					CtoS_0x10_CharacterSelectRequest login = (CtoS_0x10_CharacterSelectRequest)pak;
					charName = login.CharName;
				}
				else if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate groupPak = (StoC_0x16_VariousUpdate)pak;
					if (groupPak.SubCode == 3)
					{
						StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate)groupPak.SubData;
						charName = subData.playerName;
					}
					else if (groupPak.SubCode == 6) // group update
					{
						StoC_0x16_VariousUpdate.PlayerGroupUpdate grpUpdate = groupPak.InPlayerGroupUpdate;
						if (grpUpdate != null)
						{
							for (int grpIndex = 0; grpIndex < 8; grpIndex++)
							{
								if (grpUpdate.groupMembers.Length > grpIndex)
								{
									if (groupMembers[grpIndex] == null)
									{
										groupMembers[grpIndex] = new GroupMemberInfo();
									}
									groupMembers[grpIndex].groupMember = grpUpdate.groupMembers[grpIndex];
									groupMembers[grpIndex].flag_0x16_UpdateIsLast = true;
									groupMembers[grpIndex].flagMemberInGroup= true;
								}
								else
								{
									if (groupMembers[grpIndex] != null)
										groupMembers[grpIndex].flagMemberInGroup = false;
								}
							}
						}
					}
				}
				else if (pak is StoC_0x70_PlayerGroupUpdate)
				{
					StoC_0x70_PlayerGroupUpdate groupPak = (StoC_0x70_PlayerGroupUpdate)pak;
					foreach (object o in groupPak.Updates)
					{
						if (o is StoC_0x70_PlayerGroupUpdate.PlayerStatusData)
						{
							StoC_0x70_PlayerGroupUpdate.PlayerStatusData stat = o as StoC_0x70_PlayerGroupUpdate.PlayerStatusData;
							if (groupMembers[stat.playerIndex] == null)
							{
								groupMembers[stat.playerIndex] = new GroupMemberInfo();
							}
							groupMembers[stat.playerIndex].playerStatusData = stat;
							groupMembers[stat.playerIndex].flag_0x16_UpdateIsLast = false;
						}
						else if (o is StoC_0x70_PlayerGroupUpdate.PlayerBuffsData)
						{
							StoC_0x70_PlayerGroupUpdate.PlayerBuffsData buffs = o as StoC_0x70_PlayerGroupUpdate.PlayerBuffsData;
							if (groupMembers[buffs.playerIndex] == null)
							{
								groupMembers[buffs.playerIndex] = new GroupMemberInfo();
							}
							groupMembers[buffs.playerIndex].playerBuffsData = buffs;
						}
						else if (o is StoC_0x70_PlayerGroupUpdate_173.PlayerMapData)
						{
							StoC_0x70_PlayerGroupUpdate_173.PlayerMapData map = o as StoC_0x70_PlayerGroupUpdate_173.PlayerMapData;
							if (groupMembers[map.player] == null)
							{
								groupMembers[map.player] = new GroupMemberInfo();
							}
							groupMembers[map.player].playerMapData = map;
						}
					}
				}
			}

			StringBuilder str = new StringBuilder();
			bool found = false;
			for (int i = 0; i < groupMembers.Length; i++)
			{
				if (groupMembers[i] != null)
				{
					bool flagMemberInfoPrinted = false;
					if (!groupMembers[i].flagMemberInGroup)
						continue;
					found = true;
					str.AppendFormat("player{0}: level:{1,-2} oid:0x{2:X4} class:\"{3}\"\t name:\"{4}\"", i, groupMembers[i].groupMember.level, groupMembers[i].groupMember.oid, groupMembers[i].groupMember.classname, groupMembers[i].groupMember.name);
					if (charName == groupMembers[i].groupMember.name)
						str.Append(" (YOU)");
					str.Append("\n");
					if (groupMembers[i].flag_0x16_UpdateIsLast)
					{
						str.AppendFormat("player{0}: health:{1,3}% mana:{2,3}% endurance:{3,3}% status:0x{4:X2}", i, groupMembers[i].groupMember.health, groupMembers[i].groupMember.mana, groupMembers[i].groupMember.endurance, groupMembers[i].groupMember.status);
						flagMemberInfoPrinted = true;
						str.Append("\n");
					}
					if (groupMembers[i].playerStatusData != null && !flagMemberInfoPrinted)
					{
						str.Append(groupMembers[i].playerStatusData.ToString());
						str.Append("\n");
					}
					if (groupMembers[i].playerMapData != null)
					{
						str.Append(groupMembers[i].playerMapData.ToString());
						str.Append("\n");
					}
					if (groupMembers[i].playerBuffsData != null)
					{
						str.Append(groupMembers[i].playerBuffsData.ToString());
						str.Append("\n");
					}
					str.Append("\n");
				}
			}
			if (!found)
				str.Append("Group info not found\n");
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player group info (right click to close)";
			infoWindow.Width = 820;
			infoWindow.Height = 320;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		class GroupMemberInfo
		{
			public StoC_0x16_VariousUpdate.GroupMember groupMember;
			public StoC_0x70_PlayerGroupUpdate.PlayerStatusData playerStatusData;
			public StoC_0x70_PlayerGroupUpdate.PlayerBuffsData playerBuffsData;
			public StoC_0x70_PlayerGroupUpdate_173.PlayerMapData playerMapData;
			public bool flag_0x16_UpdateIsLast;
			public bool flagMemberInGroup;
		}
		#endregion
	}
}
