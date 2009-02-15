using System;
using System.IO;
using System.Windows.Forms;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Shows all packets which contain OID of controlled pet.
	/// </summary>
	[LogFilter("Controlled pet dynamic OID packets", Shortcut.CtrlC, Priority=250)]
	public class ControlledPetDynamicOidFilter : AbstractDynamicOIDFilter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ControlledPetDynamicOidFilter"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public ControlledPetDynamicOidFilter(IExecutionContext context) : base(context)
		{
		}

		/// <summary>
		/// Reads information from all usefull packets.
		/// </summary>
		/// <param name="packet">The packet.</param>
		protected override void FilterManager_OnFilteringPacketEvent(Packet packet)
		{
			if (packet is StoC_0x88_PetWindowUpdate)
			{
				StoC_0x88_PetWindowUpdate petWindow = (StoC_0x88_PetWindowUpdate)packet;
				Oid = petWindow.PetId;
			}
		}
	}
}
