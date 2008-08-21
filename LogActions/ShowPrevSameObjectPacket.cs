using System;
using System.Collections;
using System.Text;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Show the raw packet data
	/// </summary>
	[LogAction("Show prev same object packet", Priority=499)]
	public class ShowPrevSameObjectPacket : ShowPrevSamePacket
	{
		public new bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			Packet pak = context.LogManager.GetPacket(selectedPacket);
			if (pak == null)
				return false;
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

		protected override bool IsValidPacket(Packet originalPak, Packet pak)
		{
			if (base.IsValidPacket(originalPak, pak))
			{
				if (pak is IObjectIdPacket && originalPak is IObjectIdPacket && (pak as IObjectIdPacket).ObjectIds.Length > 0  && (originalPak as IObjectIdPacket).ObjectIds.Length > 0 && (pak as IObjectIdPacket).ObjectIds[0] == (originalPak as IObjectIdPacket).ObjectIds[0])
					return true;
				else if (pak is ISessionIdPacket && originalPak is ISessionIdPacket && (pak as ISessionIdPacket).SessionId == (originalPak as ISessionIdPacket).SessionId)
					return true;
				else if (pak is IHouseIdPacket && originalPak is IHouseIdPacket && (pak as IHouseIdPacket).HouseId == (originalPak as IHouseIdPacket).HouseId)
					return true;
				else if (pak is IKeepIdPacket && originalPak is IKeepIdPacket && (pak as IKeepIdPacket).KeepIds.Length > 0  && (originalPak as IKeepIdPacket).KeepIds.Length > 0 && (pak as IKeepIdPacket).KeepIds[0] == (originalPak as IKeepIdPacket).KeepIds[0])
					return true;
			}
			return false;
		}
	}
}
