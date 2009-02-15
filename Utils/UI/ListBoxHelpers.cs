using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;

namespace PacketLogConverter.Utils.UI
{
	/// <summary>
	/// This class contains helper methods for <see cref="ListBox"/> control.
	/// </summary>
	public class ListBoxHelpers
	{
		/// <summary>
		/// Refreshes the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		public static void Refresh(ListBox list)
		{
			for (int i = list.Items.Count - 1; 0 <= i; --i)
			{
				// Calls .ToString() for each element to refresh visible text
				// Perhaps also worth to restore selected item and scroll bar position
				list.Items[i] = list.Items[i];
			}
		}

		/// <summary>
		/// Adds the data to list box.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="data">The data.</param>
		/// <typeparam name="T">Type of data to add.</typeparam>
		public static void AddRange<T>(ListBox list, ICollection<T> data) where T : class
		{
			T[] array = new T[data.Count];
			data.CopyTo(array, 0);
			list.Items.AddRange(array);
		}
	}
}
