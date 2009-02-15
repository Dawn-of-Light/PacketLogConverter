using System;
using System.IO;
using System.Windows.Forms;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Shows all packets which contain OID of user who logged the packets.
	/// </summary>
	[LogFilter("Self dynamic SID/OID packets", Shortcut.CtrlD, Priority=200)]
	public class SelfDynamicOidFilter : AbstractDynamicOIDFilter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:SelfDynamicOidFilter"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public SelfDynamicOidFilter(IExecutionContext context) : base(context)
		{
		}

		/// <summary>
		/// Reads information from all usefull packets.
		/// </summary>
		/// <param name="packet">The packet.</param>
		protected override void FilterManager_OnFilteringPacketEvent(Packet packet)
		{
			//
			// Set OID
			//
			if (packet is StoC_0x20_PlayerPositionAndObjectID)
			{
				StoC_0x20_PlayerPositionAndObjectID posAndOid = (StoC_0x20_PlayerPositionAndObjectID)packet;
				Oid = posAndOid.PlayerOid;
			}
			else if (packet is StoC_0xDE_SetObjectGuildId)
			{
				StoC_0xDE_SetObjectGuildId guildId = (StoC_0xDE_SetObjectGuildId)packet;
				if (guildId.ServerId == 0xFF)
					Oid = guildId.Oid;
			}
//			else if (packet is StoC_0x04_CharacterJump)
//			{
//				StoC_0x04_CharacterJump plrJump = (StoC_0x04_CharacterJump)packet;
//				m_oid = plrJump.PlayerOid;
//			}

			//
			// Set session ID
			//
			else if (packet is StoC_0x28_SetSessionId)
			{
				StoC_0x28_SetSessionId sessionPack = (StoC_0x28_SetSessionId)packet;
				Sid = sessionPack.SessionId;
			}
		}
	}
}
