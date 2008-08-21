using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player info", Priority=1000)]
	public class ShowPlayerInfoAction : AbstractEnabledAction
	{
		#region ILogAction Members

		public enum PlrState : byte
		{
			Stand = 0,
			Swim = 1,
			Jump = 2,
			debugFly = 3,
			Sit = 4,
			Died = 5,
			Ride = 6,
			Climb = 7,
		}

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

			string serverName = "UNKNOWN";
			int serverId = -1;
			int serverColorHandling = -1;
			int sessionId = -1;
			int objectId = -1;
			int petId = -1;
			int healthPercent = -1;
			int endurancePercent = -1;
			int manaPercent = -1;
			int concentrationPercent = -1;
			int speed = -1;
			int maxspeed = -1;
			int clientTargetOid = -1;
			int serverTargetOid = -1;
			int regionXOffset = 0;
			int regionYOffset = 0;
			int glocX = -1;
			int glocY = -1;
			int glocRegion = -1;
			string loc = "UNKNOWN";
			string gloc = "UNKNOWN";
			string state = "";
			string charName = "UNKNOWN";
			string lastName = "";
			int playersInGroup = -1;
			int indexInGroup = -1;
			int mountId = -1;
			int mountSlot = -1;
			int level = -1;
			int mana = -1;
			int manaMax = -1;
			int health = -1;
			int healthMax = -1;
			int conc = -1;
			int concMax = -1;
			string className = "UNKNOWN";
			string raceName = "";
			int realm_level = -1;
			string realm_title = "";
			int champ_level = -1;
			string champ_title = "";
			int ml_level = -1;
			int houseLot = -1;
			string ml_title = "";
			int guildID = -1;
			string guildName = "None";
			string guildRank = "";
			string enterRegionName = "";
			string petInfo = "";
			int insideHouseId = 0;
			Hashtable enterSubRegion = new Hashtable();

			bool flagAwait0xA9 = false;

			for (int i = 0; i <= selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is CtoS_0xA9_PlayerPosition)
				{
					CtoS_0xA9_PlayerPosition pos = (CtoS_0xA9_PlayerPosition)pak;
					speed = pos.Status & 0x1FF;
					byte plrState = (byte)((pos.Status >> 10) & 7);
					state = plrState > 0 ? ((StoC_0xA9_PlayerPosition.PlrState)plrState).ToString() : "";
					if ((pos.Status & 0x200) == 0x200)
						state += ",Backward";
					if ((pos.Status & 0x8000) == 0x8000)
						state += ",StrafeRight";
					if ((pos.Status & 0x4000) == 0x4000)
						state += ",StrafeLeft";
					if ((pos.Status & 0x2000) == 0x2000)
						state += "Move";
					if ((pos.Flag & 0x01) == 0x01)
						state += ",CtoS_0xA9_Flagx01";
					if ((pos.Flag & 0x02) == 0x02)
						state += ",Underwater";
					if ((pos.Flag & 0x04) == 0x04)
						state += ",PetInView";
					if ((pos.Flag & 0x08) == 0x08)
						state += ",GT";
					if ((pos.Flag & 0x10) == 0x10)
						state += ",CheckTargetInView";
					if ((pos.Flag & 0x20) == 0x20)
						state += ",TargetInView";
					if ((pos.Flag & 0x40) == 0x40)
						state += ",MoveTo";
					if ((pos.Health & 0x80) == 0x80)
						state += ",Combat";
					if ((pos.Speed & 0x8000) == 0x8000)
						state += ",FallDown";
					loc = string.Format("({0,-3}): ({1,-6} {2,-6} {3,-5})", pos.CurrentZoneId, pos.CurrentZoneX, pos.CurrentZoneY, pos.CurrentZoneZ);
					if (flagAwait0xA9)
					{
						if (glocX != -1 && glocY != -1)
						{
							regionXOffset = glocX - pos.CurrentZoneX;
							regionYOffset = glocY - pos.CurrentZoneY;
						}
						flagAwait0xA9 = false;
					}
					gloc = string.Format("({0,-3}): ({1,-6} {2,-6} {3,-5})", glocRegion, regionXOffset + pos.CurrentZoneX, regionYOffset + pos.CurrentZoneY, pos.CurrentZoneZ);
				}
				else if (pak is StoC_0x16_VariousUpdate)
				{
					StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate)pak;
					if (stat.SubCode == 3)
					{
						StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate)stat.SubData;
						level = subData.playerLevel;
						className = subData.className;
						raceName = subData.raceName;
						healthMax = (ushort)(((subData.maxHealthHigh & 0xFF) << 8) | (subData.maxHealthLow & 0xFF));
						realm_level = subData.realmLevel;
						realm_title = subData.realmTitle;
						ml_level = subData.mlLevel;
						ml_title = subData.mlTitle;
						houseLot = subData.personalHouse;
						guildName = subData.guildName;
						guildRank = subData.guildRank;
						lastName = subData.lastName;
						charName = subData.playerName;
						if (subData is StoC_0x16_VariousUpdate_179.PlayerUpdate_179)
						{
							champ_level = (subData as StoC_0x16_VariousUpdate_179.PlayerUpdate_179).championLevel;
							champ_title = (subData as StoC_0x16_VariousUpdate_179.PlayerUpdate_179).championTitle;
						}
					}
					else if (stat.SubCode == 6)
					{
						StoC_0x16_VariousUpdate.PlayerGroupUpdate subData = (StoC_0x16_VariousUpdate.PlayerGroupUpdate)stat.SubData;
						playersInGroup = stat.SubCount;
						for (int j = 0; j < stat.SubCount; j++)
						{
							StoC_0x16_VariousUpdate.GroupMember member = subData.groupMembers[j];
							if (objectId >= 0 && objectId == member.oid)
								indexInGroup = j;
						}
					}
				}
				else if (pak is CtoS_0x10_CharacterSelectRequest)
				{
					CtoS_0x10_CharacterSelectRequest login = (CtoS_0x10_CharacterSelectRequest)pak;
					charName = login.CharName;
				}
				else if (pak is CtoS_0xB0_TargetChange)
				{
					CtoS_0xB0_TargetChange trg = (CtoS_0xB0_TargetChange)pak;
					clientTargetOid = trg.Oid;
				}
				else if (pak is StoC_0xAF_Message)
				{
					StoC_0xAF_Message msg = (StoC_0xAF_Message)pak;
					if (msg.Text.StartsWith("You have entered "))
					{
						enterRegionName = regionNameFromMessage(msg.Text.Substring(17));
					}
					else if (msg.Text.StartsWith("(Region) You have entered "))
					{
						string enterSubRegionName = regionNameFromMessage(msg.Text.Substring(26));
						if (!enterSubRegion.ContainsKey(enterSubRegionName))
							enterSubRegion.Add(enterSubRegionName, enterSubRegionName);
					}
					else if (msg.Text.StartsWith("(Region) You have left "))
					{
						string enterSubRegionName =  regionNameFromMessage(msg.Text.Substring(23));
						enterSubRegion.Remove(enterSubRegionName);
					}
				}
				else if (pak is StoC_0xF6_ChangeTarget)
				{
					StoC_0xF6_ChangeTarget trg = (StoC_0xF6_ChangeTarget)pak;
					serverTargetOid = trg.Oid;
				}
				else if (pak is StoC_0xB6_UpdateMaxSpeed)
				{
					StoC_0xB6_UpdateMaxSpeed spd = (StoC_0xB6_UpdateMaxSpeed)pak;
					maxspeed = spd.MaxSpeedPercent;
				}
				else if (pak is StoC_0xAD_StatusUpdate)
				{
					StoC_0xAD_StatusUpdate status = (StoC_0xAD_StatusUpdate)pak;
					healthPercent = status.HealthPercent;
					endurancePercent = status.EndurancePercent;
					manaPercent = status.ManaPercent;
					concentrationPercent = status.ConcentrationPercent;
					if (pak is StoC_0xAD_StatusUpdate_190)
					{
						health = (pak as StoC_0xAD_StatusUpdate_190).Health;
						healthMax = (pak as StoC_0xAD_StatusUpdate_190).MaxHealth;
						mana = (pak as StoC_0xAD_StatusUpdate_190).Power;
						manaMax = (pak as StoC_0xAD_StatusUpdate_190).MaxPower;
						conc = (pak as StoC_0xAD_StatusUpdate_190).Concentration;
						concMax = (pak as StoC_0xAD_StatusUpdate_190).MaxConcentration;
					}
				}
				else if (pak is StoC_0x28_SetSessionId)
				{
					StoC_0x28_SetSessionId session = (StoC_0x28_SetSessionId)pak;
					sessionId = session.SessionId;
				}
				else if (pak is StoC_0x20_PlayerPositionAndObjectID)
				{
					StoC_0x20_PlayerPositionAndObjectID posAndOid = (StoC_0x20_PlayerPositionAndObjectID)pak;
					objectId = posAndOid.PlayerOid;
					glocX = (int)posAndOid.X;
					glocY = (int)posAndOid.Y;
					if ((pak as StoC_0x20_PlayerPositionAndObjectID_171) != null)
						glocRegion = (pak as StoC_0x20_PlayerPositionAndObjectID_171).Region;
					loc = "UNKNOWN";
					gloc = string.Format("({0,-3}): ({1,-6} {2,-6} {3,-5})", glocRegion, posAndOid.X, posAndOid.Y, posAndOid.Z);
					flagAwait0xA9 = true;
					enterSubRegion.Clear();
					enterRegionName = "";
					insideHouseId = 0;
				}
				else if (pak is StoC_0xDE_SetObjectGuildId)
				{
					StoC_0xDE_SetObjectGuildId guildId = (StoC_0xDE_SetObjectGuildId)pak;
					if (objectId == guildId.Oid)
						guildID = guildId.GuildId;
					if (guildId.ServerId == 0xFF) // set ObjectID for old logs (1.70-)
						objectId = guildId.Oid;
				}
				else if (pak is StoC_0x04_CharacterJump)
				{
					StoC_0x04_CharacterJump plrJump = (StoC_0x04_CharacterJump)pak;
					if (plrJump.X == 0 && plrJump.Y == 0 && plrJump.Z == 0) // skip /faceloc
						continue;
					objectId = plrJump.PlayerOid;
					glocX = (int)plrJump.X;
					glocY = (int)plrJump.Y;
					insideHouseId = plrJump.HouseId;
					loc = "UNKNOWN";
					gloc = string.Format("({0,-3}): ({1,-6} {2,-6} {3,-5})", glocRegion, plrJump.X, plrJump.Y, plrJump.Z);
					flagAwait0xA9 = true;
				}
				else if (pak is StoC_0x88_PetWindowUpdate)
				{
					StoC_0x88_PetWindowUpdate pet = (StoC_0x88_PetWindowUpdate)pak;
					petId = pet.PetId;
				}
				else if (pak is StoC_0xDA_NpcCreate)
				{
					if (petId == -1) continue;
					StoC_0xDA_NpcCreate npc = (StoC_0xDA_NpcCreate)pak;
					if (npc.Oid != petId) continue;
					petInfo = string.Format(" \"{0}\" level:{1} model:0x{2:X4}", npc.Name, npc.Level, npc.Model);
				}
				else if (pak is StoC_0xC8_PlayerRide)
				{
					StoC_0xC8_PlayerRide ride = (StoC_0xC8_PlayerRide)pak;
					if (objectId >= 0 && objectId == ride.RiderOid)
					{
						if (ride.Flag == 0)
						{
							mountId = -1;
							mountSlot = -1;
						}
						else
						{
							mountId = ride.MountOid;
							mountSlot = ride.Slot;
						}
					}
				}
				else if (pak is StoC_0xFB_CharStatsUpdate_175)
				{
					if ((pak as StoC_0xFB_CharStatsUpdate_175).Flag != 0xFF)
						healthMax = (pak as StoC_0xFB_CharStatsUpdate_175).MaxHealth;
				}
				else if (pak is StoC_0xFB_CharStatsUpdate)
				{
					healthMax = (pak as StoC_0xFB_CharStatsUpdate).MaxHealth;
				}
				else if (pak is StoC_0x2A_LoginGranted)
				{
					StoC_0x2A_LoginGranted server = (StoC_0x2A_LoginGranted)pak;
					serverName = server.ServerName;
					serverId = server.ServerId;
					serverColorHandling = server.ColorHandling;
				}
			}

			int additionStrings = 0;
			StringBuilder str = new StringBuilder();
			if (serverId > 0)
			{
				str.AppendFormat("    server: \"{0}\" id:0x{1:X2} color:{2}\n", serverName, serverId, serverColorHandling);
				additionStrings++;
			}
			str.AppendFormat("session id: 0x{0}\n", ValueToString(sessionId, "X4"));
			str.AppendFormat(" object id: 0x{0}\n", ValueToString(objectId, "X4"));
			str.AppendFormat("    pet id: 0x{0}{1}\n", ValueToString(petId, "X4"), petInfo);
			str.AppendFormat(" char name: {0}{1}\n", charName, lastName != "None" ? string.Format(" \"{0}\"", lastName) : "");
			str.AppendFormat("     level: {0} {1}{2}\n", ValueToString(level), className, (raceName != "" ? " (" + raceName + ")" : ""));
			if (guildName != "None")
			{
				str.AppendFormat("     guild: {0}", guildName);
				str.AppendFormat(" rank:{0}", guildRank);
				if (guildID != -1)
					str.AppendFormat(" guildId:0x{0:X4}", guildID);
				str.AppendFormat("\n");
				additionStrings++;
			}
			if (realm_level > 0)
			{
				str.AppendFormat("RealmLevel: {0} \"{1}\"\n", ValueToString(realm_level), realm_title);
				additionStrings++;
			}
			if (ml_level > 0)
			{
				str.AppendFormat("  ML level: {0} \"{1}\"\n", ValueToString(ml_level), ml_title);
				additionStrings++;
			}
			if (champ_level > 0)
			{
				str.AppendFormat("ChampLevel: {0} \"{1}\"\n", ValueToString(champ_level), champ_title);
				additionStrings++;
			}
			str.AppendFormat("\n");
			if(houseLot > 0)
			{
				str.AppendFormat("     HouseLot: 0x{0}\n", ValueToString(houseLot, "X4"));
				additionStrings++;
			}
			if (mountId > 0)
			{
				str.AppendFormat("     mount id: 0x{0} (slot:{1})\n", ValueToString(mountId, "X4"), ValueToString(mountSlot));
				additionStrings++;
			}
			if (playersInGroup > 0)
			{
				str.AppendFormat("        group: {0}[{1}]\n", ValueToString(indexInGroup), ValueToString(playersInGroup));
				additionStrings++;
			}
			str.AppendFormat("        speed: {0,3}\n", ValueToString(speed));
			str.AppendFormat("     maxSpeed: {0,3}%\n", ValueToString(maxspeed));
			str.AppendFormat("       health: {0,3}%", ValueToString(healthPercent));
			if (health != -1)
				str.AppendFormat(" ({0}/{1})", health, healthMax);
			else if (healthMax != -1)
				str.AppendFormat(" (maxHealth:{0})", healthMax);
			str.Append('\n');
			str.AppendFormat("         mana: {0,3}%", ValueToString(manaPercent));
			if (mana != -1)
				str.AppendFormat(" ({0}/{1})", mana, manaMax);
			str.Append('\n');
			str.AppendFormat("    endurance: {0,3}%\n", ValueToString(endurancePercent));
			str.AppendFormat("concentration: {0,3}%", ValueToString(concentrationPercent));
			if (conc != -1)
				str.AppendFormat(" ({0}/{1})", conc, concMax);
			str.Append('\n');
			str.AppendFormat(" clientTarget: 0x{0}\n", ValueToString(clientTargetOid, "X4"));
			str.AppendFormat("  checkTarget: 0x{0}\n", ValueToString(serverTargetOid, "X4"));
			str.AppendFormat(" current zone: {0}{1}\n", loc, enterRegionName == "" ? "" : " (" + enterRegionName + ")");
			str.AppendFormat("  calced gloc: {0}\n", gloc);
			int subReg = 0;
			foreach (string subRegionName in enterSubRegion.Values)
			{
				str.AppendFormat(" subRegion[{0}]: {1}\n", subReg++, subRegionName);
				additionStrings++;
			}
			if (insideHouseId != 0)
			{
				str.AppendFormat(" inside House: {0}\n", insideHouseId);
				additionStrings++;
			}
			str.AppendFormat("        flags: {0}\n", state);
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player info (right click to close)";
			infoWindow.Width = 500;
			infoWindow.Height = 310 + 15 * additionStrings;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion

		private string ValueToString(int i)
		{
			return ValueToString(i, null);
		}

		private string ValueToString(int i, string format)
		{
			if (i == -1)
				return "(unknown)";
			if (format == null)
				return i.ToString();
			return i.ToString(format);
		}

		private string regionNameFromMessage(string msg)
		{
			if (msg.Length > 0 && msg[msg.Length - 1] == '.')
				return msg.Substring(0, msg.Length - 1);
			return msg;
		}
	}
}
