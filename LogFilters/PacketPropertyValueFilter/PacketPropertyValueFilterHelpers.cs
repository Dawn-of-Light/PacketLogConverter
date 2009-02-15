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
#warning TODO: Move somewhere where it fits better
		internal static readonly int RECURSIVE_SEARCH_DEPTH = 6;

		/// <summary>
		/// Gets paths to every property and field a class.
		/// </summary>
		/// <param name="clazz">The class to read properties for.</param>
		/// <param name="depth">How deep to read properties/fields. Zero means only properties/fields of specified class.</param>
		/// <param name="flags">The flags of property and field to read.</param>
		/// <param name="result">List with all paths.</param>
		/// <param name="curList">Path to current node.</param>
		internal static void GetPropertiesAndFields(Type clazz, int depth, BindingFlags flags, List<List<MemberInfo>> result, List<MemberInfo> curList)
		{
			if (depth < 0)
			{
				return;
			}
			if (IsClassIgnored(clazz))
			{
				return;
			}

			// Get properties
			PropertyInfo[] properties = clazz.GetProperties(flags);
			foreach (PropertyInfo info in properties)
			{
				// Clone current path, add this node if property is readable and is not indexer ("item[..]")
				if (!IsClassIgnored(info.PropertyType) && info.CanRead && info.GetIndexParameters().GetLength(0) == 0)
				{
					List<MemberInfo> newList = new List<MemberInfo>(curList);
					newList.Add(info);

					// Get sub-element type; can be array
					Type propType = info.PropertyType;
					string name = info.Name;
					name.Clone();
					if (propType.HasElementType)
					{
						propType = propType.GetElementType();
					}
					result.Add(newList);

					// Special case - no sub-properties of String types
					if (propType != typeof(string))
					{
						GetPropertiesAndFields(propType, depth - 1, flags, result, newList);
					}
				}
			}

			// Get fields
			FieldInfo[] fields = clazz.GetFields(flags);
			foreach (FieldInfo info in fields)
			{
				// Clone current path, add this node
				if (!IsClassIgnored(info.FieldType))
				{
					List<MemberInfo> newList = new List<MemberInfo>(curList);
					newList.Add(info);

					// Get sub-element type; can be array
					Type fieldType = info.FieldType;
					if (fieldType.HasElementType)
					{
						fieldType = fieldType.GetElementType();
					}
					result.Add(newList);

					// Special case - no sub-properties of String types
					if (fieldType != typeof(string))
					{
						GetPropertiesAndFields(fieldType, depth - 1, flags, result, newList);
					}
				}
			}
		}

		/// <summary>
		/// Determines whether class is ignored.
		/// </summary>
		/// <param name="classType">Type of the class.</param>
		/// <returns>
		/// 	<c>true</c> if class is ignored; otherwise, <c>false</c>.
		/// </returns>
		internal static bool IsClassIgnored(Type classType)
		{
			return classType == typeof(TimeSpan)
				|| classType == typeof(LogPacketAttribute)
//				|| valType == typeof(string)
				|| classType == typeof(ePacketProtocol)
				|| classType == typeof(ePacketDirection)
				|| typeof(Exception).IsAssignableFrom(classType)
				|| typeof(IDictionary).IsAssignableFrom(classType);
//				|| typeof(IList).IsAssignableFrom(classType);
		}

		#region data containers


		#endregion
	}
}
