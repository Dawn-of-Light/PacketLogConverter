using System;
using System.IO;
using System.Windows.Forms;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Shows all packets which contain OID of user who logged the packets.
	/// </summary>
	[LogFilter("Self dynamic OID packets", Shortcut.CtrlD, Priority=200)]
	public class SelfDynamicOidFilter : AbstractDynamicOIDFilter
	{
		/// <summary>
		/// Reads information from all usefull packets.
		/// </summary>
		/// <param name="packet">The packet.</param>
		protected override void FilterManager_OnFilteringPacketEvent(Packet packet)
		{
			if (packet is StoC_0x20_PlayerPositionAndObjectID)
			{
				StoC_0x20_PlayerPositionAndObjectID posAndOid = (StoC_0x20_PlayerPositionAndObjectID)packet;
				Oid = posAndOid.PlayerOid;
			}
//			else if (packet is StoC_0x04_CharacterJump)
//			{
//				StoC_0x04_CharacterJump plrJump = (StoC_0x04_CharacterJump)packet;
//				m_oid = plrJump.PlayerOid;
//			}
		}
	}
}
