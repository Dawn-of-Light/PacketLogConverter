using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers;

namespace PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers
{
	/// <summary>
	/// This class handles creation of value checkers.
	/// </summary>
	internal struct ValueCheckerBuilder
	{
		private readonly PacketPropertyVariablesManager variablesManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ValueCheckerBuilder"/> class.
		/// </summary>
		/// <param name="variablesManager">The variables manager.</param>
		public ValueCheckerBuilder(PacketPropertyVariablesManager variablesManager)
		{
			this.variablesManager = variablesManager;
		}

		/// <summary>
		/// Creates the checkers from filter list entries.
		/// </summary>
		/// <param name="entries">The collection with instances of <see cref="PacketPropertyValueFilterForm.FilterListEntry"/> class.</param>
		/// <returns>List with <see cref="IValueChecker{T}<object>"/> objects.</returns>
		public List<IValueChecker<object>> CreateCheckersFromFilterListEntries(ICollection entries)
		{
			List<IValueChecker<object>> ret = new List<IValueChecker<object>>(entries.Count);
			foreach (PacketPropertyValueFilterForm.FilterListEntry entry in entries)
			{
				IValueChecker<object> checker;
				if (entry.searchValue is PacketPropertyVariable)
				{
					// Construct packet property variable value checker
					PacketPropertyVariable variable = (PacketPropertyVariable)entry.searchValue;
					checker = new PacketPropertyValueChecker(variablesManager, variable.Name, entry);
				}
				else
				{
					// Construct simple constant value checker
					checker = new ConstantValueChecker(entry);
				}

				// Add created checker to returned list
				ret.Add(checker);
			}

			return ret;
		}
	}
}