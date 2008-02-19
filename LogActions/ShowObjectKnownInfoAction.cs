using System;
using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows info about object
	/// </summary>
	[LogAction("Show info about object", Priority=999)]
	public class ShowObjectKnownInfoAction : ILogAction
	{

		#region ILogAction Members

		/// <summary>
		/// Determines whether the action is enabled.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns>
		/// 	<c>true</c> if the action is enabled; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			Packet pak = context.LogManager.GetPacket(selectedPacket);
			if (pak is IObjectIdPacket)
				return true;
			if (pak is ISessionIdPacket)
				return true;
			if (pak is IHouseIdPacket)
				return true;
			if (pak is IKeepIdPacket)
				return true;
			return false;
		}

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			int selectedIndex = selectedPacket.PacketIndex;

			ushort sessionId = 0;
			ushort[] objectIds = null;
			ushort[] keepIds = null;
			ushort houseId = 0;
			Packet pak = log[selectedIndex];
			if (pak is IObjectIdPacket)
				objectIds = (pak as IObjectIdPacket).ObjectIds;
			if (pak is ISessionIdPacket)
				sessionId = (pak as ISessionIdPacket).SessionId;
			if (pak is IHouseIdPacket)
				houseId = (pak as IHouseIdPacket).HouseId;
			if (pak is IKeepIdPacket)
				keepIds = (pak as IKeepIdPacket).KeepIds;

			StringBuilder str = new StringBuilder();
			if (sessionId == 0 && houseId == 0 && (objectIds == null || objectIds.Length == 0) && (keepIds == null || keepIds.Length == 0))
				str.AppendFormat("packet not have any ID\n");

			if (sessionId > 0)
				str.Append(MakeSidInfo(log, selectedIndex, sessionId));
			if (houseId > 0)
				str.Append(MakeHidInfo(log, selectedIndex, houseId));
			if (objectIds != null && objectIds.Length > 0)
			{
				for (int i = 0; i < objectIds.Length; i++)
					if (objectIds[i] > 0)
						str.Append(MakeOidInfo(log, selectedIndex, objectIds[i], 0, 0));
			}
			if (keepIds != null && keepIds.Length > 0)
			{
				for (int i = 0; i < keepIds.Length; i++)
					if (keepIds[i] > 0)
						str.Append(MakeOidInfo(log, selectedIndex, 0xFFFF, 0, keepIds[i]));
//						str.Append(MakeKidInfo(log, selectedIndex, keepIds[i]));
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Show info about Objects (right click to close)";
			infoWindow.Width = 620;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion

		private string MakeOidInfo(PacketLog log, int selectedIndex, ushort objectId, ushort sessionId, ushort keepId)
		{
			StringBuilder str = new StringBuilder();
			int maxPacketsLookBackForGetCoords = 100000; // look back in log max 30 packets for find coords after found obejct creation packet
			bool fullInfoFound = false;
			bool flagPacketMove = false;
			bool flagPacketItem = false;
			bool flagPacketRide = false;
			bool flagPacketEquip = false;
			bool flagPacketGuild = false;
			bool flagPacketPet = false;
			bool flagPacketDialog = false;
			bool flagVisualEffect = false;
			bool flagPacketMobCreate = false;
			bool flagAllowGetFirstSelfCoords = true;
			ushort selfId = 0;
			CtoS_0xA9_PlayerPosition lastSelfCoords = null;
			LocalCoords lastObjectCoords = null;
			LocalCoords regionGlobalCoords = null;
			LocalCoords firstSelfCoords = null;
//			Packet lastObjectCoords = null;
			string objectType = "";
			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (fullInfoFound)
					maxPacketsLookBackForGetCoords--;
				if (pak is StoC_0x20_PlayerPositionAndObjectID) // stop scanning packets on enter region
				{
					if ((pak as StoC_0x20_PlayerPositionAndObjectID).PlayerOid == objectId)
						objectType = " (Self)";
//					str.Insert(0, pak.ToHumanReadableString(TimeSpan(0), true) + '\n');
					if (regionGlobalCoords == null) // skip if gloc Jump
						regionGlobalCoords = new LocalCoords((pak as StoC_0x20_PlayerPositionAndObjectID).X, (pak as StoC_0x20_PlayerPositionAndObjectID).Y, (pak as StoC_0x20_PlayerPositionAndObjectID).Z);
					flagAllowGetFirstSelfCoords = false;
					break;
				}
				else if (pak is StoC_0x04_CharacterJump)
				{
#warning TODO: Rewrite for find self Oid
					if ((pak as StoC_0x04_CharacterJump).PlayerOid == selfId)
					{
						regionGlobalCoords = new LocalCoords((pak as StoC_0x04_CharacterJump).X, (pak as StoC_0x04_CharacterJump).Y, (pak as StoC_0x04_CharacterJump).Z);
						flagAllowGetFirstSelfCoords = false;
//						break;
					}
				}
				else if (!fullInfoFound && pak is StoC_0xD4_PlayerCreate)
				{
					if ((pak as StoC_0xD4_PlayerCreate).Oid == objectId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						objectType = " (player)";
						fullInfoFound = true;
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0xD4_PlayerCreate).ZoneX, (pak as StoC_0xD4_PlayerCreate).ZoneY, (pak as StoC_0xD4_PlayerCreate).ZoneZ, (ushort)(pak as StoC_0xD4_PlayerCreate).ZoneId);
					}
				}
				else if (!fullInfoFound && pak is StoC_0x4B_PlayerCreate_172)
				{
					if ((pak as StoC_0x4B_PlayerCreate_172).Oid == objectId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						objectType = " (player)";
						fullInfoFound = true;
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0x4B_PlayerCreate_172).ZoneX, (pak as StoC_0x4B_PlayerCreate_172).ZoneY, (pak as StoC_0x4B_PlayerCreate_172).ZoneZ, (pak as StoC_0x4B_PlayerCreate_172).ZoneId);
					}
				}
				else if (!fullInfoFound && pak is StoC_0xDA_NpcCreate)
				{
					if (!flagPacketMobCreate && (pak as StoC_0xDA_NpcCreate).Oid == objectId)
					{
						flagPacketMobCreate = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						objectType = " (NPC)";
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0xDA_NpcCreate).X, (pak as StoC_0xDA_NpcCreate).Y, (pak as StoC_0xDA_NpcCreate).Z);
//						fullInfoFound = true; // need find 0x20 for get gloc region
					}
				}
				else if (!fullInfoFound && pak is StoC_0xD9_ItemDoorCreate)
				{
					if (!flagPacketItem && (pak as StoC_0xD9_ItemDoorCreate).Oid == objectId)
					{
						flagPacketItem = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						if ((pak as StoC_0xD9_ItemDoorCreate).ExtraBytes == 4)
							objectType = " (Door)";
						else
							objectType = " (Object)";
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0xD9_ItemDoorCreate).X, (pak as StoC_0xD9_ItemDoorCreate).Y, (pak as StoC_0xD9_ItemDoorCreate).Z);
//						fullInfoFound = true; // need find 0x20 for get gloc region
					}
				}
				else if (!fullInfoFound && pak is StoC_0x12_CreateMovingObject)
				{
					if ((pak as StoC_0x12_CreateMovingObject).ObjectOid == objectId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						objectType = " (MovingObject)";
						fullInfoFound = true;
					}
				}
				else if (!fullInfoFound && pak is StoC_0xC8_PlayerRide) // show object if rided
				{
					if (!flagPacketRide && (pak as StoC_0xC8_PlayerRide).RiderOid == objectId)
					{
						flagPacketRide = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
					else if ((pak as StoC_0xC8_PlayerRide).MountOid == objectId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x15_EquipmentUpdate)
				{
					if (!flagPacketEquip && (pak as StoC_0x15_EquipmentUpdate).Oid == objectId)
					{
						flagPacketEquip = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0xEE_NpcChangeProperties)
				{
					if ((pak as StoC_0xEE_NpcChangeProperties).Oid == objectId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0xDE_SetObjectGuildId)
				{
					if (!flagPacketGuild && (pak as StoC_0xDE_SetObjectGuildId).Oid == objectId)
					{
						flagPacketGuild = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0xA1_NpcUpdate)
				{
					if (!flagPacketMove && (pak as StoC_0xA1_NpcUpdate).NpcOid == objectId)
					{
						flagPacketMove = true;
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0xA1_NpcUpdate).CurrentZoneX, (pak as StoC_0xA1_NpcUpdate).CurrentZoneY, (pak as StoC_0xA1_NpcUpdate).CurrentZoneZ, (pak as StoC_0xA1_NpcUpdate).CurrentZoneId);
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x81_ShowDialog)
				{
					if (!flagPacketDialog && (pak as StoC_0x81_ShowDialog).ObjectIds != null && (pak as StoC_0x81_ShowDialog).ObjectIds.Length > 0 && (pak as StoC_0x81_ShowDialog).ObjectIds[0] == objectId)
					{
						flagPacketDialog = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0xA9_PlayerPosition)
				{
					if (!flagPacketMove && (pak as StoC_0xA9_PlayerPosition).SessionId == sessionId)
					{
						flagPacketMove = true;
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0xA9_PlayerPosition).CurrentZoneX, (pak as StoC_0xA9_PlayerPosition).CurrentZoneY, (pak as StoC_0xA9_PlayerPosition).CurrentZoneZ, (pak as StoC_0xA9_PlayerPosition).CurrentZoneId);
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x88_PetWindowUpdate)
				{
					if (!flagPacketPet && (pak as StoC_0x88_PetWindowUpdate).PetId == objectId)
					{
						flagPacketPet = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x4C_VisualEffect)
				{
					if (!flagVisualEffect && (pak as StoC_0x4C_VisualEffect).Oid == objectId)
					{
						flagVisualEffect = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (pak is CtoS_0xA9_PlayerPosition)
				{
					if (flagAllowGetFirstSelfCoords)
					{
						if (firstSelfCoords == null)
						{
							firstSelfCoords = new LocalCoords((pak as CtoS_0xA9_PlayerPosition).CurrentZoneX, (pak as CtoS_0xA9_PlayerPosition).CurrentZoneY, (pak as CtoS_0xA9_PlayerPosition).CurrentZoneZ, 0xFFFF);
						}
						else
						{
							firstSelfCoords.CurrentZoneX = (pak as CtoS_0xA9_PlayerPosition).CurrentZoneX;
							firstSelfCoords.CurrentZoneY = (pak as CtoS_0xA9_PlayerPosition).CurrentZoneY;
							firstSelfCoords.CurrentZoneZ = (pak as CtoS_0xA9_PlayerPosition).CurrentZoneZ;
						}
					}

					if (lastSelfCoords == null)
					{
						lastSelfCoords = pak as CtoS_0xA9_PlayerPosition;
					}
				}
				else if (pak is StoC_0x69_KeepOverview)
				{
					if ((pak as StoC_0x69_KeepOverview).KeepId == keepId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						if (lastObjectCoords == null)
							lastObjectCoords = new LocalCoords((pak as StoC_0x69_KeepOverview).KeepX, (pak as StoC_0x69_KeepOverview).KeepY);
					}
				}
				else if (!fullInfoFound && pak is StoC_0x61_KeepRepair)
				{
					if ((pak as StoC_0x61_KeepRepair).KeepId == keepId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x62_KeepClaim)
				{
					if ((pak as StoC_0x62_KeepClaim).KeepId == keepId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x67_KeepUpdate)
				{
					if ((pak as StoC_0x67_KeepUpdate).KeepId == keepId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (!fullInfoFound && pak is StoC_0x6C_KeepComponentOverview)
				{
					if ((pak as StoC_0x6C_KeepComponentOverview).Uid == objectId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						objectType = " (KeepComponent)";
						fullInfoFound = true;
					}
				}
				// if we found object creation packets and both coords (self and object) then break;
				// if we found object creation packets but not find any coords break after maxPacketsLookBackForGetCoords look back
				if (fullInfoFound && ((lastObjectCoords != null && lastSelfCoords != null) || maxPacketsLookBackForGetCoords <= 0))
					break;
			}
//			if (!fullInfoFound)
//				str.Insert(0, "No more info found\n");
			// TODO add zone checks... (mb recalc in global coords if zones is different ?)
			if (lastSelfCoords != null && lastObjectCoords != null && lastSelfCoords.CurrentZoneId == lastObjectCoords.CurrentZoneId)
			{
				long xdiff = (long)lastSelfCoords.CurrentZoneX - lastObjectCoords.CurrentZoneX;
				long ydiff = (long)lastSelfCoords.CurrentZoneY - lastObjectCoords.CurrentZoneY;
				long zdiff = (long)lastSelfCoords.CurrentZoneZ - lastObjectCoords.CurrentZoneZ;
				int range = (int)Math.Sqrt(xdiff * xdiff + ydiff * ydiff + zdiff * zdiff);
				str.Insert(0, string.Format("range from you:{0} (loc/loc)\n", range));
			}
			else if (lastSelfCoords != null && lastObjectCoords != null && lastObjectCoords.CurrentZoneId == 0xFFFF && regionGlobalCoords != null && firstSelfCoords != null) /* gloc Object */
			{
				long xdiff = (long)regionGlobalCoords.CurrentZoneX - firstSelfCoords.CurrentZoneX + lastSelfCoords.CurrentZoneX - lastObjectCoords.CurrentZoneX;
				long ydiff = (long)regionGlobalCoords.CurrentZoneY - firstSelfCoords.CurrentZoneY + lastSelfCoords.CurrentZoneY - lastObjectCoords.CurrentZoneY;
				long zdiff = 0;
				if (lastObjectCoords.CurrentZoneZ != 0xFFFF)
				{
					zdiff = (long)lastSelfCoords.CurrentZoneZ - lastObjectCoords.CurrentZoneZ;
				}
				int range = (int)Math.Sqrt(xdiff * xdiff + ydiff * ydiff + zdiff * zdiff);
				str.Insert(0, string.Format("range from you:{0} (loc/gloc)\n", range));
				str.Insert(0, string.Format("Your gloc:(x:{0} y:{1})\n", regionGlobalCoords.CurrentZoneX - firstSelfCoords.CurrentZoneX + lastSelfCoords.CurrentZoneX, regionGlobalCoords.CurrentZoneY - firstSelfCoords.CurrentZoneY + lastSelfCoords.CurrentZoneY));
			}
			else if (regionGlobalCoords != null && lastObjectCoords != null && lastObjectCoords.CurrentZoneId == 0xFFFF) /* gloc Object */
			{
				long xdiff = (long)regionGlobalCoords.CurrentZoneX - lastObjectCoords.CurrentZoneX;
				long ydiff = (long)regionGlobalCoords.CurrentZoneY - lastObjectCoords.CurrentZoneY;
				long zdiff = 0;
				if (lastObjectCoords.CurrentZoneZ != 0xFFFF)
				{
					zdiff = (long)regionGlobalCoords.CurrentZoneZ - lastObjectCoords.CurrentZoneZ;
				}
				int range = (int)Math.Sqrt(xdiff * xdiff + ydiff * ydiff + zdiff * zdiff);
				str.Insert(0, string.Format("range from you:{0} (gloc/gloc)\n", range));
			}
			if (objectId != 0xFFFF)
				str.Insert(0, string.Format("\nobjectId:0x{0:X4}{1}\n", objectId, objectType));
			return str.ToString();
		}

		private string MakeSidInfo(PacketLog log, int selectedIndex, ushort sessionId)
		{
			StringBuilder str = new StringBuilder();
			bool fullInfoFound = false;
//			bool moveFound = false;
			str.AppendFormat("\nsessionId:0x{0:X4}\n", sessionId);
			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0x20_PlayerPositionAndObjectID) // stop scanning packets on enter region
				{
					break; // Not found player initialize in current region
				}
				else if (pak is StoC_0xD4_PlayerCreate)
				{
					if ((pak as StoC_0xD4_PlayerCreate).SessionId == sessionId)
					{
						fullInfoFound = true;
						str.Append(MakeOidInfo(log, selectedIndex, (pak as StoC_0xD4_PlayerCreate).Oid, sessionId, 0));
						break;
					}
				}
				else if (pak is StoC_0x4B_PlayerCreate_172)
				{
					if ((pak as StoC_0x4B_PlayerCreate_172).SessionId == sessionId)
					{
						fullInfoFound = true;
						str.Append(MakeOidInfo(log, selectedIndex, (pak as StoC_0x4B_PlayerCreate_172).Oid, sessionId, 0));
						break;
					}
				}
//				else if (pak is CtoS_0xA9_PlayerPosition)
//				{
//					if (!moveFound && (pak as CtoS_0xA9_PlayerPosition).SessionId == sessionId)
//					{
//						moveFound = true;
//						str.Append(pak.ToHumanReadableString(zeroTimeSpan, true) + '\n');
//					}
//				}
//				else if (pak is StoC_0xA9_PlayerPosition)
//				{
//					if (!moveFound && (pak as StoC_0xA9_PlayerPosition).SessionId == sessionId)
//					{
//						moveFound = true;
//						str.Append(pak.ToHumanReadableString(zeroTimeSpan, true) + '\n');
//					}
//				}
			}
			if (!fullInfoFound)
				str.Append("No more info found\n");
			return str.ToString();
		}

		private string MakeHidInfo(PacketLog log, int selectedIndex, ushort houseId)
		{
			StringBuilder str = new StringBuilder();
			bool flagInteriorFound = false;
			bool flagInsideUpdateFound = false;
			bool flagExteriorFound = false;
			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0x20_PlayerPositionAndObjectID) // stop scanning packets on enter region
				{
					break;
				}
				else if (pak is StoC_0xD1_HouseCreate)
				{
					if ((pak as StoC_0xD1_HouseCreate).HouseId == houseId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						break;
					}
				}
				else if (pak is StoC_0xD2_HouseUpdate)
				{
					if (!flagExteriorFound && (pak as StoC_0xD2_HouseUpdate).HouseId == houseId && (pak as StoC_0xD2_HouseUpdate).UpdateCode == 0x80)
					{
						flagExteriorFound = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (pak is StoC_0x09_HouseDecorationUpdate)
				{
					if (!flagInteriorFound && (pak as StoC_0x09_HouseDecorationUpdate).HouseId == houseId && (pak as StoC_0x09_HouseDecorationUpdate).DecorationCode == 0x80)
					{
						flagInteriorFound = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
				else if (pak is StoC_0x08_HouseInsideUpdate)
				{
					if (!flagInsideUpdateFound && (pak as StoC_0x08_HouseInsideUpdate).HouseId == houseId)
					{
						flagInsideUpdateFound = true;
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
			}
			str.Insert(0, string.Format("\nhouseId:0x{0:X4}\n", houseId));
			return str.ToString();
		}

		private string MakeKidInfo(PacketLog log, int selectedIndex, ushort keepId)
		{
			StringBuilder str = new StringBuilder();
			for (int i = selectedIndex; i >= 0; i--)
			{
				Packet pak = log[i];
				if (pak is StoC_0x20_PlayerPositionAndObjectID) // stop scanning packets on enter region
				{
					break;
				}
				else if (pak is StoC_0x69_KeepOverview)
				{
					if ((pak as StoC_0x69_KeepOverview).KeepId == keepId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
						break;
					}
				}
				else if (pak is StoC_0x6C_KeepComponentOverview)
				{
					if ((pak as StoC_0x6C_KeepComponentOverview).KeepId == keepId)
					{
						str.Insert(0, pak.ToHumanReadableString(TimeSpan.Zero, true) + '\n');
					}
				}
			}
			str.Insert(0, string.Format("\nkeepId:0x{0:X4}\n", keepId));
			return str.ToString();
		}

		private class LocalCoords
		{
			public int CurrentZoneX;
			public int CurrentZoneY;
			public int CurrentZoneZ;
			public int CurrentZoneId;
			public LocalCoords(ushort x, ushort y, ushort z, ushort zoneId)
			{
				CurrentZoneX = x;
				CurrentZoneY = y;
				CurrentZoneZ = z;
				CurrentZoneId = zoneId;
			}
			public LocalCoords(uint x, uint y, ushort z)
			{
				CurrentZoneX = (int)x;
				CurrentZoneY = (int)y;
				CurrentZoneZ = z;
				CurrentZoneId = 0xFFFF;
			}
			public LocalCoords(uint x, uint y)
			{
				CurrentZoneX = (int)x;
				CurrentZoneY = (int)y;
				CurrentZoneZ = 0xFFFF;
				CurrentZoneId = 0xFFFF;
			}
		}
	}
}
