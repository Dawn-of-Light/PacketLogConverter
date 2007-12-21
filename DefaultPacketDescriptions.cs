namespace PacketLogConverter
{
	/// <summary>
	/// Summary description for DefaultPacketDescriptions.
	/// </summary>
	public sealed class DefaultPacketDescriptions
	{
	
		private readonly static string[] m_ctosDescriptions = new string[Packet.MAX_CODE];
		private readonly static string[] m_stocDescriptions = new string[Packet.MAX_CODE];

		static DefaultPacketDescriptions()
		{
			InitDescriptions();
		}

		public static string GetDescription(int code, ePacketDirection dir)
		{
			if (dir == ePacketDirection.ClientToServer)
				return m_ctosDescriptions[code].ToLower();
			return m_stocDescriptions[code].ToLower();
		}

		#region Init descriptions

		private static void InitDescriptions()
		{
			m_ctosDescriptions[0x00] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x01] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x03] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x05] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x06] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x07] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x0B] = "Unknown Packet";
			m_ctosDescriptions[0x0C] = "Unknown Packet";
			m_ctosDescriptions[0x0D] = "Unknown Packet";
			m_ctosDescriptions[0x0E] = "Unknown Packet";
			m_ctosDescriptions[0x10] = "Select character";
			m_ctosDescriptions[0x11] = "Unknown Packet";
			m_ctosDescriptions[0x13] = "Unknown Packet";
			m_ctosDescriptions[0x14] = "UDP init request";
			m_ctosDescriptions[0x18] = "Unknown Packet (housing)";
			m_ctosDescriptions[0x1A] = "Unknown Packet";
			m_ctosDescriptions[0x1C] = "Unknown Packet";
			m_ctosDescriptions[0x1D] = "Unknown Packet";
			m_ctosDescriptions[0x2B] = "World sent?";
			m_ctosDescriptions[0x37] = "Client crash";
			m_ctosDescriptions[0x71] = "Use item (pull to quickbar and click)";
			m_ctosDescriptions[0x74] = "Enter/Leave attack mode";
			m_ctosDescriptions[0x76] = "Unknown Packet (GS_SEND_REMOVE_CONCENTRATION)";
			m_ctosDescriptions[0x78] = "Buy from merchant";
			m_ctosDescriptions[0x79] = "Sell item";
			m_ctosDescriptions[0x7A] = "Interact (right click)";
			m_ctosDescriptions[0x7C] = "Train skill or realm ability";
			m_ctosDescriptions[0x7D] = "Cast spell (click on spell icon)";
			m_ctosDescriptions[0x80] = "Destroy inventory item?";
			m_ctosDescriptions[0x82] = "Dialog box response";
			m_ctosDescriptions[0x84] = "Looking for group flag";
			m_ctosDescriptions[0x85] = "Find group/player";
			m_ctosDescriptions[0x87] = "Invite to group";
			m_ctosDescriptions[0x8A] = "Command the pet";
			m_ctosDescriptions[0x90] = "Zone change request";
			m_ctosDescriptions[0x98] = "Accept group invite";
			m_ctosDescriptions[0x99] = "Operate door";
			m_ctosDescriptions[0x9A] = "Unknown Packet (GS_REQUEST_ARENA)";
			m_ctosDescriptions[0x9D] = "Region list request";
			m_ctosDescriptions[0x9F] = "Disband from group";
			m_ctosDescriptions[0xA3] = "Ping Server";
			m_ctosDescriptions[0xA5] = "Request static world objects?";
			m_ctosDescriptions[0xA7] = "Login Request";
			m_ctosDescriptions[0xA9] = "Player position update";
			m_ctosDescriptions[0xAC] = "Unknown Packet (GS_F_PLAYER_EXIT)";
			m_ctosDescriptions[0xAF] = "Player Chat/Command";
			m_ctosDescriptions[0xB0] = "Target change";
			m_ctosDescriptions[0xB5] = "Pick up object";
			m_ctosDescriptions[0xB8] = "Disconnect";
			m_ctosDescriptions[0xB9] = "Unknown Packet";
			m_ctosDescriptions[0xBA] = "Player heading update";
			m_ctosDescriptions[0xBB] = "Use skill (click on skill icon)";
			m_ctosDescriptions[0xBE] = "Request missing mob";
			m_ctosDescriptions[0xBF] = "Game open request";
			m_ctosDescriptions[0xC0] = "Character delete";
			m_ctosDescriptions[0xC2] = "Character name bad check";
			m_ctosDescriptions[0xC7] = "Sit down / Stand up";
			m_ctosDescriptions[0xC8] = "Dismount";
			m_ctosDescriptions[0xCA] = "Unknown Packet";
			m_ctosDescriptions[0xCB] = "Character name dublicate check";
			m_ctosDescriptions[0xCD] = "Character creation confirmation";
			m_ctosDescriptions[0xD0] = "Unknown Packet";
			m_ctosDescriptions[0xD4] = "World init request";
			m_ctosDescriptions[0xD5] = "Request missing player";
			m_ctosDescriptions[0xD8] = "Spell delve request";
			m_ctosDescriptions[0xDC] = "Update combat message filter";
			m_ctosDescriptions[0xDD] = "Move inventory item";
			m_ctosDescriptions[0xE0] = "Appraise item";
			m_ctosDescriptions[0xE2] = "Choose emblem";
			m_ctosDescriptions[0xE8] = "Player world initialize request";
			m_ctosDescriptions[0xEB] = "Modify trade window (add/remove items)";
			m_ctosDescriptions[0xEC] = "Set ground target";
			m_ctosDescriptions[0xED] = "Display time bar (crafting, diving, release timer)";
			m_ctosDescriptions[0xF2] = "UDP Ping";
			m_ctosDescriptions[0xF4] = "Encryption request";
			m_ctosDescriptions[0xF5] = "Unknown Packet";
			m_ctosDescriptions[0xF8] = "Unknown Packet (GS_SEND_REMOVE_EFFECT)";
			m_ctosDescriptions[0xFC] = "Character overview request";
			m_ctosDescriptions[0xFF] = "Create character";

			m_stocDescriptions[0x02] = "update inventory or Vaultkeeper";
			m_stocDescriptions[0x03] = "House permission list?";
			m_stocDescriptions[0x04] = "Player Jump";
			m_stocDescriptions[0x05] = "House freind level permission?";
			m_stocDescriptions[0x12] = "Create Moving Object";
			m_stocDescriptions[0x13] = "ML Quest Update";
			m_stocDescriptions[0x15] = "Object Equipment Update";
			m_stocDescriptions[0x16] = "Player Stats Update(subcodes)";
//			m_stocDescriptions[0x16] = "Player Stats Update(spec,abil,styles,spells,song,RA)";
//			m_stocDescriptions[0x16] = "Player Stats Update(spell trees)";
//			m_stocDescriptions[0x16] = "Player Stats Update(name classe)";
//			m_stocDescriptions[0x16] = "Player Stats Update(combats stats)";
//			m_stocDescriptions[0x16] = "Player Stats Update(group window)";
//			m_stocDescriptions[0x16] = "Player Stats Update(crafting skill)";
			m_stocDescriptions[0x17] = "Merchant Window";
			m_stocDescriptions[0x18] = "Rotate object?";
			m_stocDescriptions[0x1B] = "Spell Effect Animation";
			m_stocDescriptions[0x1F] = "Search Explorer Result";
			m_stocDescriptions[0x20] = "Set Player position and object id";
			m_stocDescriptions[0x21] = "Set Debug Mode";
			m_stocDescriptions[0x22] = "Send Version And CryptKey";
			m_stocDescriptions[0x23] = "GS_ERROR_LOGON?";
			m_stocDescriptions[0x28] = "SessionID";
			m_stocDescriptions[0x29] = "ping reply";
			m_stocDescriptions[0x2A] = "login granted";
			m_stocDescriptions[0x2B] = "Player Init Finished";
			m_stocDescriptions[0x2C] = "LoginDenied";
			m_stocDescriptions[0x2D] = "Send Game Open Reply";
			m_stocDescriptions[0x2F] = "UDP init reply";
			m_stocDescriptions[0x4B] = "Request missing player response";
			m_stocDescriptions[0x4C] = "Visual Effect on Living";
			m_stocDescriptions[0x5C] = "Set XFire status";
			m_stocDescriptions[0x5F] = "Set roleplay status?";
			m_stocDescriptions[0x61] = "Repair cockpit + keep upgrade?";
			m_stocDescriptions[0x62] = "Keep lord?";
			m_stocDescriptions[0x63] = "Siege store?";
			m_stocDescriptions[0x67] = "Keep/Tower update";
			m_stocDescriptions[0x69] = "Keep/Tower overview";
			m_stocDescriptions[0x6C] = "Keep/Tower component";
			m_stocDescriptions[0x6D] = "Keep/Tower component update";
			m_stocDescriptions[0x70] = "Player Group Update";
			m_stocDescriptions[0x72] = "Spell Cast Animation";
			m_stocDescriptions[0x73] = "Spell Cast Animation Interrupt";
			m_stocDescriptions[0x74] = "Player Attack Mode";
			m_stocDescriptions[0x75] = "Set concentration list";
			m_stocDescriptions[0x7B] = "Trainer Window";
			m_stocDescriptions[0x7E] = "Set Time";
			m_stocDescriptions[0x7F] = "Display Self Buff";
			m_stocDescriptions[0x81] = "show dialogue box";
			m_stocDescriptions[0x83] = "quest list init And update";
			m_stocDescriptions[0x86] = "Find group membres window Update";
			m_stocDescriptions[0x88] = "Pet window";
			m_stocDescriptions[0x89] = "revive";
			m_stocDescriptions[0x8C] = "GS_DELETE_ARENA?";
			m_stocDescriptions[0x8D] = "Player Model Change";
			m_stocDescriptions[0x8F] = "GS_INIT_SPELL?";
			m_stocDescriptions[0x91] = "points update";
			m_stocDescriptions[0x92] = "Weather";
			m_stocDescriptions[0x99] = "GS_F_OPEN_DOOR";
			m_stocDescriptions[0x9E] = "region init";
			m_stocDescriptions[0xA1] = "Update Mob";
			m_stocDescriptions[0xA2] = "Object removal";
			m_stocDescriptions[0xA4] = "Player Quit";
			m_stocDescriptions[0xA9] = "Player Position Update";
			m_stocDescriptions[0xAA] = "Create Object?";
			m_stocDescriptions[0xAB] = "Player Death?";
			m_stocDescriptions[0xAC] = "destroy?";
			m_stocDescriptions[0xAD] = "Self health Update And Sit Stand";
			m_stocDescriptions[0xAE] = "Death";
			m_stocDescriptions[0xAF] = "Console Message";
			m_stocDescriptions[0xB0] = "Unknown Packet";
			m_stocDescriptions[0xB1] = "Start Arena?";
			m_stocDescriptions[0xB2] = "WORLD_STATE?";
			m_stocDescriptions[0xB6] = "Speed Update";
			m_stocDescriptions[0xB7] = "Region Change (Jump Spots)";
			m_stocDescriptions[0xBA] = "Player Short State?";
			m_stocDescriptions[0xBC] = "combat swing";
			m_stocDescriptions[0xBD] = "Encumberance Update";
			m_stocDescriptions[0xC3] = "NameChecktwo And Response";
			m_stocDescriptions[0xC4] = "Detail window Display - Server";
			m_stocDescriptions[0xC5] = "Add Friends";
			m_stocDescriptions[0xC6] = "Remove Friends";
			m_stocDescriptions[0xC8] = "ride";
			m_stocDescriptions[0xC9] = "GS_F_PLAY_SOUND?";
			m_stocDescriptions[0xCC] = "NameCheckOne And Response";
			m_stocDescriptions[0xD0] = "Unknown Packet";
			m_stocDescriptions[0xD1] = "House Creation";
			m_stocDescriptions[0xD2] = "House Update";
			m_stocDescriptions[0xD3] = "Unknown Packet";
			m_stocDescriptions[0xD4] = "Player Creation";
			m_stocDescriptions[0xD5] = "GS_INIT_PLAYER_REQUEST?";
			m_stocDescriptions[0xD6] = "Disable Ability";
			m_stocDescriptions[0xD9] = "Object Creation";
			m_stocDescriptions[0xDA] = "Mob Creation";
			m_stocDescriptions[0xDB] = "Player Model change";
			m_stocDescriptions[0xDC] = "Unknown Packet";
			m_stocDescriptions[0xDE] = "SendPlayerGuildID";
			m_stocDescriptions[0xDF] = "Ground Target Assist Reply";
			m_stocDescriptions[0xE1] = "SendObjectDelete";
			m_stocDescriptions[0xE2] = "Unknown Packet";
			m_stocDescriptions[0xEA] = "Trade Window";
			m_stocDescriptions[0xEC] = "Unknown Packet";
			m_stocDescriptions[0xED] = "GS_PRODUCT?";
			m_stocDescriptions[0xEE] = "GS_F_PLAYER_STATS?";
			m_stocDescriptions[0xEF] = "GS_F_TIMER_DIALOG?";
			m_stocDescriptions[0xF0] = "Char create reply";
			m_stocDescriptions[0xF1] = "GS_F_SEND_CHAR_ERROR?";
			m_stocDescriptions[0xF3] = "timer for crafting";
			m_stocDescriptions[0xF5] = "Unknown Packet";
			m_stocDescriptions[0xF6] = "Change Target";
			m_stocDescriptions[0xF9] = "Player Animation";
			m_stocDescriptions[0xFA] = "money update";
			m_stocDescriptions[0xFB] = "character stats update";
			m_stocDescriptions[0xFD] = "request char reponse Char Details";
			m_stocDescriptions[0xFE] = "Set realm";

			for (int i = 0; i < Packet.MAX_CODE; i++)
			{
				string stocDesc = PacketManager.GetPacketDescription(-1, i, ePacketDirection.ServerToClient);
				string ctosDesc = PacketManager.GetPacketDescription(-1, i, ePacketDirection.ClientToServer);

				if (stocDesc != null)
				{
					m_stocDescriptions[i] = stocDesc;
				}
				else if (m_stocDescriptions[i] == null)
				{
					m_stocDescriptions[i] = "Unknown packet 0x"+i.ToString("X2");
				}

				if (ctosDesc != null)
				{
					m_ctosDescriptions[i] = ctosDesc;
				}
				else if (m_ctosDescriptions[i] == null)
				{
					m_ctosDescriptions[i] = "Unknown packet 0x"+i.ToString("X2");
				}
			}
		}

		#endregion
	}
}
