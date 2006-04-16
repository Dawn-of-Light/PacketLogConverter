using System.Text;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows known player info before selected packet
	/// </summary>
	[LogAction("Show player info", Priority=1000)]
	public class ShowPlayerInfoAction : ILogAction
	{
		#region ILogAction Members

		public enum PlrState : byte
		{
			Stand = 0,
			Swim = 1,
			Jump = 2,
			debugFly = 3,
			Sit = 4,
			Died = 5,
			Ride = 6,
			Climb = 7,
		}

		/// <summary>
		/// Activate log action
		/// </summary>
		/// <param name="log">The current log</param>
		/// <param name="selectedIndex">The selected packet index</param>
		/// <returns>True if log data tab should be updated</returns>
		public virtual bool Activate(PacketLog log, int selectedIndex)
		{
			int sessionId = -1;
			int objectId = -1;
			int healthPercent = -1;
			int endurancePercent = -1;
			int manaPercent = -1;
			int concentrationPercent = -1;
			int speed = -1;
			int maxspeed = -1;
			string state = "";
			string charName = "UNKNOWN";

			for (int i = 0; i < selectedIndex; i++)
			{
				Packet pak = log[i];
				if (pak is CtoS_0xA9_PlayerPosition)
				{
					CtoS_0xA9_PlayerPosition pos = (CtoS_0xA9_PlayerPosition)pak;
					speed = pos.Status & 0x1FF;
					byte plrState = (byte)((pos.Status >> 10) & 7);
					state = plrState > 0 ? ((PlrState)plrState).ToString() : "";
					if ((pos.Status & 0x200) == 0x200)
						state += ",Backward";
					if ((pos.Status & 0x8000) == 0x8000)
						state += ",StrafeRight";
					if ((pos.Status & 0x4000) == 0x4000)
						state += ",StrafeLeft";
					if ((pos.Status & 0x2000) == 0x2000)
						state += "Move";
					if ((pos.Flag & 0x02) == 0x02)
						state += ",Diving";
					if ((pos.Flag & 0x08) == 0x08)
						state += ",GT";
					if ((pos.Flag & 0x10) == 0x10)
						state += ",HaveTarget";
					if ((pos.Flag & 0x20) == 0x20)
						state += ",TargetInView";
					if ((pos.Flag & 0x40) == 0x40)
						state += ",MoveTo";
					if ((pos.Health & 0x80) == 0x80)
						state += ",Combat";
				}
				else if (pak is CtoS_0x10_CharacterSelectRequest)
				{
					CtoS_0x10_CharacterSelectRequest login = (CtoS_0x10_CharacterSelectRequest)pak;
					charName = login.CharName;
				}
				else if (pak is StoC_0xB6_UpdateMaxSpeed)
				{
					StoC_0xB6_UpdateMaxSpeed spd = (StoC_0xB6_UpdateMaxSpeed)pak;
					maxspeed = spd.MaxSpeedPercent;
				}
				else if (pak is StoC_0xAD_StatusUpdate)
				{
					StoC_0xAD_StatusUpdate status = (StoC_0xAD_StatusUpdate)pak;
					healthPercent = status.HealthPercent;
					endurancePercent = status.EndurancePercent;
					manaPercent = status.ManaPercent;
					concentrationPercent = status.ConcentrationPercent;
				}
				else if (pak is StoC_0x28_SetSessionId)
				{
					StoC_0x28_SetSessionId session = (StoC_0x28_SetSessionId)pak;
					sessionId = session.SessionId;
				}
				else if (pak is StoC_0x20_PlayerPositionAndObjectID)
				{
					StoC_0x20_PlayerPositionAndObjectID posAndOid = (StoC_0x20_PlayerPositionAndObjectID)pak;
					objectId = posAndOid.PlayerOid;
				}
			}

			StringBuilder str = new StringBuilder();
			str.AppendFormat("session id: 0x{0}\n", ValueToString(sessionId, "X4"));
			str.AppendFormat(" object id: 0x{0}\n", ValueToString(objectId, "X4"));
			str.AppendFormat(" char name: {0}\n", charName);
			str.AppendFormat("\n");
			str.AppendFormat("        speed: {0,3}\n", ValueToString(speed));
			str.AppendFormat("     maxSpeed: {0,3}%\n", ValueToString(maxspeed));
			str.AppendFormat("       health: {0,3}%\n", ValueToString(healthPercent));
			str.AppendFormat("         mana: {0,3}%\n", ValueToString(manaPercent));
			str.AppendFormat("    endurance: {0,3}%\n", ValueToString(endurancePercent));
			str.AppendFormat("concentration: {0,3}%\n", ValueToString(concentrationPercent));
			str.AppendFormat("        state: {0}\n", state);

			InfoWindowForm infoWindow = new InfoWindowForm();
			infoWindow.Text = "Player info (right click to close)";
			infoWindow.Width = 500;
			infoWindow.Height = 220;
			infoWindow.InfoRichTextBox.Text = str.ToString();
			infoWindow.StartWindowThread();

			return false;
		}

		#endregion

		private string ValueToString(int i)
		{
			return ValueToString(i, null);
		}

		private string ValueToString(int i, string format)
		{
			if (i == -1)
				return "(unknown)";
			if (format == null)
				return i.ToString();
			return i.ToString(format);
		}
	}
}
