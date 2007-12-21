using System;
using System.IO;
using System.Collections;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogWriters
{
	/// <summary>
	/// Writes inventory items to the file with custom format
	/// </summary>
	[LogWriter("KeepObjectXmlWriter", "KeepGuard.xml")]
	public class KeepObjectXmlWriter : ILogWriter
	{
		private Hashtable m_keeps = new Hashtable();

		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="stream">The stream.</param>
		/// <param name="callback">The callback for UI updates.</param>
		public void WriteLog(IExecutionContext context, Stream stream, ProgressCallback callback)
		{
			using (StreamWriter s = new StreamWriter(stream))
			{
				int region = 0;
				int ZoneXOffset = 0;
				int ZoneYOffset = 0;
				Hashtable m_obj = new Hashtable();
				foreach (PacketLog log in context.LogManager.Logs)
				{
					for (int i = 0; i < log.Count; i++)
					{
						if (callback != null && (i & 0xFFF) == 0) // update progress every 4096th packet
							callback(i, log.Count - 1);

						StoC_0x20_PlayerPositionAndObjectID_171 pak_reg20 = log[i] as StoC_0x20_PlayerPositionAndObjectID_171;
						if (pak_reg20 != null)
						{
							region = pak_reg20.Region;
							ZoneXOffset = pak_reg20.ZoneXOffset;
							ZoneYOffset = pak_reg20.ZoneYOffset;
							//						m_obj.Clear();
							m_keeps.Clear();
							continue;
						}
						StoC_0xB7_RegionChange pak_regB7 = log[i] as StoC_0xB7_RegionChange;
						if (pak_regB7 != null)
						{
							region = pak_regB7.RegionId;
							ZoneXOffset = 0;
							ZoneYOffset = 0;
							//						m_obj.Clear();
							m_keeps.Clear();
							continue;
						}
						if (region != 238) continue;
						StoC_0x69_KeepOverview pak_keep = log[i] as StoC_0x69_KeepOverview;
						if (pak_keep != null)
						{
							Keep keep = new Keep();
							keep.KeepID = pak_keep.KeepId;
							keep.X = (uint) (pak_keep.KeepX + ZoneXOffset*0x2000);
							keep.Y = (uint) (pak_keep.KeepY + ZoneYOffset*0x2000);
							keep.Heading = pak_keep.Heading;
							keep.Realm = pak_keep.Realm;
							keep.Level = pak_keep.Level;
							m_keeps[keep.KeepID] = keep;
							continue;
						}
						KeepGuard guard = new KeepGuard();
						StoC_0xD9_ItemDoorCreate door = log[i] as StoC_0xD9_ItemDoorCreate;
						string key = "";
						if (door != null)
						{
							if (door.ExtraBytes == 4)
							{
								guard.KeepGuard_ID = door.InternalId.ToString();
								guard.Name = door.Name;
								guard.EquipmentID = "";
								guard.KeepID = 0;
								guard.BaseLevel = 0;
								guard.X = (uint) (door.X + ZoneXOffset*0x2000);
								guard.Y = (uint) (door.Y + ZoneYOffset*0x2000);
								guard.Z = door.Z;
								guard.Heading = door.Heading;
								guard.Realm = (door.Flags & 0x30) >> 4;
								guard.Model = door.Model;
								guard.ClassType = "DOL.GS.GameKeepDoor";
								key = "REGION:" + region.ToString() + "-DOOR:" + guard.KeepGuard_ID;
							}
							else
								continue;
						}
						else
							continue;
						if (key == "") continue;
						if (m_obj.ContainsKey(key)) continue;
						Keep my_keep = getKeepCloseToSpot(guard.X, guard.Y, 2000);
						if (my_keep != null)
						{
							guard.KeepID = my_keep.KeepID;
							guard.BaseLevel = my_keep.Level;
						}
						m_obj[key] = guard;
					}
				}
				s.WriteLine("<KeepGuard>");
				foreach (KeepGuard guard in m_obj.Values)
				{
					s.WriteLine("  <KeepGuard>");
					if(guard.KeepGuard_ID == "")
						s.WriteLine("    <KeepGuard_ID />");
					else
						s.WriteLine(string.Format("    <KeepGuard_ID>{0}</KeepGuard_ID>",guard.KeepGuard_ID));
					if(guard.Name == "")
						s.WriteLine("    <Name />");
					else
						s.WriteLine(string.Format("    <Name>{0}</Name>",guard.Name));
					if(guard.EquipmentID == "")
						s.WriteLine("    <EquipmentID />");
					else
						s.WriteLine(string.Format("    <EquipmentID>{0}</EquipmentID>",guard.EquipmentID));
					s.WriteLine(string.Format("    <KeepID>{0}</KeepID>",guard.KeepID));
					s.WriteLine(string.Format("    <BaseLevel>{0}</BaseLevel>",guard.BaseLevel));
					s.WriteLine(string.Format("    <X>{0}</X>",guard.X));
					s.WriteLine(string.Format("    <Y>{0}</Y>",guard.Y));
					s.WriteLine(string.Format("    <Z>{0}</Z>",guard.Z));
					s.WriteLine(string.Format("    <Heading>{0}</Heading>",guard.Heading));
					s.WriteLine(string.Format("    <Realm>{0}</Realm>",guard.Realm));
					s.WriteLine(string.Format("    <Model>{0}</Model>",guard.Model));
					if(guard.ClassType == "")
						s.WriteLine("    <ClassType />");
					else
						s.WriteLine(string.Format("    <ClassType>{0}</ClassType>",guard.ClassType));
					s.WriteLine("  </KeepGuard>");
				}
				s.WriteLine("</KeepGuard>");
			}
		}
		public class KeepGuard
		{
			public string KeepGuard_ID;
			public string Name;
			public string EquipmentID;
			public int KeepID;
			public int BaseLevel;
			public uint X;
			public uint Y;
			public int Z;
			public int Heading;
			public int Realm;
			public int Model;
			public string ClassType;
		}
		public class Keep
		{
			public int KeepID;
			public int Level;
			public uint X;
			public uint Y;
			public int Heading;
			public int Realm;
		}
		public Keep getKeepCloseToSpot(uint x, uint y, int radius)
		{
			Keep myKeep = null;
			int myKeepRange = radius;
			int radiussqrt = radius * radius;
			foreach(Keep keep in m_keeps.Values)
			{
				int xdiff = (int)(keep.X - x);
				int ydiff = (int)(keep.Y - y);
				int range = xdiff * xdiff + ydiff * ydiff ;
				if (range > radiussqrt)
					continue;
				if ( myKeep == null || range <= myKeepRange )
				{
					myKeep = keep;
					myKeepRange = range;
				}
			}
			return myKeep;
		}
	}
}
