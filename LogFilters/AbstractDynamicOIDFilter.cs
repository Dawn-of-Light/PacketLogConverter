using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Encapsulates common logic for all OID filters.
	/// </summary>
	public abstract class AbstractDynamicOIDFilter : FilterManagerAwareFilter
	{
		protected static readonly ushort ID_NOT_SET = 0;

		private ushort m_oid;
		private ushort m_sid;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:AbstractDynamicOIDFilter"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public AbstractDynamicOIDFilter(IExecutionContext context) : base(context)
		{
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
		protected override void FilterManager_OnFilteringStartedEvent(IExecutionContext log)
		{
			m_oid = ID_NOT_SET;
			m_sid = ID_NOT_SET;
		}

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

		#endregion
	}
}
