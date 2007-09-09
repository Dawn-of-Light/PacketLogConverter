using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player info", Priority=1000)]
	public class ShowPlayerInfoAction : ILogAction
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
		/// Activate log action
		/// </summary>
		/// <param name="log">The current log</param>
		/// <param name="selectedIndex">The selected packet index</param>
		/// <returns>True if log data tab should be updated</returns>
		public virtual bool Activate(PacketLog log, int selectedIndex)
		{
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

			bool flagAwait0xA9 = false;

			for (int i = 0; i <= selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is CtoS_0xA9_PlayerPosition)
				{
					CtoS_0xA9_PlayerPosition pos = (CtoS_0xA9_PlayerPosition)pak;
					speed = pos.Status & 0x1FF;
					byte plrState = (byte)((pos.Status >> 10) & 7);
					state = plrState > 0 ? ((PlrState)plrState).ToString() : "";
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
//							PacketLogConverter.LogWriters.Logger.Say(string.Format("glocX:{0} glocY:{1} regXOffset:{2} regYOffset:{3} @X:{4} @Y:{5}", glocX, glocY, regionXOffset, regionYOffset, pos.CurrentZoneX, pos.CurrentZoneY));
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
						if (subData is StoC_0x16_VariousUpdate_179.PlayerUpdate_179)
						{
							champ_level = (subData as StoC_0x16_VariousUpdate_179.PlayerUpdate_179).championLevel;
							champ_title = (subData as StoC_0x16_VariousUpdate_179.PlayerUpdate_179).championTitle;
						}
					}
					else if (stat.SubCode == 6)
					{
						StoC_0x16_VariousUpdate.PlayerGroupUpdate subData = (StoC_0x16_VariousUpdate.PlayerGroupUpdate)stat.SubData;
						playersInGroup = subData.count;
						for (int j = 0; j < subData.count; j++)
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
					flagAwait0xA9 = true;
				}
				else if (pak is StoC_0x04_CharacterJump)
				{
					StoC_0x04_CharacterJump plrJump = (StoC_0x04_CharacterJump)pak;
					objectId = plrJump.PlayerOid;
					glocX = (int)plrJump.X;
					glocY = (int)plrJump.Y;
					flagAwait0xA9 = true;
				}
				else if (pak is StoC_0x88_PetWindowUpdate)
				{
					StoC_0x88_PetWindowUpdate pet = (StoC_0x88_PetWindowUpdate)pak;
					petId = pet.PetId;
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
			}

			StringBuilder str = new StringBuilder();
			str.AppendFormat("session id: 0x{0}\n", ValueToString(sessionId, "X4"));
			str.AppendFormat(" object id: 0x{0}\n", ValueToString(objectId, "X4"));
			str.AppendFormat("    pet id: 0x{0}\n", ValueToString(petId, "X4"));
			str.AppendFormat(" char name: {0}\n", charName);
			str.AppendFormat("     level: {0} {1}{2}\n", ValueToString(level), className, (raceName != "" ? " (" + raceName + ")" : ""));
			if (realm_level > 0)
				str.AppendFormat("RealmLevel: {0} \"{1}\"\n", ValueToString(realm_level), realm_title);
			if (ml_level > 0)
				str.AppendFormat("  ML level: {0} \"{1}\"\n", ValueToString(ml_level), ml_title);
			if (champ_level > 0)
				str.AppendFormat("ChampLevel: {0} \"{1}\"\n", ValueToString(champ_level), champ_title);
			str.AppendFormat("\n");
			if(houseLot > 0)
				str.AppendFormat("     houseLot: 0x{0}\n", ValueToString(houseLot, "X4"));
			if(mountId > 0)
				str.AppendFormat("     mount id: 0x{0} (slot:{1})\n", ValueToString(mountId, "X4"), ValueToString(mountSlot));
			if(playersInGroup > 0)
				str.AppendFormat("        group: {0}[{1}]\n", ValueToString(indexInGroup), ValueToString(playersInGroup));
			str.AppendFormat("        speed: {0,3}\n", ValueToString(speed));
			str.AppendFormat("     maxSpeed: {0,3}%\n", ValueToString(maxspeed));
			str.AppendFormat("       health: {0,3}%", ValueToString(healthPercent));
			if (health != -1)
				str.AppendFormat(" ({0}/{1})\n", health, healthMax);
			else if (healthMax != -1)
				str.AppendFormat(" (maxHealth:{0})\n", healthMax);
			else
				str.Append('\n');
			str.AppendFormat("         mana: {0,3}%", ValueToString(manaPercent));
			if (mana != -1)
				str.AppendFormat(" ({0}/{1})\n", mana, manaMax);
			else
				str.Append('\n');
			str.AppendFormat("    endurance: {0,3}%\n", ValueToString(endurancePercent));
			str.AppendFormat("concentration: {0,3}%", ValueToString(concentrationPercent));
			if (conc != -1)
				str.AppendFormat(" ({0}/{1})\n", conc, concMax);
			else
				str.Append('\n');
			str.AppendFormat(" clientTarget: 0x{0}\n", ValueToString(clientTargetOid, "X4"));
			str.AppendFormat("  checkTarget: 0x{0}\n", ValueToString(serverTargetOid, "X4"));
			str.AppendFormat(" current zone: {0}\n", loc);
			str.AppendFormat("  calced gloc: {0}\n", gloc);
			str.AppendFormat("        flags: {0}\n", state);

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player info (right click to close)";
			infoWindow.Width = 500;
			infoWindow.Height = 350;
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
	}
}
