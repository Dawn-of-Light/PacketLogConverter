using System;
using System.Collections;
using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows all known Oids before selected packet
	/// </summary>
	[LogAction("Show keep components", Priority=800)]
	public class ShowKeepComponentActions : AbstractEnabledAction
	{
		#region ILogAction Members

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public override bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog log = context.LogManager.Logs[selectedPacket.LogIndex];
			int selectedIndex = selectedPacket.PacketIndex;

			int currentRegion = 0;
			int currentZone = 0;
			Hashtable keepComponentsByOids = new Hashtable();
			Hashtable keepComponents = new Hashtable();
			Hashtable keeps = new Hashtable();
			StringBuilder str = new StringBuilder();
			for (int i = 0; i < selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is StoC_0x6C_KeepComponentOverview)
				{
					StoC_0x6C_KeepComponentOverview  keep = (StoC_0x6C_KeepComponentOverview)pak;
					string key = keep.KeepId + "C:" + keep.ComponentId;
					Component kc;
					if (!keepComponents.ContainsKey(key))
					{
						kc = new Component();
						kc.KeepId = keep.KeepId;
						kc.ComponentId = keep.ComponentId;
						kc.Oid = keep.Uid;
						kc.Skin = keep.Skin;
						kc.addX = (sbyte)keep.X;
						kc.addY = (sbyte)keep.Y;
						kc.Rotate = keep.Heading;
						kc.Height = keep.Height;
						kc.Health = keep.Health;
						kc.Status = keep.Status;
						kc.Flag = keep.Flag;
						kc.Zone = 0;
						keepComponents[key] = kc;
					}
					else kc = (Component)keepComponents[key];
					keepComponentsByOids[keep.Uid] = kc;
				}
				else if (pak is StoC_0x69_KeepOverview)
				{
					StoC_0x69_KeepOverview keep = (StoC_0x69_KeepOverview)pak;
					if (!keeps.ContainsKey(keep.KeepId))
					{
						Keep k = new Keep();
						k.KeepId = keep.KeepId;
						k.Angle = keep.Heading;
						k.X = keep.KeepX;
						k.Y = keep.KeepY;
						keeps[keep.KeepId] = k;
					}
				}
				else if (pak is StoC_0xA1_NpcUpdate)
				{
					StoC_0xA1_NpcUpdate obj = (StoC_0xA1_NpcUpdate)pak;
					if (keepComponentsByOids.ContainsKey(obj.NpcOid))
					{
						Component kc = (Component)keepComponentsByOids[obj.NpcOid];
						string key = kc.KeepId + "C:" + kc.ComponentId;
						kc.Zone = obj.CurrentZoneId;
						kc.Heading = (ushort)(obj.Heading & 0xFFF);
						kc.X = obj.CurrentZoneX;
						kc.Y = obj.CurrentZoneY;
						kc.Z = obj.CurrentZoneZ;
						keepComponentsByOids[obj.NpcOid] = kc;
						keepComponents[key] = kc;
					}
				}
				else if (pak is StoC_0xB7_RegionChange)
				{
					StoC_0xB7_RegionChange region = (StoC_0xB7_RegionChange)pak;

					if (region.RegionId != currentRegion)
					{
						currentRegion = region.RegionId;
						currentZone = region.ZoneId;
						keepComponentsByOids.Clear();
					}
				}
				else if (pak is StoC_0x20_PlayerPositionAndObjectID_171)
				{
					StoC_0x20_PlayerPositionAndObjectID_171 region = (StoC_0x20_PlayerPositionAndObjectID_171)pak;

					if (region.Region!= currentRegion)
					{
						currentRegion = region.Region;
						keepComponentsByOids.Clear();
					}
				}
			}
			foreach (Component kc in keepComponents.Values)
			{
				if (kc.Zone != 0)
				{
					Keep k = (Keep)keeps[kc.KeepId];
					str.AppendFormat("keepId:{0,-4} keepX:{1,-6} keepY:{2,-6} angle:{3,-3} ", k.KeepId, k.X, k.Y, k.Angle);
					str.AppendFormat("componentId:{0,-2} oid:{1,-5} Skin:{2,-2} X:{3,-4} Y:{4,-4} Rotate:{5} ", kc.ComponentId, kc.Oid, kc.Skin, kc.addX, kc.addY, kc.Rotate);
					str.AppendFormat("Heading:{0,-4} Zone:{1,-3} @X:{2,-5} @Y:{3,-5} Z:{4}\n", kc.Heading, kc.Zone, kc.X, kc.Y, kc.Z);
				}
			}
			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Show keep components info (right click to close)";
			infoWindow.Width = 650;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();
			return false;
		}
		#endregion

		public class Component
		{
			public ushort KeepId;
			public ushort ComponentId;
			public ushort Oid;
			public byte Skin;
			public sbyte addX;
			public sbyte addY;
			public byte Rotate;
			public byte Height;
			public byte Health;
			public byte Status;
			public byte Flag;
			public ushort Zone;
			public ushort Heading;
			public ushort X;
			public ushort Y;
			public ushort Z;
		}

		public class Keep
		{
			public ushort KeepId;
			public ushort Angle;
			public uint X;
			public uint Y;
		}
	}
}
