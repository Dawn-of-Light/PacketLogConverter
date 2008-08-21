using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PacketLogConverter.LogActions
{
	/// <summary>
	/// Shows some packets around selected packet.
	/// </summary>
	[LogAction("Show packet context")]
	public class ShowPacketContextAction : AbstractEnabledAction
	{
		private static readonly int PACKET_CONTEXT_SIZE = 100;
		public Packet savedPacket = null;

		private readonly List<Thread> m_startedThreads = new List<Thread>();

		/// <summary>
		/// Activates a log action.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="selectedPacket">The selected packet.</param>
		/// <returns><c>true</c> if log data tab should be updated.</returns>
		public override bool Activate(IExecutionContext context, PacketLocation selectedPacket)
		{
			// Create log
			PacketLog			log = new PacketLog();
			ICollection<Packet>	packets = SelectPacketContext(context, selectedPacket);
			savedPacket = context.LogManager.GetPacket(selectedPacket);
			log.AddRange(packets);
			PacketLog selectedLog = context.LogManager.GetPacketLog(selectedPacket.LogIndex);
			log.StreamName				= "(packet context) " + selectedLog.StreamName;
			log.IgnoreVersionChanges	= false;
			log.Version					= selectedLog.Version;

			// Show form in another thread
			Thread t = new Thread(ShowNewForm);
			lock (m_startedThreads)
			{
				m_startedThreads.Add(t);
				t.SetApartmentState(ApartmentState.STA);
				t.Name = "Packet context";
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
				if (savedPacket != null)
				{
					form.RestoreLogPositionByPacket(savedPacket);
				}
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
		private static ICollection<Packet> SelectPacketContext(IExecutionContext context, PacketLocation packet)
		{
			List<Packet> packets;
			PacketLog log = context.LogManager.GetPacketLog(packet.LogIndex);
			if (log != null && log.Count > 0)
			{
				// Calculate first/last indices
				int firstPacketIndex = Math.Max(0, packet.PacketIndex - PACKET_CONTEXT_SIZE);
				int lastPacketIndex = Math.Min(log.Count, packet.PacketIndex + PACKET_CONTEXT_SIZE);

				// Select range
				packets = log.GetRange(firstPacketIndex, lastPacketIndex);
			}
			else
			{
				packets = new List<Packet>(0);
			}

			return packets;
		}
	}
}
