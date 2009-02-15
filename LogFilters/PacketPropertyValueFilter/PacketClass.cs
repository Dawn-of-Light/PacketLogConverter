using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// This class contains helper methods for <see cref="PacketPropertyValueFilterForm"/>.
	/// </summary>
	public partial class PacketPropertyValueFilterForm
	{
		/// <summary>
		/// This class shows packet class in combo box.
		/// </summary>
		[Serializable]
		internal class PacketClass
		{
			public readonly Type type;
			public readonly LogPacketAttribute attr;

			public PacketClass(Type type, LogPacketAttribute attr)
			{
				this.type = type;
				this.attr = attr;
			}

			public override string ToString()
			{
				if (type == null || attr == null)
					return "(any packet)";
				string dir = (attr.Direction == ePacketDirection.ServerToClient ? "S=>C" : "S<=C");
				string desc = (attr.Description != null ? attr.Description : DefaultPacketDescriptions.GetDescription(attr.Code, attr.Direction));
				return string.Format("{0} 0x{1:X2}: \"{2}\"", dir, attr.Code, desc);
			}

			///<summary>
			///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
			///</summary>
			///
			///<returns>
			///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
			///</returns>
			///
			///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
			public override bool Equals(object obj)
			{
				bool ret = false;
				PacketClass p = obj as PacketClass;
				if (p != null)
				{
					// Compare all fields
					ret = (p.attr == null ? attr == null : p.attr.Equals(attr) && p.type.Equals(type));
				}
				return ret;
			}

			///<summary>
			///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
			///</summary>
			///
			///<returns>
			///A hash code for the current <see cref="T:System.Object"></see>.
			///</returns>
			///<filterpriority>2</filterpriority>
			public override int GetHashCode()
			{
				int ret = attr.GetHashCode() + type.GetHashCode();
				return ret;
			}
		}
	}
}
