using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers;
using PacketLogConverter.Utils;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// This class filters packets by user-entered values of properties.
	/// </summary>
	internal class Filter : FilterManagerAwareFilter, IDisposable
	{
		private readonly PacketPropertyVariablesManager	variablesManager;
		private readonly IValueChecker<object>[]		valueCheckers;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Filter"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="variablesManager">The variables manager.</param>
		/// <param name="valueCheckers">The value checkers.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="variablesManager"/> is <code>null</code>.</exception>
		public Filter(IExecutionContext context, PacketPropertyVariablesManager variablesManager, List<IValueChecker<object>> valueCheckers)
			: base(context)
		{
			if (null == variablesManager)
			{
				throw new ArgumentNullException("variablesManager", "Variables manager is required to function properly.");
			}

			this.variablesManager = variablesManager;
			// Clone list of value checkers
			this.valueCheckers = valueCheckers.ToArray();
		}

		/// <summary>
		/// Processes start of log filtering event.
		/// </summary>
		/// <param name="log">The packets log.</param>
		protected override void FilterManager_OnFilteringStartedEvent(IExecutionContext log)
		{
			variablesManager.ResetVariablesValues();
		}

		/// <summary>
		/// Processes stop of log filtering event.
		/// </summary>
		/// <param name="log"></param>
		protected override void FilterManager_OnFilteringStoppedEvent(IExecutionContext log)
		{
			// Reset variable values after checkers are disposed to not trigger them
			variablesManager.ResetVariablesValues();
		}

		/// <summary>
		/// Processes start of filtering of a packet by filter manager.
		/// </summary>
		/// <param name="packet"></param>
		protected override void FilterManager_OnFilteringPacketEvent(Packet packet)
		{
			variablesManager.ProcessPacket(packet);
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public override bool Serialize(MemoryStream data)
		{
			// This filter is not serializable, it is an intermediate hack
			return false;
		}

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		public override bool Deserialize(MemoryStream data)
		{
			// This filter is not serializable, it is an intermediate hack
			return false;
		}

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsPacketIgnored(Packet packet)
		{
			// check logic always compared with previous condition current packet
			// condition "OR(" begin new (conditions) while again not meet "OR("
			// samples:
			// (A || (B && C)) = A "OR" B "AND" C
			// ((A || B) && C) = A "AND" C "OR(" B "AND" C
			// ((A && B) || C) = A "AND" B OR C = A "AND" B "OR(" C
			// ((A && B) || (C && D) = A "AND" B "OR(" C "AND" D
			// (((A && B) || C) && D) = A "AND" B "OR" C "AND" D
			int i = 0;
			bool packetIgnoreState = true;
			bool flagBadAndCheck = false;

			foreach (IValueChecker<object> checker in valueCheckers)
			{
				// Cache filter list entry of current checker
				PacketPropertyValueFilterForm.FilterListEntry entry = checker.Entry;
				if (entry.packetClass.type == null || entry.packetClass.type.IsAssignableFrom(packet.GetType()))
				{
					bool state = checker.IsPacketIgnored(packet);

					if (entry.condition == "OR(")
					{
						if (!packetIgnoreState) // if previouse Subcheck from this packet is not ignored then packet pass
							return false;
						flagBadAndCheck = false;
						i = 0; // begin new Subcheck
					}
					else if (flagBadAndCheck) // if previous AND check == false and this releation is not new Subcheck, then packets is ignored
					{
						return true;
					}
					if (i == 0)
						packetIgnoreState = state;
					else
					{
						if (entry.condition == "AND")
							packetIgnoreState |= state;
						else if (entry.condition == "OR")
							packetIgnoreState &= state;
					}
					if (packetIgnoreState && (i > 0) && entry.condition == "AND") // if this check is last in Subcheck then packet will be ignored
						flagBadAndCheck = true;
					i++;
				}
			}
			return packetIgnoreState;
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			// Dispose all value checkers
			foreach (IValueChecker<object> checker in valueCheckers)
			{
				checker.Dispose();
			};
		}
	}
}
