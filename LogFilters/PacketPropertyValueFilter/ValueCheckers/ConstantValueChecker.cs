using System;
using System.Collections.Generic;
using System.Text;
using PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers;

namespace PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers
{
	/// <summary>
	/// This class checks for constant value.
	/// </summary>
	internal class ConstantValueChecker : ObjectValueChecker
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ConstantValueChecker"/> class.
		/// </summary>
		/// <param name="entry">The entry.</param>
		public ConstantValueChecker(PacketPropertyValueFilterForm.FilterListEntry entry)
			: base(entry)
		{
			this.value = entry.searchValue;

			InitNonSerialized();
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public override void Dispose()
		{
			// Do nothing
		}
	}
}