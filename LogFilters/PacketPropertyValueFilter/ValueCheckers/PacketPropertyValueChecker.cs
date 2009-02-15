using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers
{
	/// <summary>
	/// This class maintains and checks current value of a single packet property variable.
	/// </summary>
	internal class PacketPropertyValueChecker : ObjectValueChecker
	{
		private readonly	PacketPropertyVariablesManager					variablesManager;
		private readonly	string											variableName;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:PacketPropertyValueChecker"/> class.
		/// </summary>
		/// <param name="variablesManager">The variables manager.</param>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="entry">The entry.</param>
		public PacketPropertyValueChecker(PacketPropertyVariablesManager variablesManager, string variableName, PacketPropertyValueFilterForm.FilterListEntry entry)
			: base(entry)
		{
			variablesManager.RegisterVariableValueUpdateHandler(variableName, UpdateValue);

			this.variablesManager	= variablesManager;
			this.variableName		= variableName;

			InitNonSerialized();
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public override void Dispose()
		{
			variablesManager.UnregisterVariableValueUpdateHandler(variableName, UpdateValue);
		}

		/// <summary>
		/// Updates the value.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private void UpdateValue(PacketPropertyVariable variable, object oldValue, object newValue)
		{
			value = newValue;

			InitNonSerialized();
		}
	}
}
