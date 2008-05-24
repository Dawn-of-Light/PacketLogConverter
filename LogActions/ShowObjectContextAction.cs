using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PacketLogConverter.LogPackets;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows some packets around selected packet.
	/// </summary>
	[LogAction("Show object context")]
	public class Object : ILogAction
	{
		private readonly List<Thread> m_startedThreads = new List<Thread>();

		/// <summary>
		/// Determines whether the action is enabled.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns>
		/// 	<c>true</c> if the action is enabled; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEnabled(IExecutionContext context, PacketLocation selectedPacket)
		{
			Packet pak = context.LogManager.GetPacket(selectedPacket);
			if (pak is IObjectIdPacket && (pak as IObjectIdPacket).ObjectIds.Length > 0)
				return true;
			return false;
		}

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			PacketLog selectedLog = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			int selectedIndex = selectedPacket.PacketIndex;
			Packet originalPak = selectedLog[selectedIndex];
			ushort objectId = 0;
			if (originalPak is IObjectIdPacket && (originalPak as IObjectIdPacket).ObjectIds.Length > 0)
				objectId = (originalPak as IObjectIdPacket).ObjectIds[0];
			if (objectId == 0)
				return false;
			// Create log
			PacketLog			log = new PacketLog();
			ICollection<Packet>	packets = SelectPacketContext(context, selectedPacket, objectId);
			log.AddRange(packets);
			log.Version					= selectedLog.Version;
			log.StreamName				= "(context OID:0x" + objectId.ToString("X4") + ") " + selectedLog.StreamName;
			log.IgnoreVersionChanges	= false;

			// Show form in another thread
			Thread t = new Thread(ShowNewForm);
			lock (m_startedThreads)
			{
				m_startedThreads.Add(t);
				t.SetApartmentState(ApartmentState.STA);
				t.Name = "context OID:0x" + objectId.ToString("X4");
				t.Start(log);
			}

			return false;
		}

		/// <summary>
		/// Shows the new form.
		/// </summary>
		/// <param name="log">The log.</param>
		private void ShowNewForm(object log)
		{
			try
			{
				// Create new form
				MainForm form = new MainForm();
				form.LogManager.AddLog((PacketLog) log);
				form.ShowDataTab();

				Application.Run(form);
			}
			catch (Exception e)
			{
				Log.Error("packet context window loop", e);
			}
			finally
			{
				// Remove this thread from the list of active threads
				lock (m_startedThreads)
				{
					m_startedThreads.Remove(Thread.CurrentThread);
				}
			}
		}

		/// <summary>
		/// Selects the packet context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="packet">The packet.</param>
		/// <returns>List with packet context.</returns>
		private static ICollection<Packet> SelectPacketContext(IExecutionContext context, PacketLocation packet, ushort objectId)
		{
			List<Packet> packets;
			PacketLog log = context.LogManager.GetPacketLog(packet.LogIndex);
			if (log != null && log.Count > 0)
			{
				packets = new List<Packet>();
				for (int i = packet.PacketIndex; i >= 0; i--)
				{
					Packet pak = log[i];
					bool flagPacketAdded = false;
					if (pak is IObjectIdPacket)
					{
						foreach (ushort Oid in (pak as IObjectIdPacket).ObjectIds)
						{
							if (Oid == objectId)
							{
								packets.Insert(0, pak);
								flagPacketAdded = true;
								break;
							}
						}
					}
					if (pak is StoC_0x20_PlayerPositionAndObjectID) // stop scanning packets on enter region
					{
						if (!flagPacketAdded)
							packets.Insert(0, pak);
						flagPacketAdded = true;
						break;
					}
					else if (pak is CtoS_0xAC_PlayerResetCharacter)
					{
						if (!flagPacketAdded)
							packets.Insert(0, pak);
						flagPacketAdded = true;
						break;
					}
				}

				for (int i = packet.PacketIndex + 1; i < log.Count; i++)
				{
					Packet pak = log[i];
					bool flagPacketAdded = false;
					if (pak is IObjectIdPacket)
					{
						foreach (ushort Oid in (pak as IObjectIdPacket).ObjectIds)
						{
							if (Oid == objectId)
							{
								packets.Add(pak);
								flagPacketAdded = true;
								break;
							}
						}
					}
					if (pak is StoC_0x20_PlayerPositionAndObjectID) // stop scanning packets on enter region
					{
						if (!flagPacketAdded)
							packets.Add(pak);
						flagPacketAdded = true;
						break;
					}
					else if (pak is CtoS_0xAC_PlayerResetCharacter)
					{
						if (!flagPacketAdded)
							packets.Add(pak);
						flagPacketAdded = true;
						break;
					}
				}
			}
			else
			{
				packets = new List<Packet>(0);
			}

			return packets;
		}
	}
}
