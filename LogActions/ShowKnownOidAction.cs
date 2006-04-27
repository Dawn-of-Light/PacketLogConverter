using System;
using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows all known Oids before selected packet
	/// </summary>
	[LogAction("Show known Oids", Priority=800)]
	public class ShowKnownOidAction : ILogAction
	{
		#region ILogAction Members

		public bool Activate(PacketLog log, int selectedIndex)
		{
			int currentRegion;
			int currentZone;
			SortedList oidInfo = MakeOidList(selectedIndex, log, out currentRegion, out currentZone);

			StringBuilder str = new StringBuilder();
			str.AppendFormat("Info for region {0}, zone {1}\n\n", currentRegion, currentZone);

			foreach (DictionaryEntry entry in oidInfo)
			{
				ushort oid = (ushort)entry.Key;
				ObjectInfo objectInfo = (ObjectInfo)entry.Value;
				str.AppendFormat("0x{0:X4}: {1}\n", oid, objectInfo.ToString());
			}

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Show known Oids (right click to close)";
			infoWindow.Width = 550;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		public static SortedList MakeOidList(int selectedIndex, PacketLog log, out int currentRegion, out int currentZone)
		{
			SortedList oidInfo = new SortedList();
			currentRegion = -1;
			currentZone = -1;
			ushort playerOid = 0;
			string CharName = "UNKNOWN";

			for (int i = 0; i < selectedIndex; ++i)
			{
				Packet pak = log[i];
				if (pak is StoC_0xD4_PlayerCreate)
				{
					StoC_0xD4_PlayerCreate player = (StoC_0xD4_PlayerCreate)pak;

					oidInfo[player.Oid] = new LivingInfo(pak.Time, "PLR", player.Name, player.Level, player.GuildName);
				}
				else if (pak is StoC_0x4B_PlayerCreate_172)
				{
					StoC_0x4B_PlayerCreate_172 player = (StoC_0x4B_PlayerCreate_172)pak;

					oidInfo[player.Oid] = new LivingInfo(pak.Time, "PLR", player.Name, player.Level, player.GuildName);
					if (currentZone == -1)
						currentZone = player.ZoneId;
				}
				else if (pak is StoC_0xA9_PlayerPosition)
				{
					StoC_0xA9_PlayerPosition player = (StoC_0xA9_PlayerPosition)pak;
					if (currentZone == -1)
						currentZone = player.CurrentZoneId;
				}
				else if (pak is CtoS_0xA9_PlayerPosition)
				{
					CtoS_0xA9_PlayerPosition player = (CtoS_0xA9_PlayerPosition)pak;
					if (currentZone == -1)
						currentZone = player.CurrentZoneId;
				}
				else if (pak is StoC_0x12_CreateMovingObject)
				{
					StoC_0x12_CreateMovingObject obj = (StoC_0x12_CreateMovingObject)pak;

					oidInfo[obj.ObjectOid] = new ObjectInfo(pak.Time, "MOVE", obj.Name, 0);
				}
				else if (pak is StoC_0x6C_KeepComponentOverview)
				{
					StoC_0x6C_KeepComponentOverview keep = (StoC_0x6C_KeepComponentOverview)pak;

					oidInfo[keep.Uid] = new ObjectInfo(pak.Time, "Keep", string.Format("keepId:0x{0:X4} componentId:{1}", keep.KeepId, keep.ComponentId), 0);
				}
				else if (pak is StoC_0xD1_HouseCreate)
				{
					StoC_0xD1_HouseCreate house = (StoC_0xD1_HouseCreate)pak;

					oidInfo[house.Oid] = new ObjectInfo(pak.Time, "HOUS", house.Name, 0);
				}
				else if (pak is StoC_0xDA_NpcCreate)
				{
					StoC_0xDA_NpcCreate npc = (StoC_0xDA_NpcCreate)pak;

					oidInfo[npc.Oid] = new LivingInfo(pak.Time, "NPC", npc.Name, npc.Level, npc.GuildName);
				}
				else if (pak is CtoS_0xA9_PlayerPosition)
				{
					CtoS_0xA9_PlayerPosition plr = (CtoS_0xA9_PlayerPosition)pak;
					if (currentZone == -1)
						currentZone = plr.CurrentZoneId;
				}
				else if (pak is StoC_0xD9_ItemDoorCreate)
				{
					StoC_0xD9_ItemDoorCreate item = (StoC_0xD9_ItemDoorCreate)pak;
					string type = "ITEM";
					if (item.ExtraBytes > 0) type = "DOOR";
					oidInfo[item.Oid] = new ObjectInfo(pak.Time, type, item.Name, 0);
				}
				else if (pak is StoC_0xB7_RegionChange)
				{
					StoC_0xB7_RegionChange region = (StoC_0xB7_RegionChange)pak;

//					if (region.RegionId != currentRegion)
//					{
						currentRegion = region.RegionId;
						currentZone = region.ZoneId;
						oidInfo.Clear();
//					}
				}
				else if (pak is StoC_0x20_PlayerPositionAndObjectID_171)
				{
					StoC_0x20_PlayerPositionAndObjectID_171 region = (StoC_0x20_PlayerPositionAndObjectID_171)pak;

//					if (region.Region!= currentRegion)
//					{
						currentRegion = region.Region;
						oidInfo.Clear();
//					}
					playerOid = region.PlayerOid;
					oidInfo[region.PlayerOid] = new LivingInfo(pak.Time, "YOU", CharName, 0, "");
				}
				else if (pak is StoC_0x16_VariousUpdate)
				{
					if (playerOid != 0)
					{
						StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate)pak;
						if (stat.SubCode == 3)
						{
							StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate)stat.SubData;
							if (oidInfo[playerOid] != null)
							{
								LivingInfo plr = (LivingInfo)oidInfo[playerOid];
								plr.level = subData.playerLevel;
								plr.guildName = subData.guildName;
								oidInfo[playerOid] = plr;
							}
						}
					}
				}
				else if (pak is CtoS_0x10_CharacterSelectRequest)
				{
					CtoS_0x10_CharacterSelectRequest login = (CtoS_0x10_CharacterSelectRequest)pak;
					CharName = login.CharName;
				}
			}
			return oidInfo;
		}

		#endregion

		public class ObjectInfo
		{
			public TimeSpan time;
			public string type;
			public string name;
			public byte level;

			public ObjectInfo(TimeSpan time, string type, string name, byte level)
			{
				this.time = time;
				this.type = type;
				this.name = name;
				this.level = level;
			}

			public override string ToString()
			{
				return string.Format("{0}h{1:D2}m{2:D2}s{3:D3}ms  {4,-4}: {5,-2} : \"{6}\"", time.Hours, time.Minutes, time.Seconds, time.Milliseconds, type, level, name);
			}
		}

		public class LivingInfo : ObjectInfo
		{
			public string guildName;

			public LivingInfo(TimeSpan time, string type, string name, byte level, string guildName) : base(time, type, name, level)
			{
				this.guildName = guildName;
			}

			public override string ToString()
			{
				string str = base.ToString();
				if (guildName != null && guildName.Length > 0)
					str += " <" + guildName + ">";
				return str;
			}
		}
	}
}
