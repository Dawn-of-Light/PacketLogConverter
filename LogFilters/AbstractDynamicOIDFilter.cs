using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Encapsulates common logic for all OID filters.
	/// </summary>
	public abstract class AbstractDynamicOIDFilter : ILogFilter
	{
		protected static readonly ushort ID_NOT_SET = 0;
		
		protected DynamicFilterHelper m_filterHelper;
		protected ushort m_oid;
		private bool m_active;

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
		/// Resets all variables when filtering starts.
		/// </summary>
		/// <param name="log">The log.</param>
		protected virtual void FilterManager_OnFilteringStartedEvent(PacketLog log)
		{
			m_oid = ID_NOT_SET;
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

		#region ILogFilter Members

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns><code>true</code> if filter has changed and log should be updated.</returns>
		public virtual bool ActivateFilter()
		{
			m_active = !m_active;
			if (IsFilterActive)
				FilterManager.AddFilter(this);
			else
				FilterManager.RemoveFilter(this);

			m_filterHelper.SetEventHandlers(m_active);

			return true;
		}

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool IsPacketIgnored(Packet packet)
		{
			// No id is set
			if (m_oid == ID_NOT_SET)
				return true;

			bool bRet = true;

			IObjectIdPacket pak = packet as IObjectIdPacket;
			if (pak != null)
			{
#warning TODO: Check performance?
//				int index = Array.IndexOf<ushort>(pak.ObjectIds, m_selfOID);
//				bRet = 0 > index;
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

			return bRet;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsFilterActive
		{
			get { return m_active; }
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public virtual bool Serialize(MemoryStream data)
		{
			data.WriteByte((byte)(m_active ? 1 : 0));
			return true;
		}

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		public virtual bool Deserialize(MemoryStream data)
		{
			int active = data.ReadByte();
			m_active = 0 != active;
			return true;
		}

		#endregion
	}
}
