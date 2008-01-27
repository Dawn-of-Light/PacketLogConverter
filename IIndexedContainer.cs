using System;
using System.Collections.Generic;
using System.Text;

namespace PacketLogConverter
{
	/// <summary>
	/// Provides common interface for data containers with index. More simple than <see cref="IList{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type of objects contained in container.</typeparam>
	public interface IIndexedContainer<T>
	{
		/// <summary>
		/// Gets the count of objects contained in the container.
		/// </summary>
		/// <value>The count.</value>
		int Count { get; }

		/// <summary>
		/// Gets the <see cref="T:T"/> at the specified index.
		/// </summary>
		/// <value></value>
		T this[int index] { get; }
	}
}
