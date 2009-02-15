using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter.LogFilters.PacketPropertyValueFilter.ValueCheckers
{
	/// <summary>
	/// Checks a value.
	/// </summary>
	/// <typeparam name="T">Type of value this checker checks.</typeparam>
	internal interface IValueChecker<T> : IDisposable
	{
		/// <summary>
		/// Determines whether a packet is ignored by this instance of checker.
		/// </summary>
		/// <param name="packetToCheck">The value to check.</param>
		/// <returns>
		/// 	<c>true</c> if value is ignored; otherwise, <c>false</c>.
		/// </returns>
		bool IsPacketIgnored(Packet packetToCheck);

		/// <summary>
		/// Gets the entry.
		/// </summary>
		/// <value>The entry.</value>
		PacketPropertyValueFilterForm.FilterListEntry Entry { get; }
	}
}