using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Encapsulates common logic for all OID filters.
	/// </summary>
	public abstract class AbstractDynamicOIDFilter : AbstractFilter
	{
		protected static readonly ushort ID_NOT_SET = 0;
		
		private DynamicFilterHelper m_filterHelper;
		private ushort m_oid;
		private ushort m_sid;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AbstractDynamicOIDFilter"/> class.
		/// </summary>
		protected AbstractDynamicOIDFilter()
		{
			// Initialize helper class
			m_filterHelper.Start			= FilterManager_OnFilteringStartedEvent;
			m_filterHelper.Stop				= FilterManager_OnFilteringStoppedEvent;
			m_filterHelper.ProcessPacket	= FilterManager_OnFilteringPacketEvent;
		}

		/// <summary>
		/// Gets or sets the oid.
		/// </summary>
		/// <value>The oid.</value>
		protected ushort Oid
		{
			get { return m_oid; }
			set { m_oid = value; }
		}

		/// <summary>
		/// Gets or sets the sid.
		/// </summary>
		/// <value>The sid.</value>
		protected ushort Sid
		{
			get { return m_sid; }
			set { m_sid = value; }
		}

		#region Filter event handlers

		/// <summary>
		/// Resets all variables when filtering starts.
		/// </summary>
		/// <param name="log">The log.</param>
		protected virtual void FilterManager_OnFilteringStartedEvent(PacketLog log)
		{
			m_oid = ID_NOT_SET;
			m_sid = ID_NOT_SET;
		}

		/// <summary>
		/// Resets all variables when filtering stops.
		/// </summary>
		/// <param name="log">The log.</param>
		protected virtual void FilterManager_OnFilteringStoppedEvent(PacketLog log)
		{
		}

		/// <summary>
		/// Reads information from all usefull packets.
		/// </summary>
		/// <param name="packet">The packet.</param>
		protected abstract void FilterManager_OnFilteringPacketEvent(Packet packet);

		#endregion

		#region ILogFilter Members

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsPacketIgnored(Packet packet)
		{
			bool bRet = true;

			if (m_sid != ID_NOT_SET)
			{
				ISessionIdPacket spak = packet as ISessionIdPacket;
				if (spak != null && spak.SessionId == m_sid)
				{
					bRet = false;
				}
			}
			if (bRet && m_oid != ID_NOT_SET)
			{
				IObjectIdPacket pak = packet as IObjectIdPacket;
				if (pak != null)
				{
#warning TODO: Check performance?
//					int index = Array.IndexOf<ushort>(pak.ObjectIds, m_selfOID);
//					bRet = 0 > index;
					// Check pet OID
					foreach (ushort id in pak.ObjectIds)
					{
						if (id == m_oid)
						{
							bRet = false;
							break;
						}
					}
				}
			}

			return bRet;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public override bool IsFilterActive
		{
			set
			{
				base.IsFilterActive = value;
				m_filterHelper.SetEventHandlers(value);
			}
		}

		#endregion
	}
}
