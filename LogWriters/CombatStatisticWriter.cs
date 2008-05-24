using System.Collections;
using System.IO;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes combat statistic
	/// </summary>
	[LogWriter("Combat statistics", "*.txt")]
	public class TestCombatActionWriter : ILogWriter
	{
		protected StoC_0x02_InventoryUpdate.Item[] weapons;
		protected int VisibleSlots = 0xFF;
		protected static StoC_0x02_InventoryUpdate.Item newitem = new StoC_0x02_InventoryUpdate.Item();
		public enum eObjectType: byte
		{
			player = 0,
			npc = 1,
			staticObject = 2,
			keep = 3,
			movingObject = 4,
			door = 5
		}

		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			SortedList oidInfo = new SortedList();
			IList CombatWaitMessage = new ArrayList();
			Hashtable styleIcons = new Hashtable();
			Hashtable plrInfo = new Hashtable();
			weapons = new StoC_0x02_InventoryUpdate.Item[4];
			PlayerInfo playerInfo = null;// = new PlayerInfo();

			int playerOid = -1;
			string nameKey = "";
			string statKey = "";
			string plrName = "";
			string plrClass = "";
			int plrLevel = 0;
			int countBC = 0;
			using (StreamWriter s = new StreamWriter(stream))
			{
				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i, log.Count - 1);

						Packet pak = log[i];
						// Enter region (get new self OID)
						if (pak is StoC_0x20_PlayerPositionAndObjectID)
						{
							StoC_0x20_PlayerPositionAndObjectID plr = (StoC_0x20_PlayerPositionAndObjectID) pak;
							playerOid = plr.PlayerOid;
							oidInfo.Clear();
							oidInfo[plr.PlayerOid] = new ObjectInfo(eObjectType.player, "You", 0);
							s.WriteLine("{0, -16} playerOid:0x{1:X4}", pak.Time.ToString(), playerOid);
						}
							// Fill objects OID
						else if (pak is StoC_0xD4_PlayerCreate)
						{
							StoC_0xD4_PlayerCreate player = (StoC_0xD4_PlayerCreate) pak;
							oidInfo[player.Oid] = new ObjectInfo(eObjectType.player, player.Name, player.Level);
						}
						else if (pak is StoC_0x4B_PlayerCreate_172)
						{
							StoC_0x4B_PlayerCreate_172 player = (StoC_0x4B_PlayerCreate_172) pak;
							oidInfo[player.Oid] = new ObjectInfo(eObjectType.player, player.Name, player.Level);
						}
						else if (pak is StoC_0x12_CreateMovingObject)
						{
							StoC_0x12_CreateMovingObject obj = (StoC_0x12_CreateMovingObject) pak;
							oidInfo[obj.ObjectOid] = new ObjectInfo(eObjectType.movingObject, obj.Name, 0);
						}
						else if (pak is StoC_0x6C_KeepComponentOverview)
						{
							StoC_0x6C_KeepComponentOverview keep = (StoC_0x6C_KeepComponentOverview) pak;
							oidInfo[keep.Uid] =
								new ObjectInfo(eObjectType.keep, string.Format("keepId:0x{0:X4} componentId:{1}", keep.KeepId, keep.ComponentId),
								               0);
						}
						else if (pak is StoC_0xDA_NpcCreate)
						{
							StoC_0xDA_NpcCreate npc = (StoC_0xDA_NpcCreate) pak;
							oidInfo[npc.Oid] = new ObjectInfo(eObjectType.npc, npc.Name, npc.Level);
						}
						else if (pak is StoC_0xD9_ItemDoorCreate)
						{
							StoC_0xD9_ItemDoorCreate item = (StoC_0xD9_ItemDoorCreate) pak;
							eObjectType type = eObjectType.staticObject;
							if (item.ExtraBytes > 0) type = eObjectType.door;
							oidInfo[item.Oid] = new ObjectInfo(type, item.Name, 0);
						}
							// Fill current weapons
						else if (pak is StoC_0x02_InventoryUpdate)
						{
							StoC_0x02_InventoryUpdate invPack = (StoC_0x02_InventoryUpdate) pak;
							if (invPack.PreAction == 1 || invPack.PreAction == 0)
							{
								VisibleSlots = invPack.VisibleSlots;
								for (int j = 0; j < invPack.SlotsCount; j++)
								{
									StoC_0x02_InventoryUpdate.Item item = (StoC_0x02_InventoryUpdate.Item) invPack.Items[j];
									switch (item.slot)
									{
										case 10:
											weapons[0] = item;
											break;
										case 11:
											weapons[1] = item;
											break;
										case 12:
											weapons[2] = item;
											break;
										case 13:
											weapons[3] = item;
											break;
										default:
											break;
									}
								}
							}
						}
							// Fill character stats
						else if (pak is StoC_0x16_VariousUpdate)
						{
							// name, level, class
							StoC_0x16_VariousUpdate stat = (StoC_0x16_VariousUpdate) pak;
							if (stat.SubCode == 3)
							{
								StoC_0x16_VariousUpdate.PlayerUpdate subData = (StoC_0x16_VariousUpdate.PlayerUpdate) stat.SubData;
								nameKey = "N:" + subData.playerName + "L:" + subData.playerLevel;
								statKey = "";
								plrName = subData.playerName;
								plrLevel = subData.playerLevel;
								plrClass = subData.className;
								s.WriteLine("{0, -16} 0x16:3 nameKey:{1} plrName:{2} {3} {4}", pak.Time.ToString(), nameKey, plrName, plrLevel,
								            plrClass);
							}
								// mainhand spec, mainhand DPS
							else if (stat.SubCode == 5)
							{
								StoC_0x16_VariousUpdate.PlayerStateUpdate subData = (StoC_0x16_VariousUpdate.PlayerStateUpdate) stat.SubData;
								string key =
									string.Format("WD:{0}.{1}WS:{2}", subData.weaponDamageHigh, subData.weaponDamageLow,
									              (subData.weaponSkillHigh << 8) + subData.weaponSkillLow);
								if (nameKey != "")
								{
									if (plrInfo.ContainsKey(nameKey + key))
									{
										playerInfo = (PlayerInfo) plrInfo[nameKey + key];
									}
									else
									{
										playerInfo = new PlayerInfo();
										playerInfo.name = plrName;
										playerInfo.level = plrLevel;
										playerInfo.className = plrClass;
										playerInfo.weaponDamage = string.Format("{0,2}.{1,-3}", subData.weaponDamageHigh, subData.weaponDamageLow);
										playerInfo.weaponSkill = (subData.weaponSkillHigh << 8) + subData.weaponSkillLow;
										plrInfo.Add(nameKey + key, playerInfo);
									}
									plrInfo[nameKey + key] = playerInfo;
								}
								statKey = key;
								s.WriteLine("{0, -16} 0x16:5 S:{1} {2} {3} {4} {5}", pak.Time.ToString(), statKey, playerInfo.name,
								            playerInfo.level, playerInfo.weaponDamage, playerInfo.weaponSkill);
							}
							// Fill styles
							if (stat.SubCode == 1)
							{
								StoC_0x16_VariousUpdate.SkillsUpdate subData = (StoC_0x16_VariousUpdate.SkillsUpdate) stat.SubData;
								styleIcons.Clear();
								if (log.Version < 186)
								{
									styleIcons.Add((ushort)0x01F4, "Bow prepare");
									styleIcons.Add((ushort)0x01F5, "Lefthand hit");
									styleIcons.Add((ushort)0x01F6, "Bothhands hit");
									styleIcons.Add((ushort)0x01F7, "Bow shoot");
//									styleIcons.Add((ushort)0x01F9, "Volley aim ?");
//									styleIcons.Add((ushort)0x01FA, "Volley ready ?");
//									styleIcons.Add((ushort)0x01FB, "Volley shoot ?");
								}
								else
								{
									styleIcons.Add((ushort)0x3E80, "Bow prepare");
									styleIcons.Add((ushort)0x3E81, "Lefthand hit");
									styleIcons.Add((ushort)0x3E82, "Bothhands hit");
									styleIcons.Add((ushort)0x3E83, "Bow shoot");
//									styleIcons.Add((ushort)0x3E85, "Volley aim ?");
//									styleIcons.Add((ushort)0x3E86, "Volley ready ?");
//									styleIcons.Add((ushort)0x3E87, "Volley shoot ?");
								}
								foreach (StoC_0x16_VariousUpdate.Skill skill in subData.data)
								{
									if (skill.page == StoC_0x16_VariousUpdate.eSkillPage.Styles)
									{
										styleIcons[skill.icon] = skill.name;
//										s.WriteLine("{0, -16} 0x16:1 icon:0x{1:X4} name:{2}", pak.Time.ToString(), skill.icon, styleIcons[skill.icon]);
									}
								}
/* 								foreach (DictionaryEntry entry in styleIcons)
								{
									ushort icon = (ushort)entry.Key;
									s.WriteLine("{0, -16} 0x16:1 icon:0x{1:X4} name:{2}", pak.Time.ToString(), icon, entry.Value);
								}*/
							}
						}
						// Combat animation
						else if (pak is StoC_0xBC_CombatAnimation && (playerInfo != null))
						{
							StoC_0xBC_CombatAnimation combat = (StoC_0xBC_CombatAnimation) pak;
							CombatWaitMessage.Clear();
							ObjectInfo targetObj = oidInfo[combat.DefenderOid] as ObjectInfo;
							string styleName = (combat.StyleId == 0 /*  || (combat.Result & 0x7F) != 0x0B)*/)
							                   	? ""
							                   	: (styleIcons[combat.StyleId] == null
							                   	   	? "not found " + combat.StyleId.ToString()
							                   	   	: (styleIcons[combat.StyleId]).ToString());
							string targetName = targetObj == null ? "" : " target:" + targetObj.name + " (" + targetObj.type + ")";
							if (combat.Stance == 0 && combat.AttackerOid == playerOid /* && combat.DefenderOid != 0*/)
							{
								switch (combat.Result & 0x7F)
								{
									case 0:
										CombatWaitMessage.Add(new WaitMessage(0x11, "You miss!"));
										CombatWaitMessage.Add(new WaitMessage(0x11, "You were strafing in combat and miss!"));
										break;
									case 1:
										if (targetObj != null)
											CombatWaitMessage.Add(new WaitMessage(0x11, targetObj.GetFirstFullName + " parries your attack!"));
										break;
									case 2:
//										if (targetObj != null)//TODO
//											CombatWaitMessage.Add(new WaitMessage(0x11, targetObj.GetFirstFullName + " The Midgardian Assassin attacks you and you block the blow!"));
										break;

									case 3:
										if (targetObj != null)
											CombatWaitMessage.Add(new WaitMessage(0x11, targetObj.GetFirstFullName + " evades your attack!"));
										break;
									case 4:
										CombatWaitMessage.Add(new WaitMessage(0x11, "You fumble the attack and take time to recover!"));
										break;
									case 0xA:
										if (targetObj != null)
											CombatWaitMessage.Add(
												new WaitMessage(0x11, "You attack " + targetObj.GetFullName + " with your % and hit for % damage!"));
										break;
									case 0xB:
										CombatWaitMessage.Add(new WaitMessage(0x11, "You perform your " + styleName + " perfectly. %"));
										if (targetObj != null)
										{
											CombatWaitMessage.Add(
												new WaitMessage(0x11, "You attack " + targetObj.GetFullName + " with your % and hit for % damage!"));
										}
										break;
									case 0x14:
										if (targetObj != null)
											CombatWaitMessage.Add(new WaitMessage(0x11, "You hit " + targetObj.GetFullName + " for % damage!"));
										break;
									default:
										break;
								}
							}
							if (combat.AttackerOid == playerOid)
							{
								s.WriteLine("{0, -16} 0xBC attackerOid:0x{1:X4}(You) defenderOid:0x{2:X4} style:0x{4:X4} result:0x{3:X2}{5}{6}",
								            pak.Time.ToString(), combat.AttackerOid, combat.DefenderOid, combat.Result, combat.StyleId,
								            styleName == "" ? "" : " styleName:" + styleName, targetName);
								foreach (WaitMessage msg in CombatWaitMessage)
								{
									s.WriteLine("         WAITING 0xAF 0x{0:X2} {1}", msg.msgType, msg.message);
								}
								countBC++;
							}
						}
							// Messages
						else if (pak is StoC_0xAF_Message)
						{
							StoC_0xAF_Message msg = (StoC_0xAF_Message) pak;
							switch (msg.Type)
							{
//								case 0x10: // Your cast combat
								case 0x11: // Your Melee combat
//								case 0x1B: // resist
//								case 0x1D: // X hits you
//								case 0x1E: // X miss you
									s.WriteLine("{0, -16} 0xAF 0x{1:X2} {2} ", pak.Time.ToString(), msg.Type, msg.Text);
									break;
								default:
									break;
							}
						}
					}
				}
				if (nameKey != "" && statKey != "")
					plrInfo[nameKey + statKey] = playerInfo;
			}
		}

		public StoC_0x02_InventoryUpdate.Item GetMainWeapon()
		{
			StoC_0x02_InventoryUpdate.Item item = newitem;
			for (int i = 0; i < 4; i++)
			{
				if ((i == (VisibleSlots & 0x0F)) || (i == ((VisibleSlots >> 4) & 0x0F)))
				{
					item = (StoC_0x02_InventoryUpdate.Item)weapons[i];
//					if (item.objectType == 42)//shield
//						str.AppendFormat("slot:{0,-2} level:{1,-2} size:{2} con:{3,-3} qual:{4,-3} bonus:{5,-2} model:0x{6:X4} \"{7}\"\n",
//						item.slot, item.level, item.value1, item.condition, item.quality, item.bonus, item.model, item.name);
//					else
//						str.AppendFormat("slot:{0,-2} level:{1,-2} dps:{2:0.00} spd:{3:0.00} damageType:{4} con:{5,-3} qual:{6,-3} bonus:{7,-2} model:0x{8:X4} \"{9}\"\n",
//						item.slot, item.level, 0.1 * item.value1, 0.1 * item.value2, item.damageType, item.condition, item.quality, item.bonus, item.model, item.name);
				}
			}
			return item;
		}

		public StoC_0x02_InventoryUpdate.Item GetLeftHandWeapon()
		{
			StoC_0x02_InventoryUpdate.Item item = newitem;
			for (int i = 0; i < 4; i++)
			{
				if ((i == (VisibleSlots & 0x0F)) || (i == ((VisibleSlots >> 4) & 0x0F)))
				{
					item = (StoC_0x02_InventoryUpdate.Item)weapons[i];
//					if (item.objectType == 42)//shield
//						str.AppendFormat("slot:{0,-2} level:{1,-2} size:{2} con:{3,-3} qual:{4,-3} bonus:{5,-2} model:0x{6:X4} \"{7}\"\n",
//						item.slot, item.level, item.value1, item.condition, item.quality, item.bonus, item.model, item.name);
//					else
//						str.AppendFormat("slot:{0,-2} level:{1,-2} dps:{2:0.00} spd:{3:0.00} damageType:{4} con:{5,-3} qual:{6,-3} bonus:{7,-2} model:0x{8:X4} \"{9}\"\n",
//						item.slot, item.level, 0.1 * item.value1, 0.1 * item.value2, item.damageType, item.condition, item.quality, item.bonus, item.model, item.name);
				}
			}
			return item;
		}

		public class ObjectInfo
		{
			public eObjectType type;
			public byte level;
			public string name;

			public ObjectInfo(eObjectType type, string name, byte level)
			{
				this.type = type;
				this.name = name;
				this.level = level;
			}

			public string GetFullName
			{
				get
				{
					if (type != eObjectType.player)
						return "the " + name;
					else
						return name;
				}
			}

			public string GetFirstFullName
			{
				get
				{
					if (type != eObjectType.player)
						return "The " + name;
					else
						return name;
				}
			}
		}

		public class WaitMessage
		{
			public byte msgType;
			public string message;

			public WaitMessage(byte msgType, string message)
			{
				this.msgType = msgType;
				this.message = message;
			}
		}

		public class PlayerInfo
		{
			public string className;
			public int level;
			public string name;
			public string weaponDamage;
			public int weaponSkill;
			public int hit;
			public int miss;
			public int fumble;
			public int failAttacks;
			public int parry;
			public int block;
			public int evade;
			public int attacked;
		}
	}
}
